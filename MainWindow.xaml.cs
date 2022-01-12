using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using OxyPlot;
using System.IO;
using Microsoft.Win32;
using OxyPlot.Wpf;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace МетодЕйлераРунгеКутта
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DifferentialEquation equation;

        int numberEducation;
        double x0 = 0;
        double xn = 0;
        double n = 0;
        List<Function> functions = new List<Function>();
        List<double> y0 = new List<double>();
        string[] variables = new string[0];
        List<Function> functionsExact = new List<Function>();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            equation = new DifferentialEquation();
            numberEducation = 0;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            stackPanelLobattoEqution.Children.Add(SystemEducation.Equation(++numberEducation));
            stackPanelExactEquation.Children.Add(SystemEducation.ExactEquation(numberEducation));
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (numberEducation < 2)
			{
				stackPanelLobattoEqution.Children.Add(SystemEducation.Equation(++numberEducation));
				stackPanelExactEquation.Children.Add(SystemEducation.ExactEquation(numberEducation));
			}
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (numberEducation > 0)
			{
				stackPanelLobattoEqution.Children.RemoveAt(--numberEducation);
				stackPanelExactEquation.Children.RemoveAt(numberEducation);
			}



        }

			private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Graphics.Series.Clear();

            //try
            //{
	        

			List<StackPanel> stackPanels = stackPanelLobattoEqution.Children.OfType<StackPanel>().ToList();
            functions = new List<Function>();
            y0 = new List<double>();
            variables = new string[numberEducation + 1];

            variables[0] = "x";
            for (int i = 1; i <= numberEducation; i++)
            {
                variables[i] = "y" + i;
            }
            //variables = new string[] { "x", "y1", "y2", "y3", "y4",  };


            for (int i = 0; i < stackPanels.Count; i++)
            {
                functions.Add(new Function((stackPanels[i].Children[1] as ComboBox).Text, variables));
            }

            for (int i = 0; i < stackPanels.Count; i++)
            {
                y0.Add(double.Parse((stackPanels[i].Children[3] as TextBox).Text));
            }

            x0 = double.Parse(textBoxX0.Text);
            xn = double.Parse(textBoxXN.Text);
            n = double.Parse(textBoxN.Text);
	        double h = (xn - x0) / n;
			stackPanels = stackPanelExactEquation.Children.OfType<StackPanel>().ToList();
            functionsExact = new List<Function>();
            for (int i = 0; i < stackPanels.Count; i++)
            {
                functionsExact.Add(new Function((stackPanels[i].Children[1] as ComboBox).Text, new string[]{"x"}));
                PlotFunctions.PlotFunction(Graphics, functionsExact[i], x0, xn, "y" + (i + 1) + "  Exact", Brushes.Blue, 0.01);
            }


            

	        if (numberEducation == 1)
	        {
		        equation = new DifferentialEquation(x0, xn, 3, y0, functions);
				double u1 = 0, u2 = 0, u3 = 0;
		        if (uExact.IsChecked == true)
		        {
			        u1 = functionsExact[0].result(x0 + (xn - x0) / n);
			        u2 = functionsExact[0].result(x0 + (2 * (xn - x0)) / n);
			        u3 = functionsExact[0].result(x0 + (3 * (xn - x0)) / n);
		        }
		        else if (uEuler.IsChecked == true)
		        {
			        List<DataPoint> dataPoints = equation.MethodsEuler();
			        u1 = dataPoints[1].Y;
			        u2 = dataPoints[2].Y;
			        u3 = dataPoints[3].Y;

		        }
		        else if (uRungeKutta.IsChecked == true)
		        {
			        List<DataPoint> dataPoints = equation.MethodsRungeKutta();
			        u1 = dataPoints[1].Y;
			        u2 = dataPoints[2].Y;
			        u3 = dataPoints[3].Y;
		        }

		        equation = new DifferentialEquation(x0, xn, n, y0, functions, u1, u2, u3);

		        List<DataPoint> points = equation.MethodsAdamsa();

		        PlotFunctions.PlotFunction(Graphics, points, "y1  Adams", Brushes.Red);
		        PlotX.Minimum = equation.x0;
		        PlotX.Maximum = equation.xn;

		        datagridAdams.ItemsSource = points;
		        textBoxEN.Text = Math.Abs(points.Last().Y - functionsExact[0].result(xn)).ToString();
			}
	        else if(numberEducation==2)
	        {
		        equation = new DifferentialEquation(x0, x0+h, 1, y0, functions);
				double[] u1 = new double[2];
		        if (uExact.IsChecked == true)
		        {
			        u1[0] = functionsExact[0].result(x0 + h);
			        u1[1] = functionsExact[1].result(x0 + h);
		        }
		        else if (uEuler.IsChecked == true)
		        {
			        List<List<DataPoint>> dataPoints = equation.MethodsEuler2();
			        u1[0] = dataPoints[0].Last().Y;
			        u1[1] = dataPoints[1].Last().Y;

				}
		        else if (uRungeKutta.IsChecked == true)
		        {
			        List<List<DataPoint>> dataPoints = equation.MethodsRungeKutta2();
			        u1[0] = dataPoints[0].Last().Y;
			        u1[1] = dataPoints[1].Last().Y;
		        }

		        equation = new DifferentialEquation(x0, xn, n, y0, functions, u1);

		        List<List<DataPoint>> points = equation.MethodsAdamsa2();
		        for (int i = 0; i < points.Count; i++)
		        {
			        PlotFunctions.PlotFunction(Graphics, points[i], "y"+(i+1)+"  Adams", Brushes.Red);
				}
		        
		        PlotX.Minimum = equation.x0;
		        PlotX.Maximum = equation.xn;
		        List<object> list = new List<object>();
		        for (int i = 0; i < points[0].Count; i++)
		        {
			        list.Add(new { x = points[0][i].X, y1 = points[0][i].Y, y2 = points[1][i].Y });
		        }
		        //datagridEuler.ItemsSource = list
		        datagridAdams.ItemsSource = list;
		        textBoxEN.Text = Math.Max(Math.Abs(points[0].Last().Y - functionsExact[0].result(xn)), Math.Abs(points[1].Last().Y - functionsExact[1].result(xn))).ToString();
	        }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error enter data");
            //    return;
            //}
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            double a = double.Parse(textBoxATest.Text);
            double b = double.Parse(textBoxBTest.Text);
            PlotXTest.Minimum = a;
            PlotXTest.Maximum = b;

            PlotFunctions.PlotFunction(GraphicsTest, new Function(textBoxFTest.Text, "x"), a, b, "f", Brushes.Blue, 0.1);
        }

        //private void ButtonSolve_Click(object sender, RoutedEventArgs e)
        //{


        //    Graphics.Series.Clear();

        //    string strFunction = comboBoxSelectFunction.Text;

        //    List<Function> functions = new List<Function>();
        //    functions.Add(new Function(strFunction, "x", "y"));




        //    PlotX.Minimum = methods.x0;
        //    PlotX.Maximum = methods.xn;
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            GraphicsTest.Series.Clear();
            GraphicsTest.Series.Add(new LineSeries());
            GraphicsTest.Series[0].ItemsSource = new List<DataPoint>();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //x0 =0;
            //xn =0;
            //n =0;
            //functionsLobatto= new List<Function>();
            //y0 = new List<double>();
            //variables = new string[0];
            //functionsExact = new List<Function>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
               DataFile.ReadFile(openFileDialog.FileName, ref x0, ref xn, ref n, ref functions, ref y0, ref variables, ref functionsExact, ref numberEducation);

            stackPanelLobattoEqution.Children.Clear();
            stackPanelExactEquation.Children.Clear();
            for(int i=0; i<numberEducation; i++)
            {
                stackPanelLobattoEqution.Children.Add(SystemEducation.Equation(i+1, functions[i].strFunction, y0[i].ToString()));
                stackPanelExactEquation.Children.Add(SystemEducation.ExactEquation(i+1, functionsExact[i].strFunction));

            }
            textBoxX0.Text = x0.ToString();
            textBoxXN.Text = xn.ToString();
            textBoxN.Text = n.ToString();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                DataFile.SaveFile(saveFileDialog.FileName, x0, xn, n, functions, y0, functionsExact, numberEducation);
        }
    }
}
