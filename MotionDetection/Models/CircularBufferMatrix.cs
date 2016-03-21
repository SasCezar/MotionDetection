﻿namespace MotionDetection.Models
{
	public class CircularBufferMatrix<T>
	{
		private readonly T[][][] _circularBuffer;


		public CircularBufferMatrix(int sensorType, int sensorNumber, int time)
		{
			ReadIndex = 0;
			WriteIndex = 0;
			SensorType = sensorType;
			SensorNumber = sensorNumber;
			Time = time;
			_circularBuffer = new T[SensorType][][];
			for (var i = 0; i < SensorType; i++)
			{
				_circularBuffer[i] = new T[SensorNumber][];
				for (var j = 0; j < SensorNumber; j++)
				{
					_circularBuffer[i][j] = new T[Time];
				}
			}
		}

		public int ReadIndex { get; set; }
		public int WriteIndex { get; set; }

		public T this[int i, int j, int x]
		{
			get
			{
				var value = _circularBuffer[i][j][(x + ReadIndex)%Time];
				return value;
			}
			set
			{
				_circularBuffer[i][j][x%Time] = value;
				if (x%(2/3*Time) == 0)
				{
					ReadIndex = (ReadIndex + 1/3*Time)%Time;
					WriteIndex = (WriteIndex + 1/3*Time)%Time;
				}
			}
		}

		public T[] GetSubArray(int sensorType, int sensorNumber)
		{
			return _circularBuffer[sensorType][sensorNumber];
		}

		#region Getters

		public int SensorNumber { get; }

		public int SensorType { get; }

		public int Time { get; }

		#endregion
	}
}