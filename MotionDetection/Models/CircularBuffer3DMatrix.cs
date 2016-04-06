using System;
using System.Diagnostics;

namespace MotionDetection.Models
{
	public class CircularBuffer3DMatrix<T> : Buffer3DMatrix<T>
	{
		private int ReadIndex { get; set; }
		private int _offset;


		public CircularBuffer3DMatrix(int sensorType, int sensorNumber, int time) : base(sensorType, sensorNumber,time)
		{
			ReadIndex = 0;
			_offset = Time*1/3;
		}


		public new T this[int sensorType, int sensorNumber, int time]
		{
			get { return CircularBuffer[sensorType][sensorNumber][ReadIndex + time%Time]; }
			set { CircularBuffer[sensorType][sensorNumber][time%Time] = value; }
		}

		public void UpdateReadIndex()
		{
			ReadIndex = (ReadIndex + _offset)%Time;
		}

	}
}