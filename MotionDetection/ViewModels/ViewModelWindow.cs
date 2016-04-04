using System.Collections.Generic;
using System.Data;
using MotionDetection.Commands;
using MotionDetection.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MotionDetection.ViewModels
{
	public class ViewModelWindow
	{
	    private int Counter = 0;

		public DataReceiver Receiver;

	    public PlotModel MyModel {get; set; }

	    private LineSeries Series;


        public ViewModelWindow()
		{
			Receiver = new DataReceiver();
			Receiver.NewDataReceived += OnDataReceived;

			Command = new ConnectionCommand(Receiver);

            MyModel = new PlotModel()
            {
                Title = "Example"
            };

			Points = new List<DataPoint>();

            MyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0

            });
            
            MyModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left

            });

            Series = new LineSeries()
            {
                Title = "Accelerometro",
            };
            MyModel.Series.Add(Series);
        }

		public string Title { get; } = "Serie 1";


		public ConnectionCommand Command { get; set; }
		public IList<DataPoint> Points { get; set; }


		public void OnDataReceived(object sender, DataEventArgs sensorArgs)
		{
			var sensorData = sensorArgs.SensorData;
            //Points.Add(new DataPoint(sensorData.Time, sensorData.Value));
            Series.Points.Add(new DataPoint(sensorData.Time, sensorData.Value));
		    ++Counter;
		    if (Counter%30 == 0)
		    {
		        MyModel.InvalidatePlot(true);

		    }
		}
	}
}