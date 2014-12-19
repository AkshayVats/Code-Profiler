using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Code_Profiler
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings(bool startup)
        {
            InitializeComponent();
            AttemptFindCompiler(new Compilers.VC(), GetVisualStudioInstallationPath, tb1, chk1);
            AttemptFindCompiler(new Compilers.Python(), GetPythonPath, tb2, chk2);
            AttemptFindCompiler(new Compilers.GCC(), GetGCCPath, tb3, chk3);
            if (startup)
            {
                this.Close();
            }
            
            
        }
        private bool AttemptFindCompiler(Compiler c, Func<string> finder, TextBox tb, CheckBox chk)
        {
            try
            {
                if (c.FindCompiler())
                {
                    tb.Background = Brushes.LightGreen;
                    chk.IsChecked = true;
                    return true;
                }
                tb.Text = finder();
                if (c.FindCompiler())
                {
                    tb.Background = Brushes.LightGreen;
                    chk.IsChecked = true;
                    return true;
                }
            }
            catch { }
            chk.IsChecked = false;
            tb.Background = Brushes.IndianRed;
            return false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description="Select %ProgramFiles(x86)%\\Microsoft Visual Studio 12.0\\VC equivalent folder";
            fbd.SelectedPath = Environment.ExpandEnvironmentVariables( tb1.Text);
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                 tb1.Text = fbd.SelectedPath; 
            }
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            Compilers.VC vc = new Compilers.VC();
            bool f = vc.FindCompiler();
            if (f) tb1.Background = Brushes.LightGreen;
            else tb1.Background = Brushes.Ivory;
            MessageBox.Show("Compiler location is " + (f ? "correct" : "incorrect"));
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            Compilers.Python c = new Compilers.Python();
            bool f = c.FindCompiler();
            MessageBox.Show("Compiler location is " + (f ? "correct" : "incorrect"));
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fbd = new System.Windows.Forms.OpenFileDialog();
            fbd.Title = "Select C:\\Python27\\python.exe equivalet";
            fbd.InitialDirectory = "C:\\Python27";
            fbd.Filter = "python.exe|python.exe";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb1.Text = fbd.FileName;
            }
        }

        private string GetVisualStudioInstallationPath()
        {
            string[] ver = new string[] { "13.0", "12.0", "10.0", "9.0" };
            string installationPath = null;
                
            foreach (string v in ver)
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    installationPath = (string)Microsoft.Win32.Registry.GetValue(
                       "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\VisualStudio\\" + v + "\\",
                        "InstallDir",
                        null);
                }
                else
                {
                    installationPath = (string)Microsoft.Win32.Registry.GetValue(
               "HKEY_LOCAL_MACHINE\\SOFTWARE  \\Microsoft\\VisualStudio\\" + v + "\\",
                      "InstallDir",
                      null);
                }
                if (installationPath != null) return installationPath.Replace("Common7\\IDE\\", "VC");
            }
            return installationPath;

        }
        private string GetPythonPath()
        {
            var k = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64).OpenSubKey("software").OpenSubKey("python").OpenSubKey("pythoncore").OpenSubKey("2.7").OpenSubKey("installpath").GetValue(null);
            if(k!=null)
            return new System.IO.FileInfo(k as string + "\\python.exe").FullName;
            return null;
        }
        private string GetGCCPath()
        {
            var r= Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Dev-C++", "DisplayIcon", null);
            if(r!=null)return (r as string).Replace("devcpp.exe", @"MinGW64\bin\g++.exe");
            return null;
        }
        private void b4_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fbd = new System.Windows.Forms.OpenFileDialog();
            fbd.Title = @"Select *\MinGW*\bin\g++.exe equivalet";
            fbd.InitialDirectory = "";
            fbd.Filter = "g++.exe|g++.exe";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb1.Text = fbd.FileName;
            }
        }

        private void b5_Click(object sender, RoutedEventArgs e)
        {
            Compilers.GCC c = new Compilers.GCC();
            bool f = c.FindCompiler();
            MessageBox.Show("Compiler location is " + (f ? "correct" : "incorrect"));
        }
    }
}
