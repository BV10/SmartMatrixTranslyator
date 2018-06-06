using System.Collections.Generic;

namespace SyntacticTools
{
    public class Function
    {
        public class Param
        {
            public string Type { get; set; }
            public string Name { get; set; }
        }

        public string Name { get; set; }
        public List<Param> ParamsList { get; set; }
        public string ReturnType { get; set; }
    }
}