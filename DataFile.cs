using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МетодЕйлераРунгеКутта
{
    class DataFile
    {
        public static void ReadFile(string nameFile, ref double x0, ref double xn, ref double n, ref List<Function> functionsLobatto, ref List<double> y0, ref string[] variables, ref List<Function> functionsExact, ref int numberEducation)
        {
            string[] str = File.ReadAllLines(nameFile);
            double[] abn = str[0].Split(' ').Select(i => double.Parse(i)).ToArray();
            x0 = abn[0];
            xn = abn[1];
            n = abn[2];

            numberEducation = int.Parse(str[1]);

            variables = new string[numberEducation+1];
            variables[0] = "x";
            for (int i = 1; i <= numberEducation; i++)
            {
                variables[i] = "y" + i;
            }

            functionsLobatto = new List<Function>();
            for (int i = 0; i < numberEducation; i++)
            {
                functionsLobatto.Add(new Function(str[i+2].Split(' ')[0], variables));
            }

            y0 = new List<double>();
            for (int i = 0; i < numberEducation; i++)
            {
                y0.Add(double.Parse(str[i + 2].Split(' ')[1]));
            }

            functionsExact = new List<Function>();
            for (int i = 0; i < numberEducation; i++)
            {
                functionsExact.Add(new Function(str[i +numberEducation+ 2], variables));
                //PlotFunctions.PlotFunction(Graphics, new Function((stackPanels[i].Children[1] as ComboBox).Text, variables), x0, xn, "y" + (i + 1) + "  Exact", Brushes.Blue, 0.01);
            }
        }

        public static void SaveFile(string nameFile, double x0, double xn, double n, List<Function> functionsLobatto, List<double> y0, List<Function> functionsExact, int numberEducation)
        {
            string[] str = new string[2 * numberEducation + 2];
            str[0] = x0 + " " + xn + " " + n;
            str[1] = numberEducation.ToString();
            for(int i=0; i<numberEducation; i++)
            {
                str[i + 2] = functionsLobatto[i].strFunction + " " + y0[i];
            }
            for (int i = 0; i < numberEducation; i++)
            {
                str[numberEducation + i + 2] = functionsExact[i].strFunction;
            }
            File.WriteAllLines(nameFile, str, Encoding.UTF8);
        }
    }
}
