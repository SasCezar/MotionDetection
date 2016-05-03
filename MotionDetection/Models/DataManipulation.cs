using System;
using System.Linq;

namespace MotionDetection.Models
{
	public struct EulerAngles
	{
		public double Roll { get; set; }
		public double Pitch { get; set; }
		public double Yaw { get; set; }
	}

	public class DataManipulation
	{
		public static double[] Modulo(double[] x, double[] y, double[] z)
		{
			var result = new double[x.Length];

			for (var i = 0; i < result.Length; ++i)
			{
				result[i] = Math.Sqrt(Math.Pow(x[i], 2) + Math.Pow(y[i], 2) + Math.Pow(z[i], 2));
			}

			return result;
		}

		public static double[] StandardDeviation(double[] x, int windowSize)
		{
			var result = new double[x.Length];
			var rawSquare = x.Select(value => Math.Pow(value, 2)).ToArray();
			var meanSquare = Mean(x, windowSize).Select(value => Math.Pow(value, 2)).ToArray();
			for (var i = 0; i < result.Length; ++i)
			{
				var start = Utils.FirstIndex(i, windowSize);
				var stop = Utils.LastIndex(i, windowSize, result.Length);
				var width = stop - start + 1;
				var segmentSum = new ArraySegment<double>(rawSquare, start, width).Sum();
				var value = segmentSum/width - meanSquare[i];
				result[i] = Math.Sqrt(value);
			}
			return result;
		}

		public static double[] Mean(double[] x, int windowSize)
		{
			var result = new double[x.Length];
			for (var i = 0; i < x.Length; i++)
			{
				var sum = 0.0;
				var start = Utils.FirstIndex(i, windowSize);
				var stop = Utils.LastIndex(i, windowSize, x.Length);

				for (var h = start; h <= stop; ++h)
				{
					sum = sum + x[h];
				}
				result[i] = sum/(stop - start + 1);
			}

			return result;
		}
	}
}