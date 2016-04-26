﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace MotionDetection.Models
{
	public class CircularBuffer3DMatrix<T> : Buffer3DMatrix<T>
	{
		public int GlobalTime { get; set; }

		public CircularBuffer3DMatrix(int unityNumber, int sensorType, int time) : base(unityNumber,sensorType, time)
		{
			GlobalTime = 0;
		}


		public new T this[int unityNumber, int sensorType, int time]
		{
			get
			{
                return CircularBuffer[unityNumber][sensorType][time%Time];
			}
		    set
		    {
                CircularBuffer[unityNumber][sensorType][time%Time] = value;
		    }
		}
	}
}