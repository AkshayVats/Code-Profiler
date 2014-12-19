using System;
using System.Collections.Generic;
using System.Text;
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
using System.Linq;

namespace Code_Profiler
{
	/// <summary>
	/// Interaction logic for SourceTab.xaml
	/// </summary>
	public partial class SourceTab : UserControl
	{
		public SourceTab()
		{
			this.InitializeComponent();
            
		}

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                using (var f = System.IO.File.OpenText(ofd.FileName))
                {
                    src.Text = f.ReadToEnd();
                }
                var fi = new FileInfo(ofd.FileName);
                (this.Parent as TabItem).Header = fi.Name;
                var compiler = MainWindow.compilers.FirstOrDefault(i => i.FileExtensions.Contains(fi.Extension.Remove(0,1)));
                this.compiler.Text = compiler == null ? "TEXT" : compiler.Name;
            }
        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = ((ComboBox)sender).SelectedValue as string;
            if (name != null && name != "")
            {
                var hb = src;
                hb.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition(MainWindow.FindCompiler(name).HighlighterKey);
                //hb.Refresh();
            }
        }

        private void src_TextChanged(object sender, EventArgs e)
        {
            chk.IsChecked = true;
        }
        public string Header
        {
            get
            {
                return ((TabItem)Parent).Header as string;
            }
            set
            {
                ((TabItem)Parent).Header = value; 
            }
        }
        public string CompilerResults
        {
            get;
            set;
        }
        private void bI_Click(object sender, RoutedEventArgs e)
        {
            new SnippetViewer() { Text = CompilerResults, Title="Compiler Result" }.ShowDialog();
        }
	}
}