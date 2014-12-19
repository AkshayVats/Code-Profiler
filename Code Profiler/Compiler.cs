using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Profiler
{
    
    internal interface Compiler
    {
        Task<StringBuilder> Run(string location_to_use, StringBuilder src, StringBuilder test, string arg = "");
        Task<string > Compile(string location_to_use, StringBuilder src);
        bool FindCompiler();
        bool Enabled { get; }
        string Name { get; }
        string[] FileExtensions { get; }
        string HighlighterKey { get; }
    }
    
}
