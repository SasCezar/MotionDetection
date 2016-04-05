using System;

namespace MotionDetection.Models
{
	internal class DataManipulation
	{
		// TODO Rewrite Modulo method to return a double[] and accept vectors
		// TODO Same for all methods
		public static double[]   Modulo(double[] sensorValues)
		{
			var B = new double[A.SensorType][];
			for (var i = 0; i < A.SensorType; i++)
			{
				B[i] = new double[A.Time];
				for (var j = 0; j < A.Time; j++)
				{
					B[i][j] = Math.Sqrt(Math.Pow(A[0, i, j], 2) + Math.Pow(A[1, i, j], 2) + Math.Pow(A[2, i, j], 2));
				}
			}
			return B;
		}

		public static double[][][] Smoothing(CircularBufferMatrix<double> A)
		{
			var B = new double[A.SensorNumber][][]; //, A.SensorType, A.Time];
			var s = 5; // smoothing size 2*s+1
			for (var i = 0; i < A.SensorNumber; i++)
			{
				B[i] = new double[A.SensorType][];
				for (var j = 0; j < A.SensorType; j++)
				{
					B[i][j] = new double[A.Time];
					for (var k = 0; k < A.Time; k++)
					{
						if (k > 10 && k + 10 < A.Time)
						{
							// o facciamo un bel for? k-10..k+10
							B[i][j][k] = 0;
							for (var tmp = -s; tmp < s; tmp++)
							{
								B[i][j][k] += A[i, j, k + tmp];
							}
							B[i][j][k] = B[i][j][k]/(s*2 + 1);
						}
						else
						{
							if (k < s)
							{
								for (var tmp = 0; tmp < k + s; tmp++)
								{
									B[i][j][k] += A[i, j, tmp];
								}
								B[i][j][k] = B[i][j][k]/(k + s);
							}
							else
							{
								for (var tmp = k - s; tmp < A.Time; tmp++)
								{
									B[i][j][k] += A[i, j, tmp];
								}
								B[i][j][k] = B[i][j][k]/(A.Time - k + s);
							}
						}
					}
				}
			}
			return B;
		}

		public static double[][] DifferenceQuotient(CircularBufferMatrix<double> A, int Sensor)
		{
			// A = sampwin[,] o 
			var B = new double[A.SensorType][]; //, A.Time];
			for (var i = 0; i < A.SensorType; i++)
			{
				B[i] = new double[A.Time];
				for (var j = 0; j < A.Time - 1; j++)
				{
					B[i][j] = (A[Sensor, i, j + 1] - A[Sensor, i, j])/A[Sensor, i, j];
				}
			}
			return B;
		}

		public static double Average(CircularBufferMatrix<double> A, int Sensor, int SensorNo)
		{
			double B = 0;
			for (var i = 0; i < A.Time; i++)
			{
				B += A[Sensor, SensorNo, i];
			}
			B = B/A.Time;
			return B;
		}

		public static double StdDev(CircularBufferMatrix<double> A, int Sensor, int SensorNo)
		{
			double B = 0;
			var Avg = Average(A, Sensor, SensorNo);
			for (var i = 0; i < A.Time; i++)
			{
				B += Math.Pow(A[Sensor, SensorNo, i] - Avg, 2);
			}
			B = B/A.Time;
			B = Math.Sqrt(B);
			return B;
		}

		public static double[][][] Eulero(CircularBufferMatrix<double> A)
		{
			/*
				0 = Roll (i.e. φ),
				1 = Pitch (i.e. θ),
				2 = Yaw (i.e. ψ).
			*/
			var B = new double[3][][]; //,A.SensorType,A.Time];
			B[0] = B[1] = B[2] = new double[A.SensorType][];

			for (var j = 0; j < A.SensorType; j++)
			{
				B[0][j] = B[1][j] = B[2][j] = new double[A.Time];
				for (var k = 0; k < A.Time; k++)
				{
					B[0][j][k] =
						Math.Atan((2*A[11, j, k]*A[12, j, k] + 2*A[9, j, k]*A[10, j, k])/
						          (2*A[9, j, k]*A[9, j, k] + 2*A[12, j, k]*A[12, j, k] - 1));
					B[1][j][k] = -Math.Asin(2*A[10, j, k]*A[12, j, k] - 2*A[9, j, k]*A[11, j, k]);
					B[2][j][k] =
						Math.Atan((2*A[10, j, k]*A[11, j, k] + 2*A[9, j, k]*A[12, j, k])/
						          (2*A[9, j, k]*A[9, j, k] + 2*A[10, j, k]*A[10, j, k] - 1));
				}
			}
			return B;
		}
	}
}