using System;

namespace MotionDetection.Models
{
	public class Buffer3DMatrix<T>
	{
		private int _unityNumber = -1;
		private int _sensorType = -1;
		private int _time = -1;

		public Buffer3DMatrix(int unityNumber, int sensorType, int time)
		{
			SensorType = sensorType;
			UnityNumber = unityNumber;
			Time = time;
			CircularBuffer = new T[UnityNumber][][];
			for (var i = 0; i < UnityNumber; i++)
			{
				CircularBuffer[i] = new T[SensorType][];
				for (var j = 0; j < UnityNumber; j++)
				{
					CircularBuffer[i][j] = new T[Time];
				}
			}
		}

		protected T[][][] CircularBuffer { get; set; }

		public T this[int unityNumber, int sensorType, int time]
		{
			get { return CircularBuffer[unityNumber][sensorType][time]; }
			set { CircularBuffer[unityNumber][sensorType][time] = value; }
		}

		public T[] GetSubArray(int unityNumber, int sensorType)
		{
			return CircularBuffer[unityNumber][sensorType];
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