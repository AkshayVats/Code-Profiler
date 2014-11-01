using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Code_Profiler.Compilers
{
    public class Python:Code_Profiler.Compiler
    {

        public async Task<StringBuilder> Run(string location_to_use, StringBuilder src, StringBuilder test, string arg = "")
        {
            StringBuilder sb = new StringBuilder();
            Process p = new Process();
            p.StartInfo.FileName = location_to_use+".py";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            p.StandardInput.Write(test + "");

            sb.Append(await p.StandardOutput.ReadToEndAsync());
            return sb;

        }

        public string DEFAULT_LOCATION
        {
            get { return ""; }
        }

        
       

        public Task<StringBuilder> CompileAndRun(string location_to_use, StringBuilder src, StringBuilder test, string arg="")
        {
            using (var f = new System.IO.StreamWriter(location_to_use + ".py"))
            {
                f.Write(src);
            }
            return Run(location_to_use , src, test);
        }
    }

}
