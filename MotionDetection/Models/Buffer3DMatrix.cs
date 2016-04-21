using System;

namespace MotionDetection.Models
{
	public class Buffer3DMatrix<T>
	{
		private int _unityNumber = -1;
		private int _sensorType = -1;
		private int _time = -1;

		private Buffer3DMatrix()
		{
		}

		public Buffer3DMatrix(int unityNumber, int sensorType, int time)
		{
			SensorType = sensorType;
			UnityNumber = unityNumber;
			Time = time;
			CircularBuffer = new T[SensorType][][];
			for (var i = 0; i < SensorType; i++)
			{
				CircularBuffer[i] = new T[UnityNumber][];
				for (var j = 0; j < UnityNumber; j++)
				{
					CircularBuffer[i][j] = new T[Time];
				}
			}
		}

		protected T[][][] CircularBuffer { get; set; }

		public T this[int sensorType, int sensorNumber, int time]
		{
			get { return CircularBuffer[sensorType][sensorNumber][time]; }
			set { CircularBuffer[sensorType][sensorNumber][time] = value; }
		}

		public T[] GetSubArray(int sensorType, int sensorNumber)
		{
			return CircularBuffer[sensorType][sensorNumber];
		}

		#region Getters

		public int UnityNumber
		{
			get { return _unityNumber; }
			set
			{
				if (UnityNumber != -1)
				{
					throw new Exception("Cannot change buffer size");
				}
				_unityNumber = value;
			}
		}

		public int SensorType
		{
			get { return _sensorType; }
			set
			{
				if (_sensorType != -1)
				{
					throw new Exception("Cannot change buffer size");
				}
				_sensorType = value;
			}
		}

		public int Time
		{
			get { return _time; }
			set
			{
				if (_time != -1)
				{
					throw new Exception("Cannot change buffer size");
				}
				_time = value;
			}
		}

		#endregion
	}
}