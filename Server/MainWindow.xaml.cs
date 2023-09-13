using System;
using System.Windows;
using Core;

namespace Server
{
    public partial class MainWindow : Window
    {
        private readonly NetworkServerHost _host;


        public MainWindow()
        {
            InitializeComponent();

            _host = new NetworkServerHost();
            //_ = host.Start();
        }

        private void MinimizeProgram(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeProgram(object sender, EventArgs e)
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

        private void CloseProgram(object sender, EventArgs e)
        {
            _ = _host.Close();
            Close();
        }
    }
}
