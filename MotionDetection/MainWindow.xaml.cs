﻿using System;
using System.Windows;
using LiveCharts;

namespace MotionDetection
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			DataReceiver.Start();
		}
	}
}