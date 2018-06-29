using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPDictFormatter
{
    public class MainDict
    {
        public int ID { get; set; }
        public string JpChar { get; set; }
        public string Kana { get; set; }
        public string Explanation { get; set; }
        public bool IsInUserDefDict { get; set; }
    }
}
