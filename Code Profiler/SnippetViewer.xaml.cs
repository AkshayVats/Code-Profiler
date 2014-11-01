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
    /// Interaction logic for SnippetViewer.xaml
    /// </summary>
    public partial class SnippetViewer : Window
    {
        public new String Text
        {
            get { return snip.Text; }
            set { snip.Text = value; }
        }
        public SnippetViewer()
        {
            InitializeComponent();
        }
    }
}
