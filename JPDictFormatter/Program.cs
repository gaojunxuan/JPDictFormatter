using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPDictFormatter
{
    class Program
    {
        static void Main(string[] args)
        {
            Formatter formatter = new Formatter();
            //var result = formatter.ParseExplanationString("か\n\n科\n[ 名 ] 科系\n\nしな\n\n科\n[ 名 ] 娇态\n\nか\n科\n[ 造語 ] 专业；学科；（生物分类上的）科；规定\n\nとが\n\n科\n[ 名 ] 过错；错误（同あやまち）；罪；罪过（同つみ）");
            //var result = formatter.ParseExplanationString("か\n[ 併助 ] 表示任选其一；表示不肯定的指明或笼统的选择；表示刚...就；表示是不是；是否\n[ 副助 ] 表示不定；表示不确切的推断；也许是；说不定；表示刚一...就；本以为...不料想；忽...忽；表示似乎；好像\n[ 終助 ] 表示疑问；表示反问；表示责难；表示劝诱；征求同意；表示叮咛；提醒注意；表示自言自语或重复别人的话；表示惊讶；感叹\n");
            //var result = formatter.ParseExplanationString("こ\n[ 接頭 ] 小；细小；微少；一点；可憎；讨厌；有点...\n\n[ 接頭 ] 小；细小；微少；一点；可憎；讨厌；有点...");
            //var grouped = formatter.CreateGroupedList(result, 50);
            //ひ\n\n火\n[名] 火；火焰；灯光；火警；火灾；愤怒之火\n\nか\n\n火\n[名] 火（五行之一）
            //foreach (var i in grouped)
            //{
            //    System.Diagnostics.Debug.WriteLine(i.DefinitionId + ", " + i.ItemId + ", " + i.Reading + ", " + i.Kanji + ", " + i.LoanWord + ", " + i.Pos + ", " + i.Definition + ", " + i.SeeAlso);
            //}
            //Console.ReadLine();


            Console.Write("Source path: ");
            string sourcePath = Console.ReadLine();
            Console.Write("Target path: ");
            string targetPath = Console.ReadLine();
            formatter.FormatDatabase(sourcePath, targetPath);
            Console.ReadLine();
        }
    }
}
