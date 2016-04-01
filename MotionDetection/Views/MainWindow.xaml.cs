using System.Threading.Tasks;
using System.Windows;
using LiveCharts.CoreComponents;
using MotionDetection.Models;
using MotionDetection.ViewModels;

namespace MotionDetection.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ViewModelWindow ViewModel)
        {
            InitializeComponent();


            //Don't forget DataContext so we can bind these properties.
            DataContext = ViewModel;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Factory.StartNew(() => dataReceiver.Start, TaskCreationOption.LongRunning);
        }

     /*   public void OnDataReceived(object sender, DataEventArgs eventData)
        {
            Dispatcher.Invoke(() =>
            {
                listBox.Items.Add(eventData.SensorData.Value + " " + eventData.SensorData.Time + " " +
                                  eventData.SensorData.SensorType);
                foreach (var series in Series)
                {
                    if (series.Values.Count > 500)
                    {
                        series.Values.RemoveAt(0);
                    }
                    series.Values.Add(eventData.SensorData);
                }
            });
        }*/
    }
}