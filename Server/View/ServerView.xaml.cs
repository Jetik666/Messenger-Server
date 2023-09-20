using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using Server.View.Page_Handlers;

namespace Server.View
{
    public partial class ServerView : Page
    {
        private readonly PopupHandler _popups;

        public ServerView()
        {
            InitializeComponent();

            _popups = new();
            _popups.RegisterPopups(Popups);
        }

        private List<Popup> Popups()
        {
            List<Popup> popups = new()
            {
                AddressFamilyPopup,
                SocketTypePopup,
                ProtocolTypePopup
            };
            return popups;
        }

        private void ShowValues(object sender, RoutedEventArgs e)
        {
            if (sender is Button senderButton)
            {
                switch (senderButton.Name)
                {
                    case "AddressFamilyShow":
                        _popups.OpenedPopup = AddressFamilyPopup;
                        break;
                    case "SocketTypeShow":
                        _popups.OpenedPopup = SocketTypePopup;
                        break;
                    case "ProtocolTypeShow":
                        _popups.OpenedPopup = ProtocolTypePopup; 
                        break;
                    default:
                        if (Debugger.IsAttached)
                        {
                            Debug.WriteLine("Unknown command.");
                        }
                        _popups.ClosePopup();
                        break;
                }
            }
        }
        private void SetValue(object sender, RoutedEventArgs e)
        {
            if (sender is Button senderButton)
            {
                switch (senderButton.Tag)
                {
                    case "Address Family":
                        _popups.ClosePopup(AddressFamilyPopup);
                        AddressFamilyValue.Text = senderButton.Content.ToString();
                        break;
                    case "Socket Type":
                        _popups.ClosePopup(SocketTypePopup);
                        SocketTypeValue.Text = senderButton.Content.ToString();
                        break;
                    case "Protocol Type":
                        _popups.ClosePopup(ProtocolTypePopup);
                        ProtocolTypeValue.Text = senderButton.Content.ToString();
                        break;
                    default:
                        if (Debugger.IsAttached)
                        {
                            Debug.WriteLine("Unknown command.");
                        }
                        _popups.ClosePopup();
                        break;
                }
            }
        }
    }
}
