using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionDetection.Models
{
	public class Buffer3DMatrix<T>
	{
		private T[][][] _circularBuffer;

		public T[][][] CircularBuffer
		{
			get { return _circularBuffer; }
		}

		private Buffer3DMatrix() { }

		public Buffer3DMatrix(int sensorType, int sensorNumber, int time)
		{
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

		public T this[int sensorType, int sensorNumber, int time]
		{
			get { return _circularBuffer[sensorType][sensorNumber][time]; }
			set { _circularBuffer[sensorType][sensorNumber][time] = value; }
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
