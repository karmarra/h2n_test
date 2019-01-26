using System.Windows;
using System.Windows.Controls;

namespace h2n_test.Common
{
    public class CustomWindow : Window
    {

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("MinButton") is Button minButton)
                minButton.Click += MinButtonOnClick;
            if (GetTemplateChild("MaxButton") is Button maxButton)
                maxButton.Click += MaxButtonOnClick;
            if (GetTemplateChild("CloseButton") is Button closeButton)
                closeButton.Click += CloseButtonOnClick;
            base.OnApplyTemplate();
        }

        private void MinButtonOnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void MaxButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) SystemCommands.RestoreWindow(this);
            else SystemCommands.MaximizeWindow(this);
        }

        private void CloseButtonOnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }
    }
}
