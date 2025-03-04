﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionDetection.ViewModels;

namespace MotionDetection.Models
{

    public delegate void PostureHandeler(object sender, SingleDataEventArgs eventArgs);

    class PostureRecognition
    {
        public event PostureHandeler OnPostureRecognizedHandeler;

        int WindowSize = ViewModelWindow.StaticBufferSize / 50 * 11;

        public double[] RecognizePosture(double[] accelerometer)
        {
            var result = new double[accelerometer.Length];

            for (int i = 0; i < result.Length; i++)
            {
                var value = accelerometer[i];

                if (value <= 2.7)
                {
                    result[i] = 0;
                }
                if (value > 2.7 && value <= 3.7)
                {
                    result[i] = 1;
                }
                if (value > 3.7 && value <= 7)
                {
                    result[i] = 2;
                }
                if (value > 7)
                {
                    result[i] = 3;
                }
            }
           
            result = SignalProcess.Median(result, WindowSize);
            Printer.Posture = result;
            return result;
        }

        public void OnDataReceived(object sender, BufferEventArgs<double> bufferEventArgs)
        {
            var result = RecognizePosture(bufferEventArgs.Data.GetSubArray(0, (int)SensorType.Accelerometer1));
            OnPostureRecognizedHandeler?.Invoke(this, new SingleDataEventArgs
            {
                SensorOne = result,
                SeriesType = (int)SeriesType.Posture,
                Time = bufferEventArgs.Time,
                UnityNumber = 0
            });
        }
    }
}
