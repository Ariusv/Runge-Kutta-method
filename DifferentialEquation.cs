using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using OxyPlot;
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
	class DifferentialEquation
	{
		public double x0;
		public double xn;
		public double n;

		public int N;

		public List<double> y0;


		public List<Function> functions;


		//adamsa
		double u1;
		double u2;
		double u3;

		//systemadams
		private double[] u;

		public DifferentialEquation()
		{
		}

		public DifferentialEquation(double x0, double xn, double n, List<double> y0, List<Function> functions)
		{
			this.x0 = x0;
			this.xn = xn;
			this.n = n;
			this.y0 = y0;
			this.functions = functions;
			this.N = y0.Count;
		}

		public DifferentialEquation(double x0, double xn, double n, List<double> y0, List<Function> functions, double [] U)
		{
			this.x0 = x0;
			this.xn = xn;
			this.n = n;
			this.y0 = y0;
			this.functions = functions;
			this.N = y0.Count;
			u = new double[2];
			for (int i = 0; i < U.Length; i++)
			{
				this.u[i] = U[i];
			}
		}

		public DifferentialEquation(double x0, double xn, double n, List<double> y0, List<Function> functions,
			double u1, double u2, double u3)
		{
			this.x0 = x0;
			this.xn = xn;
			this.n = n;
			this.y0 = y0;
			this.functions = functions;
			this.N = y0.Count;
			this.u1 = u1;
			this.u2 = u2;
			this.u3 = u3;
		}


		public List<List<DataPoint>> MethodsLobatto()
		{
			List<List<DataPoint>> dataPoints = new List<List<DataPoint>>();
			for (int k = 0; k < N; k++)
			{
				dataPoints.Add(new List<DataPoint>());
				dataPoints[k].Add(new DataPoint(x0, y0[k]));
			}

			double[] b = new double[4] {1 / (double) 12, 5 / (double) 12, 5 / (double) 12, 1 / (double) 12};
			double[] c = new double[4] {0, (5 - Math.Sqrt(5)) / (double) 10, (5 + Math.Sqrt(5)) / (double) 10, 1};
			double[,] a = new double[4, 4]
			{
				{0, 0, 0, 0},
				{(5 + Math.Sqrt(5)) / (double) 60, 1 / (double) 6, (15 - 7 * Math.Sqrt(5)) / (double) 60, 0},
				{(5 - Math.Sqrt(5)) / (double) 60, (15 + 7 * Math.Sqrt(5)) / (double) 60, 1 / (double) 6, 0},
				{1 / (double) 6, (5 - Math.Sqrt(5)) / (double) 12, (5 + Math.Sqrt(5)) / (double) 12, 0}
			};

			double s = 4;
			double h = (xn - x0) / n;

			double xi = x0;
			List<double> yi = Copy(y0);

			double[] k1 = new double[N];
			double[] k2 = new double[N];
			double[] k3 = new double[N];
			double[] k4 = new double[N];

			while (xi < xn)
			{


				for (int k = 0; k < N; k++)
				{

					k1[k] = functions[k].result(ToArray(xi, yi));
					k2[k] = functions[k].result(ToArray(xi, yi));
					k3[k] = functions[k].result(ToArray(xi, yi));
					k4[k] = functions[k].result(ToArray(xi, yi));
				}

				double[][] K = new double[4][] {k1, k2, k3, k4};

				double[,] KPlusOne = new double[4, N];

				double x;
				while (true)
				{
					for (int j = 0; j < s; j++)
					{

						x = xi + h * c[j];

						List<double> y = new List<double>();
						for (int k = 0; k < N; k++)
						{
							y.Add(new double());
							y[k] = yi[k];
							for (int i = 0; i < s; i++)
							{
								y[k] += h * a[j, i] * K[i][k];
							}
						}

						for (int k = 0; k < N; k++)
						{
							KPlusOne[j, k] = functions[k].result(ToArray(x, y));

						}

					}

					bool isEps = true;
					double eps = 0.000001;

					for (int k = 0; k < N; k++)
					{
						for (int j = 0; j < s; j++)
						{
							if (Math.Abs((KPlusOne[j, k] - K[j][k]) / K[j][k]) > eps) isEps = false;
						}
					}

					if (isEps)
					{
						break;
					}
					else
					{
						for (int k = 0; k < N; k++)
						{
							for (int j = 0; j < s; j++)
							{
								K[j][k] = KPlusOne[j, k];
							}
						}
					}
				}

				xi = xi + h;
				for (int k = 0; k < N; k++)
				{
					for (int j = 0; j < s; j++)
					{
						yi[k] += h * b[j] * K[j][k];
					}

					dataPoints[k].Add(new DataPoint(xi, yi[k]));
				}
			}

			return dataPoints;
		}

		public List<DataPoint> MethodsRungeKutta()
		{

			//if(x0 > xn)
			//{
			//    throw new Exception("Не правильні початкові дані");
			//}

			List<DataPoint> dataPoints = new List<DataPoint>();

			dataPoints.Add(new DataPoint(x0, y0[0]));

			double h = (xn - x0) / n;

			double xi = x0;
			double yi = y0[0];
			double k1 = 0;
			double k2 = 0;
			double k3 = 0;
			double k4 = 0;
			int i = 0;
			while (i < n)
			{
				k1 = functions[0].result(xi, yi);
				k2 = functions[0].result(xi + h / 2, yi + h * k1 / 2);
				k3 = functions[0].result(xi + h / 2, yi + h * k2 / 2);
				k4 = functions[0].result(xi + h, yi + h * k3);
				yi = yi + (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
				xi = xi + h;

				xi = Math.Round(xi, 8);
				dataPoints.Add(new DataPoint(xi, yi));
				i++;
			}

			return dataPoints;
		}

		public List<DataPoint> MethodsAdamsa()
		{

			//if(x0 > xn)
			//{
			//    throw new Exception("Не правильні початкові дані");
			//}
			int i = 0;
			List<DataPoint> dataPoints = new List<DataPoint>();



			double h = (xn - x0) / n;

			List<double> xi = new List<double>();
			List<double> yi = new List<double>();
			xi.Add(x0);
			yi.Add(y0[0]);
			dataPoints.Add(new DataPoint(xi.Last(), yi.Last()));
			xi.Add(xi.Last() + h);
			yi.Add(u1);
			dataPoints.Add(new DataPoint(xi.Last(), yi.Last()));
			xi.Add(xi.Last() + h);
			yi.Add(u2);
			dataPoints.Add(new DataPoint(xi.Last(), yi.Last()));
			xi.Add(xi.Last() + h);
			yi.Add(u3);
			dataPoints.Add(new DataPoint(xi.Last(), yi.Last()));
			i = 3;
			while (i < n)
			{
				yi.Add(yi.Last() + (h / 24) * (55 * functions[0].result(xi[i], yi[i]) -
				                               59 * functions[0].result(xi[i - 1], yi[i - 1]) +
				                               37 * functions[0].result(xi[i - 2], yi[i - 2]) -
				                               9 * functions[0].result(xi[i - 3], yi[i - 3])));
				xi.Add(xi.Last() + h);
				i++;
				dataPoints.Add(new DataPoint(xi.Last(), yi.Last()));
			}

			return dataPoints;
		}

		public List<List<DataPoint>> MethodsAdamsa2()
		{
			
			//if(x0 > xn)
			//{
			//    throw new Exception("Не правильні початкові дані");
			//}
			double h = (xn - x0) / n;

			int i = 0;
			List<List<DataPoint>> dataPoints = new List<List<DataPoint>>();
			dataPoints.Add(new List<DataPoint>());
			dataPoints.Add(new List<DataPoint>());
			

			




			List<double> xi = new List<double>();
			List<List<double>> yi = new List<List<double>>();
			yi.Add(new List<double>());
			yi.Add(new List<double>());
			xi.Add(x0);
			yi[0].Add(y0[0]);
			yi[1].Add(y0[1]);
			dataPoints[0].Add(new DataPoint(xi.Last(), yi[0].Last()));
			dataPoints[1].Add(new DataPoint(xi.Last(), yi[1].Last()));
			xi.Add(xi.Last() + h);
			yi[0].Add(u[0]);
			yi[1].Add(u[1]);
			dataPoints[0].Add(new DataPoint(xi.Last(), yi[0].Last()));
			dataPoints[1].Add(new DataPoint(xi.Last(), yi[1].Last()));
			i = 1;
			while (i < n)
			{
				yi[0].Add(yi[0].Last() + (h / 2) * (3*functions[0].result(xi[i],yi[0][i], yi[1][i])-functions[0].result(xi[i-1], yi[0][i-1], yi[1][i-1])));
				yi[1].Add(yi[1].Last() + (h / 2) * (3 * functions[1].result(xi[i], yi[0][i], yi[1][i]) - functions[1].result(xi[i - 1], yi[0][i - 1], yi[1][i - 1])));
				xi.Add(xi.Last() + h);
				i++;
				dataPoints[0].Add(new DataPoint(xi.Last(), yi[0].Last()));
				dataPoints[1].Add(new DataPoint(xi.Last(), yi[1].Last()));
			}

			return dataPoints;
		}

		public List<DataPoint> MethodsEuler()
		{

			//if(x0 > xn)
			//{
			//    throw new Exception("Не правильні початкові дані");
			//}

			List<DataPoint> dataPoints = new List<DataPoint>();

			dataPoints.Add(new DataPoint(x0, y0[0]));

			double h = (xn - x0) / n;

			double xi = x0;
			double yi = y0[0];
			int i = 0;
			while (i <= n)
			{
				yi = yi + h * functions[0].result(xi, yi);
				xi = xi + h;
				xi = Math.Round(xi, 8);
				dataPoints.Add(new DataPoint(xi, yi));
				i++;
			}

			return dataPoints;
		}

		public List<double> Copy(List<double> list)
		{
			List<double> listCopy = new List<double>();
			foreach (double digit in list)
			{
				listCopy.Add(digit);
			}

			return listCopy;
		}

		public double[] ToArray(double xi, List<double> yi)
		{
			double[] variableValues = new double[N + 1];
			variableValues[0] = xi;
			for (int i = 0; i < N; i++)
			{
				variableValues[i + 1] = yi[i];
			}

			return variableValues;
		}


		public List<List<DataPoint>> MethodsRungeKutta2()
		{

			//if(x0 > xn)
			//{
			//    throw new Exception("Не правильні початкові дані");
			//}

			List<List<DataPoint>> dataPoints = new List<List<DataPoint>>();
			dataPoints.Add(new List<DataPoint>());
			dataPoints.Add(new List<DataPoint>());
			dataPoints[0].Add(new DataPoint(x0, y0[0]));
			dataPoints[1].Add(new DataPoint(x0, y0[1]));

			//dataPoints.Add(new DataPoint(x0, y10));


			double h = (xn - x0) / n;

			double xi = x0;
			double y1i = y0[0];
			double y2i = y0[1];

			double y2i0 = y2i;


			double k1 = 0;
			double k2 = 0;
			double k3 = 0;
			double k4 = 0;

			double l1 = 0;
			double l2 = 0;
			double l3 = 0;
			double l4 = 0;
			int i = 0;
			while (i < n)
			{

				k1 = functions[0].result(xi, y1i, y2i);
				l1 = functions[1].result(xi, y1i, y2i);
				k2 = functions[0].result(xi + h / 2, y1i + h * k1 / 2, y2i + h * l1 / 2);
				l2 = functions[1].result(xi + h / 2, y1i + h * k1 / 2, y2i + h * l1 / 2);
				k3 = functions[0].result(xi + h / 2, y1i + h * k2 / 2, y2i + h * l2 / 2);
				l3 = functions[1].result(xi + h / 2, y1i + h * k2 / 2, y2i + h * l2 / 2);
				k4 = functions[0].result(xi + h, y1i + h * k3, y2i + h * l3);
				l4 = functions[1].result(xi + h, y1i + h * k3, y2i + h * l3);

				y1i = y1i + (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
				y2i = y2i + (h / 6) * (l1 + 2 * l2 + 2 * l3 + l4);
				xi = xi + h;

				xi = Math.Round(xi, 8);

				dataPoints[0].Add(new DataPoint(xi, y1i));
				dataPoints[1].Add(new DataPoint(xi, y2i));
				i++;
			}

			return dataPoints;
		}

		public List<List<DataPoint>> MethodsEuler2()
		{

			//if(x0 > xn)
			//{
			//    throw new Exception("Не правильні початкові дані");
			//}

			List<List<DataPoint>> dataPoints = new List<List<DataPoint>>();
			dataPoints.Add(new List<DataPoint>());
			dataPoints.Add(new List<DataPoint>());
			dataPoints[0].Add(new DataPoint(x0, y0[0]));
			dataPoints[1].Add(new DataPoint(x0, y0[1]));

			double h = (xn - x0) / n;

			double xi = x0;
			double y1i = y0[0];
			double y2i = y0[1];

			double y1i0 = y1i;
			int i = 0;
			while (i < n)
			{
				y1i = y1i + h * functions[0].result(xi, y1i0, y2i);
				y2i = y2i + h * functions[1].result(xi, y1i0, y2i);

				y1i0 = y1i;

				xi = xi + h;
				xi = Math.Round(xi, 8);
				dataPoints[0].Add(new DataPoint(xi, y1i));
				dataPoints[1].Add(new DataPoint(xi, y2i));
				i++;
			}

			return dataPoints;
		}
	}
}