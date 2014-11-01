using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Code_Profiler.Analyzer
{
    internal class BasicAnalyzer
    {
        
        public int Compare(StringBuilder s1, StringBuilder s2)
        {
            
            int line = 1;
            for (int i = 0; i < s1.Length && i < s2.Length; i++)
            {
                if (s1[i] != s2[i]) return line;
                if (s1[i] == '\n') line++;
            }
            if (s1.Length != s2.Length) return line;
            return 0;
        }
    }
}
