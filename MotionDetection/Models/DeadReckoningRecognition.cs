using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MotionDetection.ViewModels;

namespace MotionDetection.Models
{
    public delegate void DeadReckoningHandeler(object sender, MultipleDataEventArgs eventArgs);

    public class DeadReckoningRecognition
    {
        private int _time;
        private double _threshold = 0.3;
        public event DeadReckoningHandeler OnDeadReckoningRecognizedHandler;
        private double x;
        private double y;



        public double[] RecognizeDeadReckoning(double[] stdDev, double[] posture, EulerAngles[] eulerAngles)
        {
            var result = new double[stdDev.Length];
            var numOfSlices = 25;

            var xPoints = new double[numOfSlices];
            var yPoints = new double[numOfSlices];

            for (int i = 0; i < numOfSlices; ++i)
            {
                var slice = new ArraySegment<double>(stdDev, i * stdDev.Length/numOfSlices, stdDev.Length/numOfSlices);
                var distance = slice.Where(p => p > _threshold).Sum() / (ViewModelWindow.StaticBufferSize * 8);
                var angle = eulerAngles[i*stdDev.Length/numOfSlices].Yaw;
                
                var sin = Math.Sin(angle);
                var cos = Math.Cos(angle);



                x = x + sin*distance;               
                y = y + cos*distance;

                //Console.WriteLine($"distance \t {distance} \t x \t {x} \t y \t {y}");

                xPoints[i] = x;
                yPoints[i] = y;

            }

            OnDeadReckoningRecognizedHandler?.Invoke(this, new MultipleDataEventArgs
            {
                SensorOne = xPoints,
                SensorTwo = yPoints
            });             
      

            return result;
        }

        private EulerAngles[] EulerAnglesComputation(double[] q0, double[] q1, double[] q2, double[] q3)
        {
            var result = new EulerAngles[q0.Length];
            for (var i = 0; i < result.Length; ++i)
            {
                var roll = Math.Atan2(2*q2[i]*q3[i] + 2*q0[i]*q1[i], 2*Math.Pow(q0[i], 2) + 2*Math.Pow(q3[i], 2) - 1);
                var pitch = -Math.Asin(2*q1[i]*q3[i] - 2*q0[i]*q2[i]);
                var yaw = Math.Atan2(2*q1[i]*q2[i] + 2*q0[i]*q3[i], 2*Math.Pow(q0[i], 2) + 2*Math.Pow(q1[i], 2) - 1);

                result[i] = new EulerAngles {Roll = roll, Pitch = pitch, Yaw = yaw};
            }


            return result;
        }

        public void OnDataReceived(object sender, DeadArgs eventArgs)
        {
            var buffer = eventArgs.Data;

            var eulerAngles = EulerAnglesComputation(buffer.GetSubArray(0, (int) SensorType.Quaternion0),
                buffer.GetSubArray(0, (int) SensorType.Quaternion1), buffer.GetSubArray(0, (int) SensorType.Quaternion2),
                buffer.GetSubArray(0, (int) SensorType.Quaternion3));

            RecognizeDeadReckoning(eventArgs.std, eventArgs.posture, eulerAngles);

            //for (int i = 0; i < 50; i++)
            //{
            //    Console.WriteLine("DRSAYSHI" + eventArgs.std[i]); 
            //}
        }
    }
}