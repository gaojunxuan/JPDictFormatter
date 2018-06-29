using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SQLite;

namespace JPDictFormatter
{
    public class Formatter
    {
        public List<Dict> ParseExplanationString(string explanationStr)
        {
            string[] lines = explanationStr.Split('\n');
            var result = new List<Dict>();
            string reading = "";
            string kanji = "";
            string loanWord = "";
            Regex pattern = new Regex(@"(\[.*\])+");
            Regex seealsoPattern = new Regex(@"(（同.*?）)");
            Regex loanhWordPattern = new Regex(@"(（.*?）).*");
            if(!string.IsNullOrWhiteSpace(lines[0]))
            {
                reading = lines[0];
            }
            //Loan words
            if(!string.IsNullOrWhiteSpace(lines[1])&&lines.Count()>2&&loanhWordPattern.IsMatch(lines[1]))
            {
                loanWord = lines[1];
                explanationStr = lines[2];
                result.Add(new Dict() { Definition = explanationStr, LoanWord = loanWord });
            }
            //Japanese words
            else
            {
                if (string.IsNullOrWhiteSpace(lines[1]))
                {
                    kanji = lines[2];
                }
                else
                {
                    if (!lines[1].Contains("["))
                    {
                        kanji = lines[1];
                    }
                }
                int index = 0;
                foreach (var i in lines)
                {
                    if (pattern.IsMatch(i))
                    {
                        string pos = pattern.Match(i).Value;
                        string explanation = pattern.Replace(i, "").Trim();
                        string seealso = "";
                        if (index > 1)
                        {
                            if(!string.IsNullOrEmpty(lines[0]))
                            {
                                if (string.IsNullOrWhiteSpace(lines[index - 2]))
                                {
                                    reading = lines[index - 3];
                                }
                                else
                                {
                                    if (!lines[index - 2].Contains("["))
                                    {
                                        reading = lines[index - 2];
                                    }
                                }
                            }
                        }
                        if (seealsoPattern.IsMatch(explanation))
                        {
                            var seealsoMatches = seealsoPattern.Matches(explanation);
                            foreach (var m in seealsoMatches)
                            {
                                seealso += m.ToString().Replace("（同", "").Replace("）", "") + " ";
                            }
                            explanation = seealsoPattern.Replace(explanation, "");
                        }
                        result.Add(new Dict() { Definition = explanation, Pos = pos, Keyword = kanji, Reading = reading, SeeAlso = seealso });
                    }
                    index++;
                }
            }
            
            return result;
        }
        public List<Dict> CreateGroupedList(IList<Dict> items,int itemId)
        {
            var groupedItems = items.GroupBy(x => x.Reading);
            foreach(var group in groupedItems)
            {
                string reading = group.Key;
                foreach(var i in group)
                {
                    i.ItemId = itemId;
                }
                itemId++;
            }
            return groupedItems.SelectMany(group => group).ToList();
        }
        public void FormatDatabase(string sourcePath, string targetPath)
        {
            SQLiteConnection sourceConnection = new SQLiteConnection(sourcePath);
            SQLiteConnection targetConnection = new SQLiteConnection(targetPath);
            var table = sourceConnection.Table<MainDict>();
            targetConnection.CreateTable<Dict>();
            int counter = 1;
            foreach (var i in table)
            {
                string reading = i.Kana;
                if(string.IsNullOrEmpty(reading))
                {
                    reading = i.JpChar;
                }
                var items = ParseExplanationString(i.Explanation);
                var grouped = CreateGroupedList(items,counter);
                foreach(var g in grouped)
                {
                    if (!string.IsNullOrEmpty(g.Reading)&&g.Reading != reading)
                        reading = g.Reading;
                    targetConnection.Insert(new Dict() { ItemId = counter, Keyword = i.JpChar, Reading = reading.Replace("·", ""), Pos = g.Pos, Definition = g.Definition, LoanWord = g.LoanWord, SeeAlso = g.SeeAlso });
                }
                Console.WriteLine(counter);
                counter++;
            }
        }
    }
}
