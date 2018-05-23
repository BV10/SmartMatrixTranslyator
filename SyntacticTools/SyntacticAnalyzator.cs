using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LexicalTools;
using System.Text.RegularExpressions;

namespace SyntacticTools
{
    public class SyntacticAnalyzator
    {      
        public List<Lexem> Lexems { get; set; } // list lexems 
        public List<SyntaxError> SyntacticErrors { get; } // error of syntax      

        public SyntacticAnalyzator(StreamReader streamReader, List<Lexem> lexems)
        {
            //StreamReader = streamReader;
            //Lexems = lexems;
        }

        public SyntacticAnalyzator() { }

        bool SyntacticAnalysis()
        {            

           // BuildMachine(streamReader, ); // build machine of shop type            

            return false;
        }

        private void BuildMachine(StreamReader streamReader, string startNotTerminal)
        {
            // read all rules from file
            List<string> grammars = new List<string>();
            while (!streamReader.EndOfStream)
                grammars.Add(streamReader.ReadLine());
            //
        }
    }
}
