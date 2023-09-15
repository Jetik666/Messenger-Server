using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Server.View;

namespace Server.Control.View
{
    internal class ViewHandler
    {
        public ServerView ServerView { get; }
        public TerminalView TerminalView { get; }
        public DatabaseView DatabaseView { get; }

        public ViewHandler()
        {
            ServerView = new ServerView();
            TerminalView = new TerminalView();
            DatabaseView = new DatabaseView();
        }
    }
}
