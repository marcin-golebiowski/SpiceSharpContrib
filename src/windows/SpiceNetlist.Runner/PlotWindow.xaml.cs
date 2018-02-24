﻿using SpiceNetlist.SpiceSharpConnector.Processors.Controls.Plots;
using System;
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

namespace SpiceNetlist.Runner
{
    /// <summary>
    /// Interaction logic for PlotWindow.xaml
    /// </summary>
    public partial class PlotWindow : Window
    {
        public Plot Plot { get; }

        public PlotWindow(Plot plot)
        {
            Plot = plot;
            InitializeComponent();
            DataContext = new PlotViewModel(plot, false, false);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            PlotViewModel model = new PlotViewModel(Plot, this.x.IsChecked.Value, this.y.IsChecked.Value);
            this.DataContext = model;

        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            PlotViewModel model = new PlotViewModel(Plot, this.x.IsChecked.Value, this.y.IsChecked.Value);
            this.DataContext = model;
        }
    }
}
