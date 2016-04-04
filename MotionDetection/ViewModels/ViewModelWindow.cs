using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MotionDetection.Annotations;
using MotionDetection.Commands;
using MotionDetection.Models;
using OxyPlot;
using OxyPlot.Axes;

namespace MotionDetection.ViewModels
{
	public class ViewModelWindow : INotifyPropertyChanged
	{
		public DataReceiver Receiver;
	
		private string _title = "Serie 1";
		public string Title => _title;


		public PlotModel MyModel;

		public ViewModelWindow()
		{
			Receiver = new DataReceiver();
			Receiver.NewDataReceived += OnDataReceived;
			Command = new ConnectionCommand(Receiver);


			Points = new List<DataPoint>();
		}


		public ConnectionCommand Command { get; set; }
		public IList<DataPoint> Points { get; set; }


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void OnDataReceived(object sender, DataEventArgs sensorArgs)
		{
			var sensorData = sensorArgs.SensorData;
			Points.Add(new DataPoint(sensorData.Time, sensorData.Value));
			//OnPropertyChanged(nameof(Points));
		}
	}
}