using System;
using System.Windows;
using LiveCharts;
using MotionDetection.Commands;
using MotionDetection.Models;

namespace MotionDetection.ViewModels
{
    public class ViewModelWindow
    {
        public DataReceiver Receiver;
        public ConnectionCommand Command;
        public SeriesCollection Series { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }

        public ViewModelWindow()
        {
            Receiver = new DataReceiver();
            Command = new ConnectionCommand(Receiver);
            var config = new SeriesConfiguration<DataViewModel>();

            config.Y(model => model.Value);
            config.X(model => model.Time);


            Series = new SeriesCollection(config)
            {
                new LineSeries {Values = new ChartValues<DataViewModel>(), PointRadius = 0}
            };


            XFormatter = val => Math.Round(val) + " s";
            YFormatter = val => Math.Round(val) + " °";
        }

      /*  private void button_Click(object sender, RoutedEventArgs e)
        {
            var dataReceiver = new DataReceiver();
            dataReceiver.NewDataReceived += OnDataReceived;

            //await Task.Factory.StartNew(() => dataReceiver.Start, TaskCreationOption.LongRunning);


            dataReceiver.Start();
        }*/
    }
}
