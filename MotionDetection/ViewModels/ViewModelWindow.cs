using System.Collections.Generic;
using MotionDetection.Commands;
using MotionDetection.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MotionDetection.ViewModels
{
	public class ViewModelWindow
	{
		private LineSeries Series;
		private int _counter;

		public ViewModelWindow(ConnectionCommand command, DataManipulation dataManipulator)
		{
			Command = command;
			DataManipulator = dataManipulator;
			DataManipulator.NewDataReceived += OnDataReceived;
			// TODO Add multiple models (one model for each plot) with multiple series
			MyModel = new PlotModel
			{
				Title = "Example"
			};

			Points = new List<DataPoint>();

			MyModel.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Bottom,
				Minimum = 0
			});

			MyModel.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Left
			});

			Series = new LineSeries
			{
				Title = "Accelerometro"
			};
			MyModel.Series.Add(Series);
		}

		public DataManipulation DataManipulator { get; set; }

		public PlotModel MyModel { get; set; }

		public ConnectionCommand Command { get; set; }

		public IList<DataPoint> Points { get; set; }

		public void OnDataReceived(object sender, DataEventArgs sensorArgs)
		{
			//var sensorData = sensorArgs.SensorData;
			//Points.Add(new DataPoint(sensorData.Time, sensorData.Value));
			Series.Points.Add(new DataPoint(sensorArgs.SensorData, sensorArgs.Time));
			// TODO Remove and fire when new data array added
			//++_counter;
			//if (_counter%50 == 0) 
			//{
			//	MyModel.InvalidatePlot(true);
			//}
		}
	}
}