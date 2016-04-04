using System.Collections.Generic;
using MotionDetection.Commands;
using MotionDetection.Models;
using OxyPlot;

namespace MotionDetection.ViewModels
{
	public class ViewModelWindow
	{
		public DataReceiver Receiver;

		public ViewModelWindow()
		{
			Receiver = new DataReceiver();
			Receiver.NewDataReceived += OnDataReceived;

			Command = new ConnectionCommand(Receiver);

			Points = new List<DataPoint>();
		}

		public string Title { get; } = "Serie 1";


		public ConnectionCommand Command { get; set; }
		public IList<DataPoint> Points { get; set; }


		public void OnDataReceived(object sender, DataEventArgs sensorArgs)
		{
			var sensorData = sensorArgs.SensorData;
			Points.Add(new DataPoint(sensorData.Time, sensorData.Value));
		}
	}
}