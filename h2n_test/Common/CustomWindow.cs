using System.Windows;

namespace h2n_test.Common
{
    public class CustomWindow : Window
    {
        public CustomWindow() : base()
        {
        }

        public void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        public void MinButtonClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        public void MaxButtonClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) SystemCommands.RestoreWindow(this);
            else SystemCommands.MaximizeWindow(this);
        }
    }
}
