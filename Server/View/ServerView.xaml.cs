using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Core;
using Core.Validator;
using Server.View.Page_Handlers;

namespace Server.View
{
    public partial class ServerView : Page
    {
        private readonly PopupHandler _popups;
        private readonly Network _host;

        public ServerView(Network host)
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

        private void IPChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                ToggleTextBoxes(textBox, NetworkValidator.IPv4);
            }
        }
        private void PortChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                ToggleTextBoxes(textBox, NetworkValidator.Port);
            }
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
                    _host.Server.ChangeIPv4(GetIP.Text);
                    _host.Server.Port(GetPort.Text);
                    _host.Server.ChangeSocketParameter(AddressFamilyValue.Text);
                    _host.Server.ChangeST(SocketTypeValue.Text);
                    _host.Server.ChangePT(ProtocolTypeValue.Text);
                    _host.Server.UpdateEndPoint();
                    _host.Server.UpdateSocket();
                    _host.Start();

                    Application.Current.Resources["StatusColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#004B00"));
                    Application.Current.Resources["IsMouseOverColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4B0000"));

                    senderButton.Click -= ServerStart;
                    senderButton.Click += ServerClose;

                    ServerStatus.Text = "Close";

                    ToggleElements(false);
                }
                catch (Exception ex) 
                {
                    StackTrace st = new(ex, true);
                    StackFrame? frame = st.GetFrame(0);
                    int line = frame.GetFileLineNumber();

                    MessageBox.Show(ex.Message + Environment.NewLine + st.ToString() + Environment.NewLine + frame.ToString() + Environment.NewLine + line,
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
        private void ServerClose(object sender, RoutedEventArgs e)
        {
            if (sender is Button senderButton && _host.IsOnline)
            {
                try
                {
                    _host.Close();

                    Application.Current.Resources["StatusColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4B0000"));
                    Application.Current.Resources["IsMouseOverColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#004B00"));

                    senderButton.Click -= ServerClose;
                    senderButton.Click += ServerStart;

                    ServerStatus.Text = "Start";

                    ToggleElements(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
        private static void ToggleTextBoxes(TextBox textBox, Action<string> action)
        {
            if (textBox.Text != null)
            {
                try
                {
                    action(textBox.Text);
                    textBox.BorderBrush = new SolidColorBrush(Colors.White);
                }
                catch
                {
                    textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                }
            }
            else
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }
        private void ToggleElements(bool toggle)
        {
            GetIP.IsEnabled = toggle;
            GetPort.IsEnabled = toggle;
            AddressFamilyShow.IsEnabled = toggle;
            SocketTypeShow.IsEnabled = toggle;
            ProtocolTypeShow.IsEnabled = toggle;
        }
    }
}
