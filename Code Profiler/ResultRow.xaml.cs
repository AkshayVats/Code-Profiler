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
        public Program Test { get; set; }
        public Program Program1 { get; set; }
        public Program Program2 { get; set; }
        public Program Performance { get; set; }
        void ShowSnippet(StringBuilder s)
        {
            if (s == null) return;
            SnippetViewer sv = new SnippetViewer() { Text = s.ToString() };
            sv.Show();
        }
        string AsTime(long ms)
        {
            if (ms < 1000) return ms + "ms";
            else return (ms / 1000.0) + "s";
        }
        public void Refresh()
        {
            lTc.Content = Test.display;
            lTc.MouseDown += (o, e) => { ShowSnippet(Test.snippet); };

            lPrg1.Content = AsTime(Program1.display);
            lPrg1.MouseDown += (o, e) => { ShowSnippet(Program1.snippet); };

            lPrg2.Content = AsTime(Program2.display);
            lPrg2.MouseDown += (o, e) => { ShowSnippet(Program2.snippet); };

            lProg.Content = Performance.display/10.0+"%";
            lProg.MouseDown += (o, e) => { ShowSnippet(Performance.snippet); };
        }
    }
}
