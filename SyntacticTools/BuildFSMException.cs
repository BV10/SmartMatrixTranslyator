using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntacticTools
{
    class BuildFSMException   : Exception
    {
        public BuildFSMException(string message)
            : base(message)
        { }
    }
}
