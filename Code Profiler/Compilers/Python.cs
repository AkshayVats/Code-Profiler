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
            p.StartInfo.FileName = DEFAULT_LOCATION;
            p.StartInfo.Arguments = "\""+location_to_use + ".py\"";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardInput = p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            p.StandardInput.Write(test + "");

            sb.Append(await p.StandardOutput.ReadToEndAsync());
            return sb;

        }

        public static string DEFAULT_LOCATION
        {
            get {  return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.Python_location); }
        }




        public async Task<string > Compile(string location_to_use, StringBuilder src)
        {
            using (var f = new System.IO.StreamWriter(location_to_use + ".py"))
            {
                f.Write(src);
            }
            return "";
        }


        public bool FindCompiler()
        {
            return System.IO.File.Exists(DEFAULT_LOCATION);
        }


        public bool Enabled
        {
            get { return Properties.Settings.Default.Python_enable; }
        }


        public string Name
        {
            get { return "Python"; }
        }


        public string[] FileExtensions
        {
            get { return new string[] { "py" }; }
        }


        public string HighlighterKey
        {
            get { return "Python"; }
        }
    }

}
