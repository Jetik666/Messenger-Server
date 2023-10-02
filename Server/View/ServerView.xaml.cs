using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Core;
using Core.Validator;
using Server.Control.Animations;
using Server.View.Handlers;

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

            ToggleColorsHEX("StatusColor", "#4B0000");
            ToggleColorsHEX("IsMouseOverColor", "#004B00");
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
            if (sender is not Button senderButton || _host.IsOnline)
            {
                return;
            }

            try
            {
                _host.Configuration.SetParameter<IPAddress>(GetIP.Text, false);
                _host.Configuration.SetParameter<ushort>(GetPort.Text, false);
                _host.Configuration.SetParameter<AddressFamily>(AddressFamilyValue.Text, false);
                _host.Configuration.SetParameter<SocketType>(SocketTypeValue.Text, false);
                _host.Configuration.SetParameter<ProtocolType>(ProtocolTypeValue.Text, false);

                _host.Configuration.SetEndPoint();
                _host.Configuration.SetSocket();

                _host.Start();

                _host.Configuration.AbleToEdit = false;

                ToggleColorsHEX("StatusColor", "#004B00");
                ToggleColorsHEX("IsMouseOverColor", "#4B0000");
                ServerStatus.Text = "Close";
                ToggleElements(false, 1, 0.5);

                senderButton.Click -= ServerStart;
                senderButton.Click += ServerClose;
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
        private void ServerClose(object sender, RoutedEventArgs e)
        {
            if (sender is not Button senderButton || !_host.IsOnline)
            {
                return;
            }

            try
            {
                _host.Close();

                _host.Configuration.AbleToEdit = true;

                ToggleColorsHEX("StatusColor", "#4B0000");
                ToggleColorsHEX("IsMouseOverColor", "#004B00");
                ServerStatus.Text = "Start";
                ToggleElements(false, 0.5, 1);

                senderButton.Click -= ServerClose;
                senderButton.Click += ServerStart;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private static void ToggleTextBoxes(TextBox textBox, Action<string> action)
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
        private void ToggleElements(bool toggle, double fromValue, double toValue)
        {
            GetIP.IsEnabled = toggle;
            GetPort.IsEnabled = toggle;
            AddressFamilyShow.IsEnabled = toggle;
            SocketTypeShow.IsEnabled = toggle;
            ProtocolTypeShow.IsEnabled = toggle;

            AnimationHandler.DoubleAnimation(GetIP, OpacityProperty, fromValue, toValue, 250);
            AnimationHandler.DoubleAnimation(GetPort, OpacityProperty, fromValue, toValue, 250);
            AnimationHandler.DoubleAnimation(AddressFamilyShow, OpacityProperty, fromValue, toValue, 250);
            AnimationHandler.DoubleAnimation(SocketTypeShow, OpacityProperty, fromValue, toValue, 250);
            AnimationHandler.DoubleAnimation(ProtocolTypeShow, OpacityProperty, fromValue, toValue, 250);
        }
        private static void ToggleColorsHEX(string resource, string colorHEX)
        {
            Application.Current.Resources[resource] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHEX));
        }
    }
}
