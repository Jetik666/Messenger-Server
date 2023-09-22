using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Core;
using Server.Control.Animations;
using Server.Control.View;

namespace Server
{
    public partial class MainWindow : Window
    {
        private readonly NetworkServerHost _host;
        private readonly ViewHandler _viewHandler;

        public MainWindow()
        {
            // Turn off hardware acceleration
            // Hide geforce experience overlay
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;
            _viewHandler = new();

            if (Debugger.IsAttached)
            {
                Debug.WriteLine($"Debugger is attached: {Debugger.IsAttached}");
            }

            InitializeComponent(); 
        }

        private void MinimizeProgram(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
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
                default:
                    if (Debugger.IsAttached)
                    {
                        Debug.WriteLine("Unknown action.");
                    }
                    break;
            }
        }
        private void CloseProgram(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowMouseDown(object sender, RoutedEventArgs e) => CloseAllPopups();
        private void WindowDeactivated(object sender, EventArgs e) => CloseAllPopups();
        private void WindowLocation(object sender, EventArgs e) => CloseAllPopups();

        private void CloseAllPopups()
        {
            _viewHandler.ServerView.PopupHandler.DeactivatePopups();
        }

        private void ServerView(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine($"Test: {_viewHandler.ServerView.Name} view.");
            }
            ViewFrame.Navigate(_viewHandler.ServerView);
        }
        private void DatabaseView(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine("Test: Database view.");
            }
            ViewFrame.Navigate(_viewHandler.DatabaseView);
        }
        private void TerminalView(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine($"Test: Terminal view.");
            }
            ViewFrame.Navigate(_viewHandler.TerminalView);
        }
        private void SettingsView(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine("Test: Settings view.");
            }
            ViewFrame.Navigate(_viewHandler.SettingsView);
        }

        private void ShowDesc(object sender, RoutedEventArgs e)
        {
            CloseAllPopups();
            if (sender is Button button)
            {
                if (Math.Abs(MenuBar.Width - 128) < 0.001)
                {
                    AnimationHandler.DoubleAnimation(MenuBar, WidthProperty, 128, 32, 200);
                    button.Content = "|->";
                    if (Debugger.IsAttached)
                    {
                        Debug.WriteLine($"Hide {Math.Abs(MenuBar.Width - 128)}");
                    }
                }
                else
                {
                    AnimationHandler.DoubleAnimation(MenuBar, WidthProperty, 32, 128, 200);
                    button.Content = "<-|";
                    if (Debugger.IsAttached)
                    {
                        Debug.WriteLine($"All {Math.Abs(MenuBar.Width - 128)}");
                    }
                }
            }
            else
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("ShowDesc function was called by non-Button sender.");
                }
            }
        }
    }
}
