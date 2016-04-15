using System;
using MotionDetection.Commands;
using MotionDetection.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MotionDetection.ViewModels
{
	public class ViewModelWindow
	{
		public LineSeries[][] SensorsLineSeries;

		public ViewModelWindow()
		{
		}

		public ViewModelWindow(ConnectionCommand command, DataManipulation dataManipulator)
		{
			Command = command;
			DataManipulator = dataManipulator;
			DataManipulator.NewDataReceived += OnDataReceived;

			var numOfSeries = Enum.GetNames(typeof (SeriesType)).Length;
			SensorsModels = new PlotModel[5];
			SensorsLineSeries = new LineSeries[5][];

			for (var i = 0; i < 5; i++)
			{
				SensorsModels[i] = new PlotModel
				{
					Title = $"Sensor {i + 1}"
				};
				SensorsModels[i].Axes.Add(new LinearAxis
				{
					Position = AxisPosition.Bottom,
					Minimum = 0
				});
				SensorsModels[i].Axes.Add(new LinearAxis
				{
					Position = AxisPosition.Left
				});

				SensorsLineSeries[i] = new LineSeries[numOfSeries];
				for (var j = 0; j < numOfSeries; j++)
				{
					var title = ((SeriesType) j).ToString();
					SensorsLineSeries[i][j] = new LineSeries
					{
						Title = title
					};
					SensorsModels[i].Series.Add(SensorsLineSeries[i][j]);
				}
			}
		}

		public PlotModel[] SensorsModels { get; set; }

		public DataManipulation DataManipulator { get; set; }

		public ConnectionCommand Command { get; set; }


		public void OnDataReceived(object sender, DataEventArgs sensorArgs)
		{
			var start = sensorArgs.Time <= 50 ? 0 : sensorArgs.SensorData.Length/2;
			for (var i = start; i < sensorArgs.SensorData.Length; i++)
			{
				var value = sensorArgs.SensorData[i];
				SensorsLineSeries[sensorArgs.SensorNumber][sensorArgs.SeriesType].Points.Add(
					new DataPoint(sensorArgs.Time - sensorArgs.SensorData.Length + i, value));
				++i;
			}
			SensorsModels[sensorArgs.SensorNumber].InvalidatePlot(true);
		}
	}
}