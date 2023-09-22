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
            InitializeComponent();

            // Turn off hardware acceleration
            // Hide geforce experience overlay
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            _host = new NetworkServerHost();
            _viewHandler = new(_host);
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

        // TODO: Add additional popups from other views
        private void CloseAllPopups()
        {
            _viewHandler.ServerView.PopupHandler.DeactivatePopups();
        }

        private void ServerView(object sender, RoutedEventArgs e) => ViewFrame.Navigate(_viewHandler.ServerView);
        private void DatabaseView(object sender, RoutedEventArgs e) => ViewFrame.Navigate(_viewHandler.DatabaseView);
        private void TerminalView(object sender, RoutedEventArgs e) => ViewFrame.Navigate(_viewHandler.TerminalView);
        private void SettingsView(object sender, RoutedEventArgs e) => ViewFrame.Navigate(_viewHandler.SettingsView);

        private void ShowDesc(object sender, RoutedEventArgs e)
        {
            CloseAllPopups();

            Debug.WriteLine(_host.IsOnline);

            if (sender is Button button)
            {
                if (Math.Abs(MenuBar.Width - 128) < 0.001)
                {
                    AnimationHandler.DoubleAnimation(MenuBar, WidthProperty, 128, 32, 200);
                    button.Content = "|->";
                }
                else
                {
                    AnimationHandler.DoubleAnimation(MenuBar, WidthProperty, 32, 128, 200);
                    button.Content = "<-|";
                }
            }
        }
    }
}
