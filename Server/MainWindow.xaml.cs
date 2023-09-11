using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

using Core;

namespace Server
{
    public partial class MainWindow : Window
    {
        readonly NetworkServerHost host;

        public MainWindow()
        {
            InitializeComponent();
            host = new NetworkServerHost();
            _ = host.Start();
        }

        private void MinimizeProgram(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeProgram(object sender, EventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
                return;
            }
            WindowState = WindowState.Normal;
        }

        private void CloseProgram(object sender, EventArgs e)
        {
            _ = host.Close();
            Close();
        }
    }
}
