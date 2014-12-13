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

namespace Code_Profiler
{
    /// <summary>
    /// Interaction logic for ResultRow.xaml
    /// </summary>
    public partial class ResultRow : UserControl
    {

        public ResultRow()
        {
            InitializeComponent();
            
        }
        
        public struct Program
        {
            public long display { get; set; }
            public StringBuilder snippet { get; set; }
        }
        Program _Test;
        public Program Test
        {
            get { return _Test; }
            set
            {
                _Test = value;
                Label t = lTc;
                t.Content = value.display + "";
                
            }
        }
        private List<Program> Sources = new List<Program>();
        void ShowSnippet(StringBuilder s)
        {
            if (s == null) return;
            SnippetViewer sv = new SnippetViewer() { Text = s.ToString() };
            FrameworkElement c = this;
            while (!(c is MainWindow)) c =(FrameworkElement) c.Parent;
            sv.Show();
            sv.Owner = (Window)c;
            
        }
        string AsTime(long ms)
        {
            if (ms < 1000) return ms + "ms";
            else return (ms / 1000.0) + "s";
        }
        public void AddProgram(Program p)
        {
            TextBlock t = new TextBlock();
            t.TextAlignment = TextAlignment.Center;
            t.Text = AsTime(p.display);
            t.MouseDown += (o, e) => { ShowSnippet(p.snippet); };
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(t, grid.Children.Count);
            grid.Children.Add(t);
        }

        private void lTc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowSnippet(Test.snippet);
        }
        
    }
}
