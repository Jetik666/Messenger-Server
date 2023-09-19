﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server.View
{
    /// <summary>
    /// Interaction logic for ServerView.xaml
    /// </summary>
    public partial class ServerView : Page
    {
        public ServerView()
        {
            InitializeComponent();
        }

        private void ShowAddressFamily(object sender, RoutedEventArgs e)
        {
            AddressFamilyPopup.IsOpen = true;
        }
        
        private void SetAddressFamily(object sender, RoutedEventArgs e)
        {
            AddressFamilyPopup.IsOpen = false;
        }
    }
}
