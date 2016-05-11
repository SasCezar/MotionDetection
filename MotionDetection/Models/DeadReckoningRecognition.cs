using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionDetection.Models
{
    class DeadReckoningRecognition
    {
        public double[] RecognizeDeadReckoning(double[] stdDev, double[] posture)
        {
            var result = new double[stdDev.Length];

            return result;
        }


        public void OnDataReceived(object sender, DeadArgs eventArgs)
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    Console.WriteLine("DRSAYSHI" + eventArgs.std[i]); 
            //}
        }
    }
}
