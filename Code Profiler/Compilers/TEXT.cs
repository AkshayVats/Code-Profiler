using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Profiler.Compilers
{
    internal class TEXT:Code_Profiler.Compiler
    {

        

        public async Task<StringBuilder> Run(string location_to_use, StringBuilder src, StringBuilder test, string arg = "")
        {
            return src;
        }

        public async Task<string> Compile(string location_to_use, StringBuilder src)
        {
            return "";
        }


        public bool FindCompiler()
        {
            return true;
        }


        public bool Enabled
        {
            get { return true; }
        }


        public string Name
        {
            get { return "TEXT"; }
        }


        public string[] FileExtensions
        {
            get { return new string[] { "txt" }; }
        }


        public string HighlighterKey
        {
            get { return "TEXT"; }
        }
    }
}
