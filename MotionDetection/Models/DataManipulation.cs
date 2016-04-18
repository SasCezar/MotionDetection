using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotionDetection.Models
{
	public delegate void DataReceivedHandler(object sender, DataEventArgs eventArgs);

	public struct EulerAngles
	{
		public double Roll { get; set; }
		public double Pitch { get; set; }
		public double Yaw { get; set; }
	}

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
		public event DataReceivedHandler NewDataReceived;

		public void Smoothing(CircularBuffer3DMatrix<double> circularBuffer, int windowSize)
		{
			for (var i = 0; i < _buffer.SensorType; i++)
			{
				for (var j = 0; j < _buffer.SensorNumber; j++)
				{
					for (var k = 0; k < _buffer.Time; k++)
					{
						var sum = 0.0;
						var start = FirstIndex(GlobalTime - _buffer.Time + k, windowSize);
						var stop = LastIndex(GlobalTime - _buffer.Time + k, windowSize, GlobalTime);
						for (var h = start; h <= stop; ++h)
						{
							sum = sum + circularBuffer[i, j, h];
						}
						_buffer[i, j, k] = sum/(stop - start + 1);
					}
				}
			}

			ProcessData();
		}

		public async void ProcessData()
		{
			for (int i = 0; i < 5; i++)
			{
				var tasks = createTask(i);

				foreach (var task in tasks)
				{
					task.RunSynchronously();
				}

				var allResults = await Task.WhenAll(tasks);

				var taskIndex = 0;			
				foreach (var result in allResults)
				{
	
					var dataArgsAccelerometers = new DataEventArgs
					{
						SensorNumber = i,
						SeriesType = taskIndex,
						SensorData = result,
						Time = GlobalTime
					};
					NewDataReceived?.Invoke(this, dataArgsAccelerometers);
					taskIndex++;
				}
			}
		}

		public IEnumerable<Task<double[]>> createTask(int sensorNumber)
		{
			var tasks = new List<Task<double[]>>();
			var modAccTask = new Task<double[]>(() => Modulo(_buffer.GetSubArray((int)SensorType.Accelerometer1, sensorNumber),
						_buffer.GetSubArray((int)SensorType.Accelerometer2, sensorNumber), _buffer.GetSubArray((int)SensorType.Accelerometer3, sensorNumber)));
			var modGyrTask = new Task<double[]>(() => Modulo(_buffer.GetSubArray((int)SensorType.Gyroscope1, sensorNumber),
						_buffer.GetSubArray((int)SensorType.Gyroscope2, sensorNumber), _buffer.GetSubArray((int)SensorType.Gyroscope3, sensorNumber)));

			tasks.Add(modAccTask);
			tasks.Add(modGyrTask);

			return tasks;
		} 

		public static double[] Modulo(double[] x, double[] y, double[] z)
		{
			var result = new double[x.Length];

			for (var i = 0; i < result.Length; ++i)
			{
				result[i] = Math.Sqrt(Math.Pow(x[i], 2) + Math.Pow(y[i], 2) + Math.Pow(z[i], 2));
			}

			return result;
		}


		public static int FirstIndex(int i, int width)
		{
			return i - width/2 > 0 ? i - width/2 : 0;
		}

		public static int LastIndex(int i, int width, int size)
		{
			if (i + width/2 < size)
			{
				return i + width/2;
			}
			return size - 1;
		}
	}
}