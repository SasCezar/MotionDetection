using System;

namespace MotionDetection.Models
{
	public delegate void PlotOrientationHandeler(object sender, SingleDataEventArgs eventArgs);

	public class OrientationRecognition
	{
		private int _time;
		private double thresHold = 0.0001;

		public event PlotOrientationHandeler OnPlotOrientation;

		public double[]  RecognizeOrientation(double[] y, double[] z)
		{
			var result = new double[y.Length];
			result[0] = Math.Atan2(y[0], z[0]);
			int sign = (int)(result[0] / Math.Abs(result[0]));
			if (sign == 0)
			{
				sign = -1;
			}
			var quadrants = new int[result.Length];
			quadrants[0] = CalculateQuadrand(result[0]);
			for (var i = 1; i < result.Length; i++)
			{
				var radians = Math.Atan2(y[i], z[i]);
				quadrants[i] = CalculateQuadrand(radians);

				if (quadrants[i] == 4 && quadrants[i - 1] == 1)
				{
					sign++;
					if (sign == 0)
					{
						sign = 1;
					}
				}
				else if(quadrants[i] == 1 && quadrants[i - 1] == 4)
				{
					sign--;
					if (sign == 0)
					{
						sign = -1;
					}
				}


				if ((sign > 0 && radians > 0) || (sign < 0 && radians < 0))
				{
					result[i] = radians;
				}
				if (sign > 0 && radians < 0)
				{
					result[i] = Math.Abs(radians);
				}
				if (sign < 0 && radians > 0)
				{
					result[i] = -radians;
				}
				//result[i] = radians;
			}
			return result;
		}

		private int CalculateQuadrand(double theta)
		{
			if (theta > 0 && theta < Math.PI/2)
			{
				return 1;
			} 
			if (theta > Math.PI / 2 && theta > 0 && theta <= Math.PI)
			{
				return 2;
			}
			if (theta > -Math.PI / 2 && theta < -Math.PI/2)
			{
				return 3;
			}
			if (theta < -Math.PI/2 && theta < 0)
			{
				return 4;
			}
			return 0;
		}


		private double[] DifferenceQuotient(double[] x)
		{
			var result = new double[x.Length];

			for (var i = 0; i < result.Length - 1; ++i)
			{
				result[i] = x[i + 1] - x[i];
			}
			return result;
		}

		public void OnDataReceived(object sender, MultipleDataEventArgs eventArgs)
		{
			_time = eventArgs.Time;
			var	result = RecognizeOrientation(eventArgs.SensorTwo, eventArgs.SensorThree);

			if (eventArgs.UnityNumber == 0)
			{
				var start = eventArgs.Time <= 50 ? 0 : result.Length / 2;
				for (int i = start; i < result.Length; i++)
				{
					//Console.WriteLine(result[i]);
				}
			}

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