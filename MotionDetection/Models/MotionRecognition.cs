using System;

namespace MotionDetection.Models
{
	public delegate void MovementHadler(object sender, MotionEventArgs eventArgs);

	public delegate void PlotMovementEventHandeler(object sender, SingleDataEventArgs eventArgs);

	public class MotionRecognition
	{
		private const double Threshhold = 0.4;
		private CircularBuffer<int> isMoving = new CircularBuffer<int>(Parameters.CircularBufferSize);
		//private DataManipulation DataManpulator { get; set; }

		public event MovementHadler OnMovement;

        public event PlotMovementEventHandeler OnPlotMovementEventHandler;

		public void OnDataReceived(object sender, SingleDataEventArgs data)
		{
			if (data.SeriesType == 0 && data.UnityNumber == 0)
			{
				var stdout = DataManipulation.StandardDeviation(data.SensorOne, 11);
				var medData = SignalProcess.Median(stdout, 31);

				OnPlotMovementEventHandler?.Invoke(this, new SingleDataEventArgs
				{
					UnityNumber = data.UnityNumber,
					SeriesType = 3,
					SensorOne = medData,
					Time = data.Time
				});

				RecognizeStatus(medData, data.Time);
			}
		}

		

		public void RecognizeStatus(double[] std, int startTime)
		{
			var i = startTime;
			foreach (var data in std)
			{
				isMoving[i] = (data > Threshhold) || (isMoving[i] == 1) ? 1 : 0;
				++i;
			}

			OnMovement?.Invoke(this, new MotionEventArgs()
			{
				MotionData = isMoving,
				Time = startTime
			});
		}

		protected void PrintMovements(object sender, MotionEventArgs args)
		{
			var start = args.Time;
			var finish = start + Parameters.StaticBufferSize;
			for (var i = start; i < finish; ++i)
			{
				var status = args.MotionData[i]==1 ? "Movimento" : "Fermo";
				Console.WriteLine($"Time \t {args.Time} \t {status}");
			}
		}
	}
}