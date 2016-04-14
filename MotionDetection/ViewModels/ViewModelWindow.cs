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
		public LineSeries ModAccLineSeries;
		public LineSeries ModGyrLineSeries;

		public LineSeries[] AllLineSeries;

		public ViewModelWindow()
		{
			
		}

		public ViewModelWindow(ConnectionCommand command, DataManipulation dataManipulator)
		{
			Command = command;
			DataManipulator = dataManipulator;
			DataManipulator.NewDataReceived += OnDataReceived;
			// TODO Add multiple models (one model for each plot) with multiple series
			SensorOne = new PlotModel
			{
				Title = "Example"
			};

			PointsAcc = new List<DataPoint>();
			PointsGyr = new List<DataPoint>();

			SensorOne.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Bottom,
				Minimum = 0
			});

			SensorOne.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Left
			});

			ModAccLineSeries = new LineSeries
			{
				Title = "Modulo Accelerometri",

			};
			ModGyrLineSeries = new LineSeries
			{
				Title = "Modulo Giroscopi",
			};
			SensorOne.Series.Add(ModAccLineSeries);
			SensorOne.Series.Add(ModGyrLineSeries);
			AllLineSeries = new[] {ModAccLineSeries, ModGyrLineSeries};

		}

		public DataManipulation DataManipulator { get; set; }

		public PlotModel SensorOne { get; set; }

		public ConnectionCommand Command { get; set; }

		public IList<DataPoint> PointsAcc { get; set; }
		public IList<DataPoint> PointsGyr { get; set; }

		public void OnDataReceived(object sender, DataEventArgs sensorArgs)
		{
		    var start = (sensorArgs.Time <= 50) ? 0 : sensorArgs.SensorData.Length/2;
            for (var i = start; i < sensorArgs.SensorData.Length; i++)
			{
				var value = sensorArgs.SensorData[i];
				//AllLineSeries[(int)sensorArgs.SeriesType].Points.Add(new DataPoint(sensorArgs.Time - sensorArgs.SensorData.Length + i, value));
				if ((int) sensorArgs.SeriesType == 0)
				{
					ModAccLineSeries.Points.Add(new DataPoint(sensorArgs.Time - sensorArgs.SensorData.Length + i, value));
				}
				else
				{
					ModGyrLineSeries.Points.Add(new DataPoint(sensorArgs.Time - sensorArgs.SensorData.Length + i, value));
				}
				++i;
			}
			SensorOne.InvalidatePlot(true);
		}
	}
}