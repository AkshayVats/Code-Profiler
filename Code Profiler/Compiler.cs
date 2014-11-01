using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Profiler
{
    
    internal interface Compiler
    {
        string DEFAULT_LOCATION { get; }
        Task<StringBuilder> Run(string location_to_use, StringBuilder src, StringBuilder test, string arg = "");
        Task<StringBuilder> CompileAndRun(string location_to_use, StringBuilder src, StringBuilder test, string arg="");
    }
}
