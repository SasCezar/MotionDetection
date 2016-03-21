using System;
using System.Windows;
using LiveCharts;
using MotionDetection.Models;

namespace MotionDetection.Views
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			//In this case we will not only plot double values
			//to make it easier to handle "live-data" we are going to use DataViewModel class
			//we need to let LiveCharts know how to use this model
			//first we create a new configuration for DataViewModel
			SeriesConfiguration<DataViewModel> config = new SeriesConfiguration<DataViewModel>();
			//now we map X and Y
			//we will use Temperature as Y
			config.Y(model => model.Value);
			//and DateTime as X, we convert to OADate so we can plot it easly.
			config.X(model => model.Time);

			//now we create our series with this configuration
			Series = new SeriesCollection(config)   
			{
				new LineSeries {Values = new ChartValues<DataViewModel>(), PointRadius = 0}
			};

			//to display a custom label we will use a formatter,
			//formatters are just functions that take a double value as parameter
			//and return a string, in this case we will convert the OADate to DateTime
			//and then use a custom date format
			XFormatter = val => Math.Round(val) + " ms";

			//now for Y we are rounding and adding ° for degrees
			YFormatter = val => Math.Round(val) + " °";

			//Don't forget DataContext so we can bind these properties.
			DataContext = this;
		}

		public SeriesCollection Series { get; set; }
		public Func<double, string> YFormatter { get; set; }
		public Func<double, string> XFormatter { get; set; }


		private void button_Click(object sender, RoutedEventArgs e)
		{
			var dataReceiver = new DataReceiver();
			dataReceiver.NewDataReceived += OnDataReceived;
			
			//await Task.Factory.StartNew(() => dataReceiver.Start, TaskCreationOption.LongRunning);


			dataReceiver.Start();
		}

		public void OnDataReceived(object sender, DataEventArgs eventData)
		{
			Dispatcher.Invoke(() =>
			{
				listBox.Items.Add(eventData.SensorData.Value + " " + eventData.SensorData.Time + " " + eventData.SensorData.SensorType);
				foreach (var series in Series)
				{
					if (series.Values.Count > 500)
					{
						series.Values.RemoveAt(0);
					}
					series.Values.Add(eventData.SensorData);
				}
			});
		}
	}
}