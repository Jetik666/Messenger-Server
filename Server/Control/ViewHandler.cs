using Server.View;

namespace Server.Control.View
{
    internal class ViewHandler
    {
        public ServerView ServerView { get; }
        public TerminalView TerminalView { get; }
        public DatabaseView DatabaseView { get; }

        public SettingsView SettingsView { get; }

        public ViewHandler()
        {
            ServerView = new ServerView();
            TerminalView = new TerminalView();
            DatabaseView = new DatabaseView();
            SettingsView = new SettingsView();
        }
    }
}
