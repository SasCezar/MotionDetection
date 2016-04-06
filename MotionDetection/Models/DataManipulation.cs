using System;

namespace MotionDetection.Models
{
	internal class DataManipulation
	{

		private Buffer3DMatrix<double> _buffer;

        /// <summary>
        /// Constructor to associate the static buffer to the class
        /// </summary>
        /// <param name="buffer"></param>
		public DataManipulation(Buffer3DMatrix<double> buffer)
		{
			_buffer = buffer;
		}

	    public static int FirstIndex(int i, int width)
	    {
	        if (i >= width/2)
	        {
	            return (i - width)/2;
	        }
	        else
	        {
	            return 0;
	        }
	    }

	    public static int LastIndex(int i, int width, int size)
	    {
	        if (i + width/2 < size)
	        {
	            return (i + width/2);
	        }
	        else
	        {
	            return (size - 1);
	        }
	    }

		public void Smoothing(CircularBuffer3DMatrix<double> circularBuffer, int windowSize)
		{
			for (var i = 0; i < _buffer.SensorType; i++)
			{
				for (var j = 0; j < _buffer.SensorNumber; j++)
				{
					for (var k = 0; k < _buffer.Time; k++)
					{
					    double sum = 0.0;
					    int start = FirstIndex(k, windowSize);
					    int stop = LastIndex(k, windowSize, circularBuffer.Time);

					    for (int h = start; h <= stop; ++h)
					    {
					        sum = sum + circularBuffer[i, j, h];
					    }
					    _buffer[i, j, k] = sum/(stop - start + 1);
					}
				}
			}
		}


		// TODO Rewrite Modulo method to return a double[] and accept vectors
		// TODO Same for all methods
		//public static double[]   Modulo(double[] sensorValues)
		//{
		//	var B = new double[A.SensorType][];
		//	for (var i = 0; i < A.SensorType; i++)
		//	{
		//		B[i] = new double[A.Time];
		//		for (var j = 0; j < A.Time; j++)
		//		{
		//			B[i][j] = Math.Sqrt(Math.Pow(A[0, i, j], 2) + Math.Pow(A[1, i, j], 2) + Math.Pow(A[2, i, j], 2));
		//		}
		//	}
		//	return B;
		//}

		public static double[][] DifferenceQuotient(CircularBuffer3DMatrix<double> A, int Sensor)
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

		public static double Average(CircularBuffer3DMatrix<double> A, int Sensor, int SensorNo)
		{
			double B = 0;
			for (var i = 0; i < A.Time; i++)
			{
				B += A[Sensor, SensorNo, i];
			}
			B = B/A.Time;
			return B;
		}

		public static double StdDev(CircularBuffer3DMatrix<double> A, int Sensor, int SensorNo)
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

		public static double[][][] Eulero(CircularBuffer3DMatrix<double> A)
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