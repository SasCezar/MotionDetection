using System;
using System.Diagnostics;

namespace MotionDetection.Models
{
	public class CircularBuffer3DMatrix<T> : Buffer3DMatrix<T>
	{
		public int ReadIndex { get; set; }
		public int WriteIndex { get; set; }


		public CircularBuffer3DMatrix(int sensorType, int sensorNumber, int time) : base(sensorType, sensorNumber,time)
		{
			WriteIndex = 0;
			ReadIndex = 0;
		}


		public new T this[int sensorType, int sensorNumber, int time]
		{
			get { return CircularBuffer[sensorType][sensorNumber][time%Time]; }
			set { CircularBuffer[sensorType][sensorNumber][time%Time] = value; }
		}

	}
}