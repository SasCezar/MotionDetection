using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using LiveCharts;
using LiveCharts.CoreComponents;
using TestMotionDetection.Annotations;

namespace TestMotionDetection
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            //In this case we will not only plot double values
            //to make it easier to handle "live-data" we are going to use DataViewModel class
            //we need to let LiveCharts know how to use this model
            //first we create a new configuration for DataViewModel
            SeriesConfiguration<DataViewModel> config = new SeriesConfiguration<DataViewModel>();
            //now we map X and Y
            //we will use Temperature as Y
            config.Y(model => model.Value);
            //and DateTime as X, we convert to OADate so we can plot it easly.
            config.X(model => model.Time);

            //now we create our series with this configuration
            _series = new SeriesCollection(config)
            {
                new LineSeries {Values = new ChartValues<DataViewModel>(), PointRadius = 0}
            };
            Series = new SeriesCollection(config)
            {
                new LineSeries {Values = new ChartValues<DataViewModel>(), PointRadius = 0}
            };

            //to display a custom label we will use a formatter,
            //formatters are just functions that take a double value as parameter
            //and return a string, in this case we will convert the OADate to DateTime
            //and then use a custom date format
            XFormatter = val => Math.Round(val) + " ms";

            //now for Y we are rounding and adding ° for degrees
            YFormatter = val => Math.Round(val) + " °";
         }

        private SeriesCollection _series = new SeriesCollection();

        public SeriesCollection Series
        {
            get
            {
                return _series;
            }
            set
            {
                _series[0].Values.Add(value);
                OnPropertyChanged("Series");
            }
        }
       
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void generateData()
        {
            DataViewModel val = new DataViewModel();
            val.Time = 5;
            val.Value = 2.3f;
            Series = val;
        }
    }
}
