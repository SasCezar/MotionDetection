using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using LiveCharts;
using MotionDetection.Annotations;
using MotionDetection.Commands;
using MotionDetection.Models;

namespace MotionDetection.ViewModels
{
	public class ViewModelWindow : INotifyPropertyChanged
	{
		public DataReceiver Receiver;

		public ViewModelWindow()
		{
			Receiver = new DataReceiver();
			Receiver.NewDataReceived += OnDataReceived;
			Command = new ConnectionCommand(Receiver);
			var config = new SeriesConfiguration<DataViewModel>();

			config.Y(model => model.Value);
			config.X(model => model.Time);


			Series = new SeriesCollection(config)
			{
				new LineSeries {Values = new ChartValues<DataViewModel>(), PointRadius = 0}
			};


			XFormatter = val => Math.Round(val) + " s";
			YFormatter = val => Math.Round(val) + " °";
		}

		public ConnectionCommand Command { get; set; }
		public SeriesCollection Series { get; set; }

		public DataViewModel SensorData
		{
			set
			{
	
				Series[0].Values.Add(value);
				OnPropertyChanged(nameof(Series));
			}
		}
		public Func<double, string> YFormatter { get; set; }
		public Func<double, string> XFormatter { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void OnDataReceived(object sender, DataEventArgs sensorArgs)
		{
			SensorData = sensorArgs.SensorData;
		} 
	}
}