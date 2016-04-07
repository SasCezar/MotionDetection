using System;
using System.Windows;

namespace MotionDetection.Models
{
	public delegate void OnDataReceivedHandler(object sender, DataEventArgs eventArgs);

	public class DataManipulation
	{
		private Buffer3DMatrix<double> _buffer;

		public Buffer3DMatrix<double> Buffer
		{
			get { return _buffer; }
			set
			{
				if (_buffer != null)
				{
					throw new Exception("Cannot change the buffer");
				}
				_buffer = value;
			}
		}

		public int GlobalTime { get; set; }
		public event OnDataReceivedHandler NewDataReceived;

		public void Smoothing(CircularBuffer3DMatrix<double> circularBuffer, int windowSize)
		{
			for (var i = 0; i < _buffer.SensorType; i++)
			{
				for (var j = 0; j < _buffer.SensorNumber; j++)
				{
					for (var k = 0; k < _buffer.Time; k++)
					{
						//_buffer[i, j, k] = circularBuffer[i, j, k];
						var sum = 0.0;
						var start = FirstIndex(k, windowSize);
						var stop = LastIndex(k, windowSize, circularBuffer.Time);

						for (var h = start; h <= stop; ++h)
						{
							//var msg = $"Start {start}, Stop {stop}, H {h}";
							//MessageBox.Show(msg);
							sum = sum + circularBuffer[i, j, h];
						}
						_buffer[i, j, k] = sum / (stop - start + 1);
					}
				}
			}
			var dataArgs = new DataEventArgs
			{
				SensorData = _buffer.GetSubArray(0, 0),
				Time = GlobalTime
			};
			Console.WriteLine($"Global time = {GlobalTime}");
			NewDataReceived?.Invoke(this, dataArgs);
			circularBuffer.UpdateReadIndex();
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


		private static int FirstIndex(int i, int width)
		{
			return ((i - width)/2) > 0 ? ((i - width)/2) : 0;

		}

		private static int LastIndex(int i, int width, int size)
		{
			if (i + width/2 < size)
			{
				return i + width/2;
			}
			return size - 1;
		}
	}
}