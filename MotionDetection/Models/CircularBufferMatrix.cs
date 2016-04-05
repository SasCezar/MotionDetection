using System;
using System.Diagnostics;

namespace MotionDetection.Models
{
	public class CircularBufferMatrix<T>
	{
	    private static CircularBufferMatrix<T> _instance; 

		private T[][][] _circularBuffer;

        private CircularBufferMatrix() { } 

		public CircularBufferMatrix(int sensorType, int sensorNumber, int time)
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
		    _instance = this;
		}
		/* TODO Implement Read & Write indexes
		public int ReadIndex { get; set; }
		public int WriteIndex { get; set; }
		*/
		public T this[int i, int j, int x]
		{
			get
			{
				var value = _circularBuffer[i][j][x%Time];
				return value;
			}
			set
			{
				_circularBuffer[i][j][x%Time] = value;
			}
		}

		public T[] GetSubArray(int sensorType, int sensorNumber)
		{
			return _circularBuffer[sensorType][sensorNumber];
		}

		#region Getters

	    public static CircularBufferMatrix<T> Instance
	    {
	        get
	        {
	            if (_instance == null)
	            {
	                throw new Exception("Buffer not created yet!");
	            }
	            return _instance;
	        }
	    } 
		public int SensorNumber { get; }

		public int SensorType { get; }

		public int Time { get; }

		#endregion
	}
}