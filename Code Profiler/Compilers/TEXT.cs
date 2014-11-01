using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Profiler.Compilers
{
    internal class TEXT:Code_Profiler.Compiler
    {

        public string DEFAULT_LOCATION
        {
            get { throw new NotImplementedException(); }
        }

        public async Task<StringBuilder> Run(string location_to_use, StringBuilder src, StringBuilder test, string arg = "")
        {
            return src;
        }

        public async Task<StringBuilder> CompileAndRun(string location_to_use, StringBuilder src, StringBuilder test, string arg="")
        {
            return src;
        }
    }
}
