using System;

namespace MotionDetection.Models
{
	public delegate void SignalProcessedEventHandeler(object sender, BufferEventArgs<double> eventArgs);

	public class SignalProcess
	{
		public const int WindowSize = 11;
		public Buffer3DMatrix<double> Buffer;

		public SignalProcess()
		{
			Buffer = new Buffer3DMatrix<double>(Parameters.NumUnity, Parameters.NumSensor, Parameters.StaticBufferSize);
		}

		public event SignalProcessedEventHandeler OnSignalProcessedEventHandler;

		public void Smoothing(CircularBuffer3DMatrix<double> circularBuffer, int windowStartTime)
		{
			for (var i = 0; i < Buffer.UnityNumber; i++)
			{
				for (var j = 0; j < Buffer.SensorType; j++)
				{
					for (var k = 0; k < Buffer.Time; k++)
					{
						var sum = 0.0;
						var start = Utils.FirstIndex(windowStartTime + k, WindowSize);
						var stop = Utils.LastIndex(windowStartTime + k, WindowSize, windowStartTime + Parameters.StaticBufferSize);
						//Console.WriteLine($"wst \t {windowStartTime} \t start \t {start} \t stop \t {stop}");
						for (var h = start; h <= stop; ++h)
						{
							sum = sum + circularBuffer[i, j, h];
						}
						Buffer[i, j, k] = sum/(stop - start + 1);
					}
				}
			}

			OnSignalProcessedEventHandler?.Invoke(this, new BufferEventArgs<double>
			{
				Data = Buffer,
				Time = windowStartTime
			});
		}

		public void OnDataReceived(object sender, BufferEventArgs<double> args)
		{
			Smoothing((CircularBuffer3DMatrix<double>) args.Data, args.Time);
		}
	}
}