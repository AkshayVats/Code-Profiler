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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Code_Profiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            src1.Text = Properties.Settings.Default.src1;
            src2.Text=Properties.Settings.Default.src2;
            src3.Text = Properties.Settings.Default.src3;
            src4.Text = Properties.Settings.Default.src4;
            cb1.Text = Properties.Settings.Default.typ1;
            cb2.Text = Properties.Settings.Default.typ2;
            cb3.Text = Properties.Settings.Default.typ3;
            cb4.Text = Properties.Settings.Default.typ4;
            
            src1.CurrentHighlighter = AurelienRibon.Ui.SyntaxHighlightBox.HighlighterManager.Instance.Highlighters["cpp"];
            src2.CurrentHighlighter = AurelienRibon.Ui.SyntaxHighlightBox.HighlighterManager.Instance.Highlighters["cpp"];
            src3.CurrentHighlighter = AurelienRibon.Ui.SyntaxHighlightBox.HighlighterManager.Instance.Highlighters["cpp"];
            src4.CurrentHighlighter = AurelienRibon.Ui.SyntaxHighlightBox.HighlighterManager.Instance.Highlighters["cpp"];
        }

        private void OpenSource(TextBox src, ComboBox cb)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                using (var f = System.IO.File.OpenText(ofd.FileName))
                {
                    src.Text = f.ReadToEnd();
                }
                cb.Text = FileHelper.Tell(ofd.FileName);
            }
        }
        private Compiler FindCompiler(string name)
        {
            switch (name)
            {
                case "VC++":
                    return new Compilers.VC();
                case "TEXT": return new Compilers.TEXT();
            }
            return null;
        }
        private void b1_Click(object sender, RoutedEventArgs e)
        {
            OpenSource(src1, cb1);
            chk1.IsChecked = true;
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            OpenSource(src2, cb2);
            chk2.IsChecked = true;
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            OpenSource(src3, cb3);
        }

        private async void bAnalyze_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.src1 = src1.Text;
            Properties.Settings.Default.src2 = src2.Text;
            Properties.Settings.Default.src3 = src3.Text;
            Properties.Settings.Default.src4 = src4.Text;
            Properties.Settings.Default.typ1 = cb1.Text;
            Properties.Settings.Default.typ2 = cb2.Text;
            Properties.Settings.Default.typ3 = cb3.Text;
            Properties.Settings.Default.typ4 = cb4.Text;
            Properties.Settings.Default.Save();
            
            var analyzer = new Analyzer.BasicAnalyzer();
            var testFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\src3";
            var src1File = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\src1";
            var src2File = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\src2";
            var driverFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\src4";

            var c1 = FindCompiler(cb1.Text);
            var c2 = FindCompiler(cb2.Text);
            var c3 = FindCompiler(cb3.Text);
            var c4 = FindCompiler(cb4.Text);

            StringBuilder ot1, ot2, ot3, ot4;
            StringBuilder a1, a2, a3, a4;
            System.Diagnostics.Stopwatch sw1, sw2;
            sw1=new System.Diagnostics.Stopwatch();
            sw2=new System.Diagnostics.Stopwatch();

            a1 = new StringBuilder(src1.Text);
            a2 = new StringBuilder(src2.Text);
            a3 = new StringBuilder(src3.Text);
            a4 = new StringBuilder(src4.Text);

            lStatus.Content = "Compiling Driver program";
            if(chk4.IsChecked==true)
            ot4 = await c4.CompileAndRun(driverFile, a4, new StringBuilder(""));
            else ot4 = await c4.Run(driverFile, a4, new StringBuilder(""));

            chk4.IsChecked = null;

            System.IO.StringReader sr = new StringReader(ot4.ToString());
            int TESTS = int.Parse(sr.ReadLine());
            while (spResult.Children.Count > 1) spResult.Children.RemoveAt(1);
            prg.Maximum = TESTS;
            for (int i = 0; i < TESTS; i++)
            {
                prg.Value = i;
                var cmdLine = sr.ReadLine();
                if (chk3.IsChecked == true)
                {
                    lStatus.Content = "Running Test Generator (" + i + ")";

                    ot3 = await c1.CompileAndRun(testFile, a3, new StringBuilder(""), cmdLine);
                    chk3.IsChecked = null;
                }
                else ot3 = await c1.Run(testFile, a3, new StringBuilder(""), cmdLine);

                sw1.Restart();
                if (chk1.IsChecked == true)
                {
                    lStatus.Content = "Running Program 1";
                    ot1 = await c1.CompileAndRun(src1File, a1, ot3);
                    chk1.IsChecked = null;
                }
                else ot1 = await c1.Run(src1File, a1, ot3);
                sw1.Stop();
                sw2.Restart();
                if (chk2.IsChecked == true)
                {
                    lStatus.Content = "Running Program 2";
                    ot2 = await c2.CompileAndRun(src2File, a2, ot3);
                    chk2.IsChecked = null;
                }
                else ot2 = await c2.Run(src2File, a2, ot3);
                sw2.Stop();
                if (chk1.IsChecked != false && chk2.IsChecked != false)
                {
                    ResultRow rr = new ResultRow()
                    {
                        Test = new ResultRow.Program() { display=i, snippet=ot3},
                        Program1 = new ResultRow.Program() { display=sw1.ElapsedMilliseconds, snippet=ot1},
                        Program2 = new ResultRow.Program() { display = sw2.ElapsedMilliseconds, snippet = ot2 },
                        Performance = new ResultRow.Program() { display =(long)( (sw1.ElapsedMilliseconds - sw2.ElapsedMilliseconds) * 1000.0 / sw2.ElapsedMilliseconds) }
                    };
                    rr.Refresh();
                    spResult.Children.Add(rr);
                    //txtOut.Clear();
                    //txtOut.AppendText("Input\n"+ot3+"Output 1\n" + ot1 + "\nOutput 2\n" + ot2+"\n\n");
                    lStatus.Content = "Analyzing..";
                    var result = analyzer.Compare(ot1, ot2);
                    if (result == 0) { lStatus.Content = "AC!"; }
                    else
                    {
                        MessageBox.Show("Mismatch on line " + result); lStatus.Content = result + "";
                        break;
                    }

                }
            }
            prg.Value = 0;
            
            
        }

        private void bTemplate_Click(object sender, RoutedEventArgs e)
        {
            src4.Text = "";
            src4.AppendText(Properties.Resources.cpp_driver);
        }

        private void src1_TextChanged(object sender, TextChangedEventArgs e)
        {
            chk1.IsChecked = true;
        }

        private void src2_TextChanged(object sender, TextChangedEventArgs e)
        {
            chk2.IsChecked = true;
        }

        private void src3_TextChanged(object sender, TextChangedEventArgs e)
        {
            chk3.IsChecked = true;
        }

        private void src4_TextChanged(object sender, TextChangedEventArgs e)
        {
            chk4.IsChecked = true;
        }

        
    }
}
