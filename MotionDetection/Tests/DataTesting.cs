using System;
using MotionDetection.Models;

namespace MotionDetection.Tests
{
	public class DataTesting
	{
		public static void OnDataToPrint(object sender, BufferEventArgs<double> eventArgs)
		{
			var start = eventArgs.Time == 0 ? 0 : 25; 
			var data = (CircularBuffer3DMatrix<double>)eventArgs.Data;
			for (var i = start; i < Parameters.StaticBufferSize; i++)
			{
				Console.WriteLine(data[0,0,eventArgs.Time + i]);
			}
		}

		public static void CheckMedian()
		{
			var data = new double[50];
			for (int i = 0; i < 50; i++)
			{
				data[i] = i;
			}
			var mean = SignalProcess.Median(data, 50);
			for (int i = 0; i < 50; i++)
			{
				Console.WriteLine(mean[i]);
			}
		}

	}
}
