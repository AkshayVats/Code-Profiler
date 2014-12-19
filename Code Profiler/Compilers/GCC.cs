using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Code_Profiler.Compilers
{
    internal class GCC:Compiler
    {
        public string Compile(string src, string output)
        {
            if (File.Exists(output)) File.Delete(output);
            var p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = DEFAULT_LOCATION;
            info.Arguments = "\""+src+"\" -o \"" + output + "\"";
            info.WorkingDirectory = DEFAULT_LOCATION.Replace("g++.exe", "");
            //info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.RedirectStandardError = true;
            p.StartInfo = info;
            p.Start();
            p.WaitForExit();
            return p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
        }
        public static string DEFAULT_LOCATION
        {
            get { return Environment.ExpandEnvironmentVariables(Properties.Settings.Default.gcc_location); }
        }
        public async Task<StringBuilder> Run(string location_to_use, StringBuilder src, StringBuilder test, string arg = "")
        {
            if (!File.Exists(location_to_use + ".exe"))
                return new StringBuilder("Unable to compile and run");
            StringBuilder sb = new StringBuilder();
            Process p = new Process();
            p.StartInfo.FileName = location_to_use + ".exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = arg;
            p.StartInfo.RedirectStandardInput = p.StartInfo.RedirectStandardOutput = true;
            foreach (var v in Process.GetProcesses().Where(i => i.StartInfo.FileName == p.StartInfo.FileName))
                v.Kill();
            p.Start();
            p.StandardInput.Write(test + "");

            sb.Append(await p.StandardOutput.ReadToEndAsync());
            return sb;

        }

        public async Task<string> Compile(string location_to_use, StringBuilder src)
        {
            using (var f = new System.IO.StreamWriter(location_to_use + ".cpp"))
            {
                f.Write(src);
            }
            string s=null;
            await Task.Run(() => { s = Compile(location_to_use + ".cpp", location_to_use + ".exe"); });
            return s;
        }

        public bool FindCompiler()
        {
            return File.Exists(DEFAULT_LOCATION);
        }

        public bool Enabled
        {
            get { return Properties.Settings.Default.gcc_enable; }
        }

        public string Name
        {
            get { return "GCC(G++)"; }
        }
        
        public string[] FileExtensions
        {
            get { return new string[] { ".c", ".cpp" }; }
        }

        public string HighlighterKey
        {
            get { return "C++"; }
        }
    }
}
