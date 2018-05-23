using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SyntacticTools
{
    public class Rule
    {
        public class LeftPart
        {
            public string Name { get; set; }
            public int Number { get; set; }

            public override bool Equals(object obj)
            {
                return Name == obj.ToString();
            }
        }
        public LeftPart LeftPartRule { get; set; }



        public class RightPart
        {
            public string[] Terms { get; set; }
            public int[] Numbers { get; set; }
        }
        public RightPart RightPartRule { get; set; }

        public override string ToString()
        {
            string res = LeftPartRule.Name + " ("+ LeftPartRule.Number + ") ->";
            for (int i = 0; i < RightPartRule.Terms.Length; i++)
            {
                res += RightPartRule.Terms[i] + " (";
                if (RightPartRule.Numbers != null && RightPartRule.Numbers.Length == RightPartRule.Terms.Length)
                    res += RightPartRule.Numbers[i] + ") ";
            }

            return res;
        }
    }
}
