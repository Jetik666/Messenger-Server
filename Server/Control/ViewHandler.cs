using Server.View;

using Core;

namespace Server.Control.View
{
    internal class ViewHandler
    {
        public ServerView ServerView { get; }
        public TerminalView TerminalView { get; }
        public DatabaseView DatabaseView { get; }
        public SettingsView SettingsView { get; }

        public ViewHandler(Network host)
        {
            ServerView = new ServerView(host);
            TerminalView = new TerminalView();
            DatabaseView = new DatabaseView();
            SettingsView = new SettingsView();
        }
    }
}
