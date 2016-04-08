using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace MotionDetection.Models
{
	public delegate void OnDataReceivedHandler(object sender, DataEventArgs eventArgs);

    public struct EulerAngles
    {
        public double Roll { get; set; }
        public double Pitch { get; set; }
        public double Yaw { get; set; }
    }

	public class DataManipulation
	{
	    private const int STDWindow = 7;
	    private Buffer3DMatrix<double> _buffer;

		public Buffer3DMatrix<double> Buffer
		{
			get { return _buffer; }
			set
			{
				if (_buffer != null)
				{
					throw new Exception("Cannot change the buffer");
				}
				_buffer = value;
			}
		}

		public int GlobalTime { get; set; }
		public event OnDataReceivedHandler NewDataReceived;

		public void Smoothing(CircularBuffer3DMatrix<double> circularBuffer, int windowSize)
		{
			for (var i = 0; i < _buffer.SensorType; i++)
			{
				for (var j = 0; j < _buffer.SensorNumber; j++)
				{
					for (var k = 0; k < _buffer.Time; k++)
					{
                        _buffer[i, j, k] = circularBuffer[i, j, k];
                        //var sum = 0.0;
                        //var start = FirstIndex(k, windowSize);
                        //var stop = LastIndex(k, windowSize, circularBuffer.Time);

                        //for (var h = start; h <= stop; ++h)
                        //{
                        //	//var msg = $"Start {start}, Stop {stop}, H {h}";
                        //	//MessageBox.Show(msg);
                        //	sum = sum + circularBuffer[i, j, h];
                        //}
                        //_buffer[i, j, k] = sum / (stop - start + 1);
                    }
				}
			}


            //Test
            var modulo = Modulo(_buffer.GetSubArray(0, 0), _buffer.GetSubArray(1, 0), _buffer.GetSubArray(2, 0));
			//var std = StandardDeviation(modulo);
		    var eul = EulerAnglesComputation(_buffer.GetSubArray(9, 0), _buffer.GetSubArray(10, 0), _buffer.GetSubArray(11, 0),
		        _buffer.GetSubArray(12, 0));

            
            double [] rolls = new double[eul.Length];
		    int index = 0;
		    foreach (var element in eul)
		    {
		        rolls[index] = element.Roll;
		        ++index;
		    }
            //EndTest

			var dataArgs = new DataEventArgs
			{
				SensorData = rolls,
				Time = GlobalTime
			};
			//Console.WriteLine($"Global time = {GlobalTime}");
			NewDataReceived?.Invoke(this, dataArgs);
			circularBuffer.UpdateReadIndex();
		}


        // TODO Rewrite Modulo method to return a double[] and accept vectors
        // TODO Same for all methods
        public static double[] Modulo(double[] x, double[]y, double[]z)
        {
            double [] result = new double[x.Length];

            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = Math.Sqrt(Math.Pow(x[i], 2) + Math.Pow(y[i], 2) + Math.Pow(z[i], 2));
            }

            return result;
        }

	    private double[] DifferenceQuotient(double[] x)
	    {
	        double[] result = new double[x.Length];

	        for (int i = 0; i < result.Length - 1; ++i)
	        {
	            result[i] = x[i + 1] - x[i];
	        }

	        return result;
	    }

	    private double[] StandardDeviation(double[] x)
	    {
            var result = new double[x.Length];
			var rawSquare = x.Select(value => Math.Pow(value, 2)).ToArray(); 
		    var meanSquare = Mean(x, STDWindow).Select(value => Math.Pow(value, 2)).ToArray();
			for (var i = 0; i < result.Length; ++i)
			{
				var start = FirstIndex(i, STDWindow);
			    var stop = LastIndex(i, STDWindow, result.Length);
			    var width = stop - start + 1;
                var segmentSum = new ArraySegment<double>(rawSquare, start, width).Sum();
				var value = segmentSum / width - meanSquare[i];
				result[i] = Math.Sqrt(value);
			}
		    return result; 
	    }

	    private double[] Mean(double[] x, int windowSize)
	    {
            double[] result = new double[x.Length];
            for (var i = 0; i < x.Length; i++)
            {
                var sum = 0.0;
                var start = FirstIndex(i, windowSize);
                var stop = LastIndex(i, windowSize, x.Length);

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
	        for (int i = 0; i < result.Length; ++i)
	        {
	            var roll = Math.Atan((2*q2[i]*q3[i] + 2*q0[i]*q1[i])/(2*Math.Pow(q0[i], 2) + 2*Math.Pow(q3[i], 2) - 1));
	            var pitch = -Math.Asin(2*q1[i]*q3[i] - 2*q0[i]*q2[i]);
                var yaw = Math.Atan((2 * q1[i] * q2[i] + 2 * q0[i] * q3[i]) / (2 * Math.Pow(q0[i], 2) + 2 * Math.Pow(q1[i], 2) - 1));

	            result[i] = new EulerAngles() {Roll = roll, Pitch = pitch, Yaw = yaw};
                //Console.WriteLine($"q0 \t {q0[i]} \t roll \t {roll}");
	        }

	        return result;
	    }

		private static int FirstIndex(int i, int width)
		{
			return ((i - width)/2) > 0 ? ((i - width)/2) : 0;

		}

		private static int LastIndex(int i, int width, int size)
		{
			if (i + width/2 < size)
			{
				return i + width/2;
			}
			return size - 1;
		}
	}
}