using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Profiler
{
    public class FileHelper
    {
        public static string Tell(string FileName)
        {
            FileName = FileName.ToLower();
            if (FileName.EndsWith(".cpp"))
                return "VC++";
            else if (FileName.EndsWith(".java"))
                return "JAVA";
            else if (FileName.EndsWith(".py"))
                return "PYTHON";
            else
                return "TEXT";
        }
    }

}
