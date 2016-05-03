using System;

namespace MotionDetection.Models
{
	public delegate void PlotOrientationHandeler(object sender, SingleDataEventArgs eventArgs);

	public class OrientationRecognition
	{
		private int _time;

		public event PlotOrientationHandeler OnPlotOrientation;

		public double[]  RecognizeOrientation(double[] y, double[] z)
		{
			var result = new double[y.Length];
			for (var i = 0; i < result.Length; i++)
			{
				var radians = Math.Atan2(y[i],z[i]);
				result[i] = radians;
			}

			return result;
		}


		public void OnDataReceived(object sender, MultipleDataEventArgs eventArgs)
		{
			_time = eventArgs.Time;
			var	result = RecognizeOrientation(eventArgs.SensorTwo, eventArgs.SensorThree);

			OnPlotOrientation?.Invoke(this, new SingleDataEventArgs()
			{
				UnityNumber = eventArgs.UnityNumber,
				SensorOne = result,
				SeriesType = eventArgs.SeriesType,
				Time = _time,
			});
		}

	}
}