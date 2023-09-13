using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
                if (Math.Abs(MenuBarWidth.Width.Value - 128) < 0.001)
                {
                    await ShowAllAnimation(128, 32);
                    button.Content = "|->";
                    Debug.WriteLine($"Hide {Math.Abs(MenuBarWidth.Width.Value - 128)}");
                }
                else
                {
                    await ShowAllAnimation(32, 128);
                    button.Content = "<-|";
                    Debug.WriteLine($"All {Math.Abs(MenuBarWidth.Width.Value - 128)}");
                }
            }
            else
            {
                Debug.WriteLine("ShowDesc function was called by non-Button sender.");
            }
        }

        private async Task ShowAllAnimation(double fromWidth, double toWidth)
        {
            MenuBarWidth.Width = new GridLength(toWidth);

            DoubleAnimation animation = new DoubleAnimation
            {
                From = fromWidth,
                To = toWidth,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            // Check that the MenuBarWidth is correctly bound and the WidthProperty is the correct dependency property
            if (MenuBarWidth != null && WidthProperty != null)
            {
                MenuBarWidth.BeginAnimation(WidthProperty, animation);
                await Task.Delay(100); // Add a small delay to ensure the animation has time to start
                Debug.WriteLine("Show all animation is finished.");
            }
            else
            {
                Debug.WriteLine("MenuBarWidth or WidthProperty is null.");
            }
        }
    }
}
