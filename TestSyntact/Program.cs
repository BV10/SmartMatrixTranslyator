using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntacticTools;

namespace TestSyntact
{
    class Program
    {
        static void Main(string[] args)
        {
            FiniteStateMachine finiteStateMachine = new FiniteStateMachine(new StreamReader(File.OpenRead("grammar.txt")), "Program");
            finiteStateMachine.BuildMachine();
            finiteStateMachine.SaveNumericGrammar("NumericGrammar");
            finiteStateMachine.SaveTableParse("TableParseLL1");
        }
    }
}
