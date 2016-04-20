using System;
using System.Linq;

namespace MotionDetection.Models
{

	public delegate void MovementHadler(object sender, MotionEventArgs eventArgs);

	public class MotionRecognition
	{
		public event MovementHadler OnMovement;

		private const int STDWindow = 7;
        private const double Threshhold = 0.4;
        private DataManipulation DataManpulator { get; set; }
        private CircularBuffer3DMatrix<bool> isMoving = new CircularBuffer3DMatrix<bool>(1,1,75);

		public MotionRecognition(DataManipulation dataManipulator)
		{
			DataManpulator = dataManipulator;
			DataManpulator.NewDataReceived += OnDataRecived;
			OnMovement += PrintMovements;
		}

		public void OnDataRecived(object sender, DataEventArgs data)
		{
			if (data.SeriesType == 0 && data.SensorNumber == 0)
			{
				var stdout = StandardDeviation(data.SensorData);
				RecognizeStatus(stdout, data.Time);
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

		private static double[] StandardDeviation(double[] x)
		{
			var result = new double[x.Length];
			var rawSquare = x.Select(value => Math.Pow(value, 2)).ToArray();
			var meanSquare = Mean(x, STDWindow).Select(value => Math.Pow(value, 2)).ToArray();
			for (var i = 0; i < result.Length; ++i)
			{
				var start = DataManipulation.FirstIndex(i, STDWindow);
				var stop = DataManipulation.LastIndex(i, STDWindow, result.Length);
				var width = stop - start + 1;
				var segmentSum = new ArraySegment<double>(rawSquare, start, width).Sum();
				var value = segmentSum / width - meanSquare[i];
				result[i] = Math.Sqrt(value);
			}
			return result;
		}

		private static double[] Mean(double[] x, int windowSize)
		{
			var result = new double[x.Length];
			for (var i = 0; i < x.Length; i++)
			{
				var sum = 0.0;
				var start = DataManipulation.FirstIndex(i, windowSize);
				var stop = DataManipulation.LastIndex(i, windowSize, x.Length);

				for (var h = start; h <= stop; ++h)
				{
					sum = sum + x[h];
				}
				result[i] = sum / (stop - start + 1);
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

	    public void RecognizeStatus(double[] std, int time)
	    {
	        int i = time - std.Length;
	        foreach (var data in std)
	        {
		        isMoving[0,0,i] = (data > Threshhold) || isMoving[0,0,i];
		        ++i;
	        }

	        if (time > 75 && (time - 25)%50 == 0)
	        {
                OnMovement?.Invoke(this, new MotionEventArgs()
                {
                    MotionData = isMoving,
                    Time = time
                });
            }
	    }

		protected void PrintMovements(object sender, MotionEventArgs args)
	    {
	        var start = args.Time - 75;
	        var finish = start + 50;
	        for (var i = start; i < finish; ++i)
	        {
	            var status = args.MotionData[0,0,i] ? "Movimento" : "Fermo";
 	            //Console.WriteLine($"Time \t {args.Time} \t {status}");
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
