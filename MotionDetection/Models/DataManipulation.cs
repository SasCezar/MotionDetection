using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
	}
}