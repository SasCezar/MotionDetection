using System;
using System.Linq;

namespace MotionDetection.Models
{

	public delegate void MovementHadler(object sender, MotionEventArgs eventArgs);
	public delegate void PlotMovementHandeler(object sender, PlotEventArgs eventArgs);

	public class MotionRecognition
	{
		public event MovementHadler OnMovement;

		public event PlotMovementHandeler OnPlotMovement;

		private const double Threshhold = 0.4;
		//private DataManipulation DataManpulator { get; set; }
		private CircularBuffer<bool> isMoving = new CircularBuffer<bool>(Parameters.CircularBufferSize);

		public MotionRecognition()
		{
			OnMovement += PrintMovements;
		}

		public void OnDataRecived(object sender, PlotEventArgs data)
		{
			if (data.SeriesType == 0 && data.UnityNumber == 0)
			{
				var stdout = DataManipulation.StandardDeviation(data.SensorData, 11);
				var medData = SignalProcess.Median(stdout, 31);


				OnPlotMovement?.Invoke(this, new PlotEventArgs()
				{
					UnityNumber = data.UnityNumber,
					SeriesType = 3,
					SensorData = medData,
					Time = data.Time
				});

				RecognizeStatus(medData, data.Time);
			}
		}

		private double[] DifferenceQuotient(double[] x)
		{
			var result = new double[x.Length];

			for (var i = 0; i < result.Length - 1; ++i)
			{
				result[i] = x[i + 1] - x[i];
			}
			return result;
		}


		private EulerAngles[] EulerAnglesComputation(double[] q0, double[] q1, double[] q2, double[] q3)
		{
			var result = new EulerAngles[q0.Length];
			for (var i = 0; i < result.Length; ++i)
			{
				var roll = Math.Atan2(2 * q2[i] * q3[i] + 2 * q0[i] * q1[i], 2 * Math.Pow(q0[i], 2) + 2 * Math.Pow(q3[i], 2) - 1);
				var pitch = -Math.Asin(2 * q1[i] * q3[i] - 2 * q0[i] * q2[i]);
				var yaw = Math.Atan2(2 * q1[i] * q2[i] + 2 * q0[i] * q3[i], 2 * Math.Pow(q0[i], 2) + 2 * Math.Pow(q1[i], 2) - 1);

				result[i] = new EulerAngles { Roll = roll, Pitch = pitch, Yaw = yaw };
			}

			return result;
		}

		public void RecognizeStatus(double[] std, int startTime)
		{
			int i = startTime;
			foreach (var data in std)
			{
				isMoving[i] = (data > Threshhold) || isMoving[i];
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
				var status = args.MotionData[i] ? "Movimento" : "Fermo";
				Console.WriteLine($"Time \t {args.Time} \t {status}");
			}
		}

		public double[] RecognizeOrientation(double[] y, double[] z)
		{
			double[] result = new double[y.Length];

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = Math.Atan2(y[i], z[i]);

			}  
			return result;
		}
	}
}