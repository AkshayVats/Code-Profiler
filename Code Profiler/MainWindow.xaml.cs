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
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Run = new RoutedUICommand
                (
                        "Run",
                        "Run",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                                {
                                        new KeyGesture(Key.F5, ModifierKeys.None)
                                }
                );

        //Define more commands here, just like the one above
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        internal static List<Compiler> compilers = new List<Compiler>();
        List<SourceTab> sources = new List<SourceTab>();
        string Current_Environment;
        private void LoadCompilers()
        {
            foreach (var t in this.GetType().Assembly.GetTypes().Where(i => i.GetInterfaces().Contains(typeof(Compiler))))
            {
                compilers.Add(Activator.CreateInstance(t) as Compiler);
            }
        }

        void VerifySettings()
        {

            if (compilers.Any(i => i.Enabled && !i.FindCompiler()))
            {
                new Settings(true);
                VerifySettings();
                return;
            }
            cb3.Items.Clear();
            cb4.Items.Clear();
            foreach (var s in sources) s.compiler.Items.Clear();
            foreach (var t in compilers)
                if (t.Enabled)
                {
                    foreach (var s in sources) s.compiler.Items.Add(t.Name);
                    cb3.Items.Add(t.Name);
                    cb4.Items.Add(t.Name);
                }
            //TODO:something wrong with cb.text?
        }
        private void SaveEnvironment(string fn)
        {
            
            using (StreamWriter sw = new StreamWriter(fn))
            {
                sw.WriteLine("CosmicCreations.Code_Profiler.Environment");
                sw.WriteLine(cb3.Text);
                sw.WriteLine(src3.Text.Length + "");
                if (src3.Text.Length != 0)
                    sw.WriteLine(src3.Text);
                sw.WriteLine(cb4.Text);
                sw.WriteLine(src4.Text.Length + "");
                if (src4.Text.Length != 0) sw.WriteLine(src4.Text);
                sw.WriteLine(sources.Count + "");
                foreach (var s in sources)
                {
                    sw.WriteLine((s.Parent as TabItem).Header);
                    sw.WriteLine(s.compiler.Text);
                    sw.WriteLine(s.src.Text.Length);
                    if (s.src.Text.Length != 0)
                        sw.WriteLine(s.src.Text);
                }
            }

        }
        private void RestoreEnvironment(string fn)
        {
            using (StreamReader sr = new StreamReader(fn))
            {
                if (sr.ReadLine() != "CosmicCreations.Code_Profiler.Environment")
                {
                    MessageBox.Show("Invalid file!");
                    return;
                }
                cb3.Text = sr.ReadLine();
                char[] c = new char[int.Parse(sr.ReadLine())];
                if (c.Length != 0)
                {
                    sr.Read(c, 0, c.Length);
                    sr.ReadLine();
                }
                src3.Text = new string(c);
                cb4.Text = sr.ReadLine();
                c = new char[int.Parse(sr.ReadLine())];
                if (c.Length != 0)
                {
                    sr.Read(c, 0, c.Length);
                    sr.ReadLine();
                }
                src4.Text = new string(c);
                int si = int.Parse(sr.ReadLine());
                while (si-- > 0)
                {
                    var header = sr.ReadLine();
                    var com = sr.ReadLine();
                    c = new char[int.Parse(sr.ReadLine())];
                    if (c.Length != 0)
                    {
                        sr.Read(c, 0, c.Length);
                        sr.ReadLine();
                    }
                    AddSourceTab(header, com, new string(c));
                }
            }
        }
        public MainWindow()
        {

            LoadCompilers();
            InitializeComponent();
            VerifySettings();


        }
        private TabItem AddSourceTab(string header, string compiler, string src)
        {
            TabItem ti = new TabItem();
            ti.HeaderTemplate = FindResource("TabHeaderTemplate") as DataTemplate;
            ti.Header = header;

            var st = new SourceTab();
            ti.Content = st;
            foreach (var c in compilers)
                st.compiler.Items.Add(c.Name);
            sources.Add(st);
            tc.Items.Add(ti);
            st.compiler.Text = compiler;
            st.src.Text = src;

            return ti;
        }
        private void AddSourceTab()
        {
            int i = 1;
            while (sources.Any(j => ((string)((TabItem)j.Parent).Header) == "Source" + i)) i++;
            AddSourceTab("Source" + i, "TEXT", "").Focus();

        }
        private void OpenSource(ICSharpCode.AvalonEdit.TextEditor src, ComboBox cb)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                using (var f = System.IO.File.OpenText(ofd.FileName))
                {
                    src.Text = f.ReadToEnd();
                }
                var compiler = compilers.FirstOrDefault(i => i.FileExtensions.Contains(new FileInfo(ofd.FileName).Extension));
                cb.Text = compiler == null ? "TEXT" : compiler.Name;
            }
        }
        internal static Compiler FindCompiler(string name)
        {
            var c = compilers.FirstOrDefault(i => i.Name == name);
            if (c == null) return compilers.FirstOrDefault(i => i.Name == "TEXT");
            return c;
        }

        private void b3_Click(object sender, RoutedEventArgs e)
        {
            OpenSource(src3, cb3);
        }

        private async void bAnalyze_Click(object sender, RoutedEventArgs e)
        {


            var analyzer = new Analyzer.BasicAnalyzer();
            var testFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Code_Profiler\\src3";
            var driverFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Code_Profiler\\src4";
            //var src1File = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Code_Profiler\\src1";
            //var src2File = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Code_Profiler\\src2";
            
            //var c1 = FindCompiler(cb1.Text);
            //var c2 = FindCompiler(cb2.Text);
            var c3 = FindCompiler(cb3.Text);
            var c4 = FindCompiler(cb4.Text);

            StringBuilder ot3, ot4;
            StringBuilder a3, a4;
            System.Diagnostics.Stopwatch sw;
            sw=new System.Diagnostics.Stopwatch();

            //a1 = new StringBuilder(src1.Text);
            //a2 = new StringBuilder(src2.Text);
            a3 = new StringBuilder(src3.Text);
            a4 = new StringBuilder(src4.Text);

            lStatus.Content = "Compiling Driver program";
            if (chk4.IsChecked == true)
            {
                bI2.Tag = await c4.Compile(driverFile, a4);
                ot4 = await c4.Run(driverFile, a4, new StringBuilder(""));
            }
            else ot4 = await c4.Run(driverFile, a4, new StringBuilder(""));
            if (ot4.ToString() == "")
            {
                MessageBox.Show("Invalid driver program!");
                lStatus.Content = "Terminated";
                return;
            }
            chk4.IsChecked = null;

            System.IO.StringReader sr = new StringReader(ot4.ToString());
            int TESTS;
            if (!int.TryParse(sr.ReadLine(), out TESTS))
            {
                MessageBox.Show("Invalid driver program");
                return;
            }
            while (spResult.Children.Count > 1) spResult.Children.RemoveAt(1);
            prg.Maximum = TESTS;
            var tg = spResult.Children[0] as Grid;
            while (tg.Children.Count > 1) tg.Children.RemoveAt(1);
            while (tg.ColumnDefinitions.Count > 1) tg.ColumnDefinitions.RemoveAt(1);
            foreach (var s in sources)
            {
                TextBlock t = new TextBlock();
                t.Foreground = Brushes.White;
                t.TextAlignment = TextAlignment.Center;
                t.Text = s.Header;
                tg.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(t, tg.Children.Count);
                tg.Children.Add(t);
            }
            bool flag = true;
            for (int i = 0; i < TESTS&&flag; i++)
            {
                ResultRow rr = new ResultRow();
                prg.Value = i;
                var cmdLine = sr.ReadLine();
                if (chk3.IsChecked == true)
                {
                    lStatus.Content = "Running Test Generator (" + i + ")";
                    bI1.Tag = await c3.Compile(testFile, a3);
                    ot3 = await c3.Run(testFile, a3, new StringBuilder(""), cmdLine);
                    chk3.IsChecked = null;
                }
                else ot3 = await c3.Run(testFile, a3, new StringBuilder(""), cmdLine);
                rr.Test = new ResultRow.Program() { display = i, snippet = ot3 };
                
                StringBuilder old=null;
                foreach (var s in sources)
                {
                    StringBuilder a, ot;
                    Compiler c = FindCompiler(s.compiler.Text);
                    string location = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Code_Profiler\\"+s.Header;
                    a = new StringBuilder(s.src.Text);
                    if (s.chk.IsChecked == true)
                    {
                        lStatus.Content = "Running " + s.Header;
                        s.CompilerResults = await c.Compile(location, a);
                        sw.Restart();
                        ot = await c.Run(location, a, ot3);
                        s.chk.IsChecked = null;
                    }
                    else ot = await c.Run(location, a, ot3);
                    sw.Stop();
                    rr.AddProgram(new ResultRow.Program() { display = sw.ElapsedMilliseconds, snippet = ot});
                    if (old != null)
                    {
                        var result = analyzer.Compare(old, ot);
                        if (result == 0) { lStatus.Content = "AC!"; }
                        else
                        {
                            MessageBox.Show("Mismatch on line " + result); lStatus.Content = result + "";
                            flag = false;
                            break;
                        }
                    }
                    old = ot;
                }
                spResult.Children.Add(rr);
            }
            prg.Value = 0;
                //if (chk1.IsChecked == true)
            //    {
            //        lStatus.Content = "Running Program 1";
            //        ot1 = await c1.CompileAndRun(src1File, a1, ot3);
            //        chk1.IsChecked = null;
            //    }
            //    else ot1 = await c1.Run(src1File, a1, ot3);
            //    sw1.Stop();
            //    sw2.Restart();
            //    if (chk2.IsChecked == true)
            //    {
            //        lStatus.Content = "Running Program 2";
            //        ot2 = await c2.CompileAndRun(src2File, a2, ot3);
            //        chk2.IsChecked = null;
            //    }
            //    else ot2 = await c2.Run(src2File, a2, ot3);
            //    sw2.Stop();
            //    if (chk1.IsChecked != false || chk2.IsChecked != false)
            //    {
            //        ResultRow rr = new ResultRow()
            //        {
            //            Test = new ResultRow.Program() { display=i, snippet=ot3},
            //            Program1 = new ResultRow.Program() { display=sw1.ElapsedMilliseconds, snippet=ot1},
            //            Program2 = new ResultRow.Program() { display = sw2.ElapsedMilliseconds, snippet = ot2 },
            //            Performance = new ResultRow.Program() { display =(long)( (sw1.ElapsedMilliseconds - sw2.ElapsedMilliseconds) * 1000.0 / sw2.ElapsedMilliseconds) }
            //        };
            //        rr.Refresh();
            //        spResult.Children.Add(rr);
            //        if (chk1.IsChecked != false && chk2.IsChecked != false)
            //        {
            //            //txtOut.Clear();
            //            //txtOut.AppendText("Input\n"+ot3+"Output 1\n" + ot1 + "\nOutput 2\n" + ot2+"\n\n");
            //            lStatus.Content = "Analyzing..";
            //            var result = analyzer.Compare(ot1, ot2);
            //            if (result == 0) { lStatus.Content = "AC!"; }
            //            else
            //            {
            //                MessageBox.Show("Mismatch on line " + result); lStatus.Content = result + "";
            //                break;
            //            }
            //        }
            //    }
            //}
            //prg.Value = 0;


        }

        private void bTemplate_Click(object sender, RoutedEventArgs e)
        {
            src4.Text = "";
            if (cb4.Text == "VC++" || cb4.Text == "GCC(G++)")
                src4.AppendText(Properties.Resources.cpp_driver);
            else if (cb4.Text == "Python") src4.AppendText(Properties.Resources.py_driver);
            else src4.AppendText(Properties.Resources.text_driver);
        }

        private void src3_TextChanged(object sender, EventArgs e)
        {
            chk3.IsChecked = true;
        }


        private void src4_TextChanged(object sender, EventArgs e)
        {
            chk4.IsChecked = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var s in sources) tc.Items.Remove(s.Parent);
        }

        private void miSettings_Click(object sender, RoutedEventArgs e)
        {
            new Settings(false).ShowDialog();
            VerifySettings();
        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = ((ComboBox)sender).SelectedValue as string;
            if (name != null && name != "")
            {
                var hb = ((ICSharpCode.AvalonEdit.TextEditor)FindName("src" + ((ComboBox)sender).Name.Replace("cb", "")));
                hb.SyntaxHighlighting =ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition( FindCompiler(name).HighlighterKey);
//                hb.Refresh();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            src3.Text = "";
            if (cb3.Text == "VC++")
                src3.AppendText(Properties.Resources.cpp_test);

        }

        private void b4_Click(object sender, RoutedEventArgs e)
        {
            OpenSource(src4, cb4);
        }


        private void Cross_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var c = sender as Cross;
            DependencyObject p = c;
            while (!(p is TabItem))
                p = System.Windows.Media.VisualTreeHelper.GetParent(p);
            sources.Remove((p as TabItem).Content as SourceTab);
            tc.Items.Remove(p);
        }

       

        private void textBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var tbk = sender as TextBlock;
                var g = tbk.Parent as Grid;
                var tb = g.FindName("textBox") as TextBox;

                tb.Visibility = System.Windows.Visibility.Visible;
                Dispatcher.BeginInvoke(new Action(() => { tb.Focus(); }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                string oldText = tb.Text;

                if (tb.Tag == null)
                {
                    DependencyObject p = tbk;
                    while (!(p is TabItem))
                        p = System.Windows.Media.VisualTreeHelper.GetParent(p);
                    var ti = p as TabItem;
                    tb.KeyDown += (o, ee) =>
                    {
                        if (ee.Key == Key.Escape) { tb.Text = oldText; tb.Visibility = System.Windows.Visibility.Collapsed; }
                        else if (ee.Key == Key.Enter) { oldText = tb.Text; tb.Visibility = System.Windows.Visibility.Collapsed; }
                    };
                    tb.LostFocus += (o, ee) => { oldText = tb.Text; tb.Visibility = System.Windows.Visibility.Collapsed; };
                    tb.TextChanged += (o, ee) =>
                    {
                        ti.Header = tb.Text;
                    };
                }
                tb.Tag = true;
            }
        }

      

        

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Current_Environment != null && MessageBox.Show("Save Current environment?", "Save Environment", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SaveEnvironment(Current_Environment);
            }
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Profiler Environment|*.cpe";
            if (ofd.ShowDialog() == true)
            {
                RestoreEnvironment(ofd.FileName);
                Current_Environment = ofd.FileName;
                Title = new FileInfo(ofd.FileName).Name;
            }
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            if (Current_Environment != null)
            {
                SaveEnvironment(Current_Environment);
                return;
            }
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.AddExtension = true;
            sfd.Filter = "Profiler Environment|*.cpe";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!sfd.FileName.EndsWith(".cpe"))
                    sfd.FileName += ".cpe";
                SaveEnvironment(sfd.FileName);
                Current_Environment = sfd.FileName;
                Title = new FileInfo(sfd.FileName).Name;
            }
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddSourceTab();
        }

        private void NewEnv_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void bI1_Click(object sender, RoutedEventArgs e)
        {
            new SnippetViewer() { Text = ((sender as Button).Tag as string) }.ShowDialog();
        }

        private void Analyze_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            bAnalyze_Click(null, null);
        }

        


    }
}
