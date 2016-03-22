using System.Diagnostics;

namespace MotionDetection.Models
{
	public class CircularBufferMatrix<T>
	{
		private readonly T[][][] _circularBuffer;


		public CircularBufferMatrix(int sensorType, int sensorNumber, int time)
		{
			ReadIndex = -1; // Reading is not yet allowed
			WriteIndex = 0; // you should start writing @ [0]
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
				if (x%(Time/3) == 0)
				{
					ReadIndex = (Time + x - (2*Time/3)) % Time;
					WriteIndex = (ReadIndex + (Time/3)) % Time; // should be equal to x%Time
					Debug.WriteLine("Updated ReadIndex="+ReadIndex+", WriteIndex="+WriteIndex);
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