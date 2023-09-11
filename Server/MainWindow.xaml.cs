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
            Debug.WriteLine("test");
            host = new NetworkServerHost();
            Task serverTask = host.Start();
        }
    }
}
