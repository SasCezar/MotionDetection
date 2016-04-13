using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace MotionDetection.Models
{
	public class CircularBuffer3DMatrix<T> : Buffer3DMatrix<T>
	{
		private readonly int _offset;
		public int GlobalTime { get; set; }

		public CircularBuffer3DMatrix(int sensorType, int sensorNumber, int time) : base(sensorType, sensorNumber,time)
		{
			_offset = Time/3;
			GlobalTime = 0;
		}


		public new T this[int sensorType, int sensorNumber, int time]
		{
			get
			{
                return CircularBuffer[sensorType][sensorNumber][time%Time];
			}
		    set
		    {
                CircularBuffer[sensorType][sensorNumber][time%Time] = value;
		    }
		}

	}
}