using System;

namespace MotionDetection.Models
{
    public delegate void PlotOrientationHandeler(object sender, SingleDataEventArgs eventArgs);

    public class OrientationRecognition
    {
        private int _time;
        private double[] _cumulatedResult = new double[Parameters.NumUnity];
        private double[] _intraWindowRadians = new double[Parameters.NumUnity];
        private int _unityNumber;
 
        public event PlotOrientationHandeler OnPlotOrientation;

        public double[] RecognizeOrientation(double[] y, double[] z)
        {
            var result = new double[y.Length];
            var radians = new double[result.Length];

            var firstResult = Math.Atan2(y[0], z[0]);

            if (_time == 0)
            {
                result[0] = firstResult;
               
            }else
            {
                result[0] = Compare(Math.Atan2(y[0], z[0]), _intraWindowRadians[_unityNumber], _cumulatedResult[_unityNumber]);
            }

            radians[0] = firstResult;

            for (var i = 1; i < result.Length; i++)
            {
                radians[i] = Math.Atan2(y[i], z[i]);               

                result[i] = Compare(radians[i], radians[i - 1], result[i - 1]);
          
                if (i == result.Length / 2)
                {
                    _cumulatedResult[_unityNumber] = result[i];
                    _intraWindowRadians[_unityNumber] = radians[i];

                }
            }

            return result;
        }

        private int CalculateQuadrand(double theta)
        {
            if (theta >= 0 && theta <= Math.PI/2)
            {
                return 1;
            }
            if (theta >= Math.PI/2 && theta <= Math.PI)
            {
                return 2;
            }
            if (theta >= -Math.PI && theta <= -Math.PI/2)
            {
                return 3;
            }
            if (theta >= -Math.PI/2 && theta <= 0)
            {
                return 4;
            }
            return 0;
        }

        private double Compare(double actual, double prec, double resultPrec)
        {
            double result = 0;
            //da usare per capire di quanto ci si sposta. per capire se dx/sx usare il verso dei quadranti.
            double delta = 0;
            var precQuadrant = CalculateQuadrand(prec);
            var actualQuadrant = CalculateQuadrand(actual);

            if (precQuadrant == actualQuadrant)
            {
                delta = actual - prec;
                result = resultPrec + delta;

            }
            else
            {

                if (precQuadrant == 1)
                {
                    if (actualQuadrant == 2)
                    {
                        delta = actual - prec;
                        result = resultPrec + delta;
                    }
                    else
                    {
                        delta = prec + Math.Abs(actual);
                        result = resultPrec - delta;
                    }
                }
                if (precQuadrant == 2)
                {
                    if (actualQuadrant == 3)
                    {
                        delta = Math.PI - prec + Math.PI + actual;
                        result = resultPrec + delta;
                    }
                    //da qui rivedere
                    else
                    {
                        delta = prec - actual;
                        result = resultPrec - delta;
                    }
                }
                if (precQuadrant == 3)
                {
                    if (actualQuadrant == 4)
                    {
                        delta = prec - actual;
                        result = resultPrec + Math.Abs(delta);
                    }
                    else
                    {
                        delta = Math.PI + prec + Math.PI - actual;
                        result = resultPrec - Math.Abs(delta);
                    }
                }
                if (precQuadrant == 4)
                {
                    if (actualQuadrant == 1)
                    {
                        delta = Math.Abs(prec) + actual;
                        result = resultPrec + delta;
                    }
                    else
                    {
                        delta = actual - prec;
                        result = resultPrec - Math.Abs(delta);
                    }
                }
            }

        /*    if (_unityNumber == 4)
            {
                Console.WriteLine(
                    $"time \t {_time} \t prec \t {prec} \t actual \t {actual} \t delta \t {delta} \t precQuadrant \t {precQuadrant} \t actualQuadrant \t {actualQuadrant}");
            }*/

            return result;
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

        public void OnDataReceived(object sender, MultipleDataEventArgs eventArgs)
        {
            _time = eventArgs.Time;
            _unityNumber = eventArgs.UnityNumber;

            var result = RecognizeOrientation(eventArgs.SensorTwo, eventArgs.SensorThree);

            OnPlotOrientation?.Invoke(this, new SingleDataEventArgs
            {
                UnityNumber = eventArgs.UnityNumber,
                SensorOne = result,
                SeriesType = eventArgs.SeriesType,
                Time = _time
            });
        }
    }
}