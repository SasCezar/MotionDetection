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

		public ViewModelWindow(ConnectionCommand command)
		{
			Command = command;

			var numOfSeries = Enum.GetNames(typeof (SeriesType)).Length;
			SensorsModels = new PlotModel[5];
			SensorsLineSeries = new LineSeries[5][];

			for (var i = 0; i < Parameters.NumUnity; i++)
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

	    //private void OnMovementReceived(object sender, MotionEventArgs eventargs)
	    //{
	        
	    //}

	    public PlotModel[] SensorsModels { get; set; }

		public ConnectionCommand Command { get; set; }


		public void OnDataProcessed(object sender, SingleDataEventArgs singleArgs)
		{
			var start = singleArgs.Time <= 50 ? 0 : singleArgs.SensorOne.Length/2;
			for (var i = start; i < singleArgs.SensorOne.Length; i++)
			{
				var value = singleArgs.SensorOne[i];
				SensorsLineSeries[singleArgs.UnityNumber][singleArgs.SeriesType].Points.Add(
					new DataPoint(singleArgs.Time + i, value));
				++i;
			}
			SensorsModels[singleArgs.UnityNumber].InvalidatePlot(true);
		}
	}
}