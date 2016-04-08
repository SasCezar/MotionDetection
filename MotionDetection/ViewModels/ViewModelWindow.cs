using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Interop;
using MotionDetection.Commands;
using MotionDetection.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MotionDetection.ViewModels
{
	public class ViewModelWindow
	{
		public LineSeries Series;

		public ViewModelWindow()
		{
			
		}

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
		    var start = (sensorArgs.Time <= 50) ? 0 : sensorArgs.SensorData.Length/2;
            for (var i = start; i < sensorArgs.SensorData.Length; i++)
			{
				var value = sensorArgs.SensorData[i];
				//Console.WriteLine($"Time {sensorArgs.Time}; PlotTime {sensorArgs.Time - sensorArgs.SensorData.Length + i};  value {value}");
				Series.Points.Add(new DataPoint(sensorArgs.Time - sensorArgs.SensorData.Length + i, value));
				++i;
			}
			MyModel.InvalidatePlot(true);
		}
	}
}