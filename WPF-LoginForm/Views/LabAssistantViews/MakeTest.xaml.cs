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
using System.Windows.Shapes;

namespace WPF_LoginForm.Views.LabAssistantViews
{
    /// <summary>
    /// Logika interakcji dla klasy MakeTest.xaml
    /// </summary>
    public partial class MakeTest : Window
    {
        public MakeTest()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cancelTestBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fininshTestBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
