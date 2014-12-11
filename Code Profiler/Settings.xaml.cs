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
        public Settings()
        {
            InitializeComponent();
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

        
    }
}
