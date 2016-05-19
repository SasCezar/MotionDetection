using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotionDetection.ViewModels;
using OxyPlot.Series;

namespace MotionDetection.Models
{
    public class Printer
    {
        private StringBuilder CsvFile;
        public static int[] IsMoving;
        public static double[] Posture;
        public static int[] IsTurning;
        private string FilePath;
        public int Time { get; set; }
        TimeSpan printStartTime;

        public Printer(string filePath)
        {
            FilePath = filePath;
        }

        public void PrintLog()
        {
            CsvFile = new StringBuilder();
            var start = Time < ViewModelWindow.StaticBufferSize / 2 ? 0 : IsMoving.Length / 2;
            for (int i = start; i < IsMoving.Length; ++i)
            {
                if (i == 0 || IsMoving[i] != IsMoving[i - 1] || IsTurning[i] != IsTurning[i - 1] ||
                    Posture[i] != Posture[i - 1])
                {
                    var moving = ((Motion)IsMoving[i]).ToString();
                    var turning = ((Turning)IsTurning[i]).ToString();
                    var posture = ((Posture)Posture[i]).ToString(); 

                    var printFinishTime = DateTime.Now.TimeOfDay;
                    string record = $"{printStartTime.ToString("hh\\:mm\\:ss")}, {printFinishTime.ToString("hh\\:mm\\:ss")}, {moving}, {posture}, {turning}";
       
                    printStartTime = printFinishTime;
                    CsvFile.AppendLine(record);
                   
                }               
            }
            File.AppendAllText(FilePath, CsvFile.ToString());
        }


        public void OnDataAnalyzed(object sender, MultipleDataEventArgs args)
        {
            if (args.Time == 0)
            {
               printStartTime = DateTime.Now.TimeOfDay;
            }
            Time = args.Time;
            PrintLog();
        }
    }
    
    

    public enum Motion
    {
        Still,
        Moving
    }

    public enum Turning
    {
        Left = -1,
        Right = 1,
        No = 0
    }

    public enum Posture
    {
        Laying,
        LaySitting,
        Sitting,
        Standing
    }
}
