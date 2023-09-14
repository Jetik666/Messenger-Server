using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Core;

namespace Server
{
    public partial class MainWindow : Window
    {
        private readonly NetworkServerHost _host;

        public MainWindow()
        {
            // Turn off hardware acceleration
            // Hide geforce experience overlay
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            InitializeComponent();

            _host = new NetworkServerHost();
            //_ = host.Start();
        }

        private void MinimizeProgram(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeProgram(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    ResizeMode = ResizeMode.NoResize;
                    WindowState = WindowState.Maximized;
                    MainFrame.Margin = new Thickness(10, 5, 10, 60);
                    break;
                case WindowState.Maximized:
                    ResizeMode = ResizeMode.CanResize;
                    WindowState = WindowState.Normal;
                    MainFrame.Margin = new Thickness(5, 0, 5, 5);
                    break;
            }
        }

        private void CloseProgram(object sender, RoutedEventArgs e)
        {
            _ = _host.Close();
            Close();
        }

        private async void ShowDesc(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (Math.Abs(MenuBar.Width - 128) < 0.001)
                {
                    await AnimationHandler.DoubleAnimation(MenuBar, WidthProperty, 128, 32, 200);
                    button.Content = "|->";
                    Debug.WriteLine($"Hide {Math.Abs(MenuBar.Width - 128)}");
                }
                else
                {
                    await AnimationHandler.DoubleAnimation(MenuBar, WidthProperty, 32, 128, 200);
                    button.Content = "<-|";
                    Debug.WriteLine($"All {Math.Abs(MenuBar.Width - 128)}");
                }
            }
            else
            {
                Debug.WriteLine("ShowDesc function was called by non-Button sender.");
            }
        }
    }
}
