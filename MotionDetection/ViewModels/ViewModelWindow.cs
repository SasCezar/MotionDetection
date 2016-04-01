using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
		public Func<double, string> YFormatter { get; set; }
		public Func<double, string> XFormatter { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}