using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using h2n_test.Common;

namespace h2n_test
{

    public partial class MainWindow : CustomWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Brd_OnMouseMove(object sender, MouseEventArgs e)
        {
            var btn = (Border)sender;
            var gradient = btn.Background as RadialGradientBrush;
            if (gradient == null)
                return;
            gradient.Opacity = 1;
            Point pt = Mouse.GetPosition(btn);
            gradient.GradientOrigin = new Point(pt.X / btn.ActualWidth, pt.Y / btn.ActualHeight);
            gradient.Center = gradient.GradientOrigin;
        }

        private void Brd_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var btn = (Border)sender;
            var gradient = btn.Background as RadialGradientBrush;
            if (gradient == null)
                return;
            gradient.Opacity = 0;
        }
    }
}
