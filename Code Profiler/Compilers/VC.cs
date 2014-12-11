using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Code_Profiler.Compilers
{
    internal class VC:Code_Profiler.Compiler
    {
        static Process p = null;

        public void Compile(string src, string output)
        {
            if (File.Exists(output)) File.Delete(output);
            if (p == null)
            {
                p = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "cmd.exe";
                info.RedirectStandardInput = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;
                p.StartInfo = info;
                p.Start();
                p.StandardInput.WriteLine("\"" + DEFAULT_LOCATION + "\\vcvarsall.bat\" x86");
                //while (p.StandardOutput.Peek()>0) p.StandardOutput.ReadLine();
            }
            

            
            p.StandardInput.WriteLine("\"" + DEFAULT_LOCATION + "\\bin\\cl.exe\" " + src + " /o " + output);
            p.StandardInput.WriteLine("echo COMPLETED!");
            //System.Threading.Thread.Sleep(1000);
            bool flag = false;
            string s ="";//= p.StandardOutput.ReadLine();
            while (true)
            {
                s = p.StandardOutput.ReadLine();
                if (s.Contains("COMPLETED!"))
                {
                    if (flag) break;
                    else flag = true;
                }
            }
            //if (flag) while (!File.Exists(output)) ;
            
            
        }
        public async Task<StringBuilder> Run(string location_to_use, StringBuilder src, StringBuilder test, string arg="")
        {
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
            p.StandardInput.Write(test+"");

            sb.Append(await p.StandardOutput.ReadToEndAsync());
            return sb;

        }
        public static string DEFAULT_LOCATION
        {
            get { return Environment.ExpandEnvironmentVariables( Properties.Settings.Default.VC_location); }
        }

        public Task<StringBuilder> CompileAndRun(string location_to_use, StringBuilder src, StringBuilder test, string arg="")
        {
            using (var f = new System.IO.StreamWriter(location_to_use + ".cpp"))
            {
                f.Write(src);
            }
            Compile(location_to_use + ".cpp", location_to_use  + ".exe");
            return Run(location_to_use, src, test, arg);
        }


        public bool FindCompiler()
        {
            return File.Exists(DEFAULT_LOCATION + "\\vcvarsall.bat") && File.Exists(DEFAULT_LOCATION + "\\bin\\cl.exe");
        }


        public bool Enabled
        {
            get { return Properties.Settings.Default.VC_enable; }
        }


        public string Name
        {
            get { return "VC++"; }
        }


        public string[] FileExtensions
        {
            get { return new string[] { "cpp", "c" }; }
        }


        public string HighlighterKey
        {
            get { return "cpp"; }
        }
    }
}
