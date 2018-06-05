using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexicalTools;

namespace SyntacticTools
{
    public class RecordTableLL1
    {
        public List<string> ExpectedLexems { get; set; }
        public int NextState { get; set; }
        public bool Accept { get; set; }
        public int? ToStack { get; set; }
        public bool FromStack { get; set; }
        public bool Error { get; set; }
        public string NameOfAction { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            ExpectedLexems.ForEach((string lexem) => stringBuilder.Append(lexem + " "));
            stringBuilder.Append("\t");

            stringBuilder.Append(NextState);
            stringBuilder.Append("\t");

            stringBuilder.Append((Accept ? "+" : "-"));
            stringBuilder.Append("\t");

            stringBuilder.Append((ToStack));
            stringBuilder.Append("\t");

            stringBuilder.Append((FromStack ? "+" : "-"));
            stringBuilder.Append("\t");

            stringBuilder.Append((Error ? "+" : "-"));
            stringBuilder.Append("\t");

            return stringBuilder.ToString();
        }
    }
}
