using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Code_Profiler
{
    public class CustomTabHeader:TabItem
    {
        Storyboard md;
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            md = this.Template.FindName("OnMouseDown1", this) as Storyboard;
        }
    }
}
