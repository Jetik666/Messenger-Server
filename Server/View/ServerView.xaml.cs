using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Core;
using Server.View.Page_Handlers;

namespace Server.View
{
    public partial class ServerView : Page
    {
        private readonly PopupHandler _popups;
        private readonly NetworkServerHost _host;

        public ServerView(NetworkServerHost host)
        {
            InitializeComponent();

            _host = host;

            _popups = new();
            _popups.RegisterPopups(Popups);

            Application.Current.Resources["StatusColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4B0000"));
            Application.Current.Resources["IsMouseOverColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#004B00"));
        }

        public PopupHandler PopupHandler
        {
            get => _popups;
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
                        _popups.ClosePopup();
                        break;
                }
            }
        }
        
        private void ServerStart(object sender, RoutedEventArgs e)
        {
            if (sender is Button senderButton && !_host.IsOnline)
            {
                try
                {
                    _host.Start();

                    Application.Current.Resources["StatusColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#004B00"));
                    Application.Current.Resources["IsMouseOverColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4B0000"));
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ServerClose(object sender, RoutedEventArgs e)
        {

            _host.Close();
        }
    }
}
