using System;
using System.Windows;
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
		public LineSeries DeadReckoningLineSeries;

		public ViewModelWindow()
		{
		}

		public ViewModelWindow(ConnectionCommand listenCommand, ResetCommand resetCommand)
		{
			ListenCommand = listenCommand;
			ResetCommand = resetCommand;

			var numOfSeries = Enum.GetNames(typeof(SeriesType)).Length;
			SensorsModels = new PlotModel[5];
			SensorsLineSeries = new LineSeries[5][];
			DeadReckoningModels = new PlotModel();
			DeadReckoningLineSeries = new LineSeries();


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

            DeadReckoningModels = new PlotModel
            {
                Title = "Dead Reckoning"
                //PlotType = PlotType.Cartesian
            };
            DeadReckoningModels.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Title = "X (m)"
            });
            DeadReckoningModels.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Title = "Y (m)"
            });
            DeadReckoningLineSeries = new LineSeries()
            {
                Title = "Motion"
            };
            DeadReckoningModels.Series.Add(DeadReckoningLineSeries);
        }

		//private void OnMovementReceived(object sender, MotionEventArgs eventargs)
		//{

		//}

		public PlotModel[] SensorsModels { get; set; }

		public ConnectionCommand ListenCommand { get; set; }

		public ResetCommand ResetCommand { get; set; }

		public PlotModel DeadReckoningModels { get; set; }
		

		public void OnDataProcessed(object sender, SingleDataEventArgs singleArgs)
		{
			var start = singleArgs.Time < 25 ? 0 : singleArgs.SensorOne.Length/2;
			for (var i = start; i < singleArgs.SensorOne.Length; i++)
			{
				var value = singleArgs.SensorOne[i];
				SensorsLineSeries[singleArgs.UnityNumber][singleArgs.SeriesType].Points.Add(
					new DataPoint(singleArgs.Time + i, value));
			}
			SensorsModels[singleArgs.UnityNumber].InvalidatePlot(true);
		}

		public void OnDeadReckoningReceived(object sender, MultipleDataEventArgs multipleEvent)
		{
			var x = multipleEvent.SensorOne;
			var y = multipleEvent.SensorTwo;

			for (int i = 0; i < x.Length; i++)
			{
				DeadReckoningLineSeries.Points.Add(new DataPoint(x[i], y[i]));
			}
            DeadReckoningModels.InvalidatePlot(true);
		}

		public void ClearPlot()
		{
			var numOfSeries = Enum.GetNames(typeof(SeriesType)).Length;
			for (var i = 0; i < Parameters.NumUnity; i++)
			{
				SensorsModels[i].Series.Clear();
                DeadReckoningModels.Series.Clear();
				for (var j = 0; j < numOfSeries; j++)
				{
					var title = ((SeriesType) j).ToString();
					SensorsLineSeries[i][j] = new LineSeries
					{
						Title = title
					};

					SensorsModels[i].Series.Add(SensorsLineSeries[i][j]);

                }
				SensorsModels[i].InvalidatePlot(true);
           
            }
            DeadReckoningLineSeries = new LineSeries
            {
                Title = "Motion"
            };
            DeadReckoningModels.Series.Add(DeadReckoningLineSeries);
            DeadReckoningModels.InvalidatePlot(true);
        }
	}
}