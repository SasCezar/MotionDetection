using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.Series;

namespace MotionDetection.Models
{
    class Printer
    {
        private CandleStickAndVolumeSeries CsvFile;
        public static int[] IsMoving;
        public static int[] Posture;
        public static int[] IsTurning;


        public Printer(CandleStickAndVolumeSeries csvFile)
        {
            CsvFile = csvFile;
        }

        public void printLog()
        {
           

        }

       
    }

    public enum Motion
    {
        still,
        moving
    }

    public enum Turning
    {
        dx,
        sx,
        no
    }

    public enum Posture
    {
        laying,
        laySitting,
        sitting,
        standing

    }
}
