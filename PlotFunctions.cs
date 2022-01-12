using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OxyPlot;
using OxyPlot.Wpf;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace МетодЕйлераРунгеКутта
{
    class PlotFunctions
    {
        public static void PlotFunction(Plot plot, Function function, double a, double b, string title, SolidColorBrush brush, double h)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            for (double x = a; x-0.005 <= b; x += h)
            {
                dataPoints.Add(new DataPoint(x, function.result(x)));
                //MessageBox.Show(new DataPoint(x, function.result(x)).ToString());
            }
            plot.Series.Add(new LineSeries { Title = title, Color = brush.Color });
            plot.Series[plot.Series.Count-1].ItemsSource = dataPoints;
        }

        public static void PlotFunction(Plot plot, double [] X, double [] Y, string title)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            for(int i=0; i<X.Length; i++)
            {
                dataPoints.Add(new DataPoint(X[i], Y[i]));
            }

            plot.Series.Add(new LineSeries { Title = title });
            plot.Series[plot.Series.Count - 1].ItemsSource = dataPoints;
        }

        public static void PlotFunction(Plot plot, List<DataPoint> dataPoints, string title, SolidColorBrush brush)
        {
            plot.Series.Add(new LineSeries { Title = title , Color = brush.Color});
            plot.Series[plot.Series.Count - 1].ItemsSource = dataPoints;
        }

        public static void PlotPoint(Plot plot, Function function, double x)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint(x, function.result(x)));

            plot.Series.Add(new LineSeries { LineStyle = LineStyle.None, MarkerSize = 3, MarkerFill = Brushes.Red.Color, MarkerType = MarkerType.Diamond });
            plot.Series[plot.Series.Count - 1].ItemsSource = dataPoints;
        }
    }
}
