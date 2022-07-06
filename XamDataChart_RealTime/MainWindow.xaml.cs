using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace XamDataChart_RealTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChartDataSource _data;

        public MainWindow()
        {
            InitializeComponent();

            _data = new ChartDataSource();
            this.DataContext = _data;

            xAxis.MaximumValue = DateTime.Today.AddMinutes(60);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            for (int i = 0; i < 120; i++)
            {
                _data.addDataItem();
            }

            if (_data.Count > 840)
            {
                xAxis.MinimumValue = xAxis.ActualMinimumValue.AddSeconds(600);
                xAxis.MaximumValue = xAxis.ActualMaximumValue.AddSeconds(600);

                for (int i = 0; i < 120; i++)
                {
                    _data.RemoveAt(0);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
    }

    public class ChartDataSource : ObservableCollection<MyDataPoint>
    {
        private Random _rand = new Random();
        private float _baseVal = 50;
        DateTime dt = DateTime.Today;

        public ChartDataSource()
        {
            addDataItem();
        }

        public void addDataItem()
        {
            if (_rand.NextDouble() > .5)
            {
                _baseVal += _rand.Next(0, 5);
                if (_baseVal > 100)
                { _baseVal = 100; }
            }
            else
            {
                _baseVal -= _rand.Next(0, 5);
                if (_baseVal < 0)
                { _baseVal = 0; }
            }

            this.Add(new MyDataPoint { MyDate = dt, MyValue = _baseVal });
            dt = dt.AddSeconds(5);
        }
    }

    public class MyDataPoint
    {
        public DateTime MyDate { get; set; }
        public double MyValue { get; set; }
    }
}
