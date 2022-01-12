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
    public class Function
    {
        public Function(string str, params string [] variables)
        {
            operators = new List<string>(standart_operators);
            operators.AddRange(prefix_operators);
            this.strFunction = str;
            this.variables = new List<string>(variables);
            PostfixNotation = ConvertToPostfixNotation(strFunction);
        }

        public string strFunction;

        private List<string> operators;

        private List<string> standart_operators =
            new List<string>(new string[] { "(", ")", "+", "-", "*", "/", "^" });

        private List<string> prefix_operators =
            new List<string>(new string[] { "cos", "sin", "tg", "ctg", "ln", "log10", "exp" });

        string[] PostfixNotation;

        private List<string> variables;

        private List<string> constants = new List<string>(new string[] { "e", "pi" });
        private List<double> constantValues = new List<double>(new double[] { Math.E, Math.PI });


        private IEnumerable<string> Separate(string input)
        {
            int pos = 0;
            while (pos < input.Length)
            {
                string s = string.Empty + input[pos];
                if (!standart_operators.Contains(input[pos].ToString()))
                {
                    if (Char.IsDigit(input[pos]))
                        for (int i = pos + 1; i < input.Length &&
                            (Char.IsDigit(input[i]) || input[i] == ',' || input[i] == '.'); i++)
                            s += input[i];
                    else if (Char.IsLetter(input[pos]))
                        for (int i = pos + 1; i < input.Length &&
                            (Char.IsLetter(input[i]) || Char.IsDigit(input[i])); i++)
                            s += input[i];
                }
                yield return s;
                pos += s.Length;
            }
        }
        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                    return 3;
                default:
                    return 4;
            }
        }

        public string[] ConvertToPostfixNotation(string input)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (string c in Separate(input))
            {
                if (operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (prefix_operators.Contains(c))
                        {
                            stack.Push(c);
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else if (c.Equals(" "))
                {

                }
                else
                    outputSeparated.Add(c);
            }
            if (stack.Count > 0)
                foreach (string c in stack)
                    outputSeparated.Add(c);

            return outputSeparated.ToArray();
        }

	    public double result(params double[] variableValues)
	    {

		    
			    Stack<string> stack = new Stack<string>();

			    Queue<string> queue = new Queue<string>(PostfixNotation);
		    try
		    {
			    string str = queue.Dequeue();
			    while (queue.Count >= 0)
			    {
				    if (!operators.Contains(str))
				    {

					    if (constants.Contains(str))
					    {
						    str = constantValues[constants.IndexOf(str)].ToString();
					    }
					    else if (variables.Contains(str))
					    {
						    str = variableValues[variables.IndexOf(str)].ToString();
					    }
					    else if (!double.TryParse(str, out double digit))
					    {
						    throw new Exception("Error enter data");
					    }

					    stack.Push(str);
				    }

				    else
				    {
					    double summ = 0;


					    switch (str)
					    {

						    case "+":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    double b = Convert.ToDouble(stack.Pop());
							    summ = a + b;
							    break;
						    }
						    case "-":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    double b = Convert.ToDouble(stack.Pop());
							    summ = b - a;
							    break;
						    }
						    case "*":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    double b = Convert.ToDouble(stack.Pop());
							    summ = b * a;
							    break;
						    }
						    case "/":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    double b = Convert.ToDouble(stack.Pop());
							    summ = b / a;
							    break;
						    }
						    case "^":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    double b = Convert.ToDouble(stack.Pop());
							    summ = Convert.ToDouble(Math.Pow(Convert.ToDouble(b), Convert.ToDouble(a)));
							    break;
						    }
						    case "sin":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    summ = Convert.ToDouble(Math.Sin(Convert.ToDouble(a)));
							    break;
						    }
						    case "cos":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    summ = Convert.ToDouble(Math.Cos(Convert.ToDouble(a)));
							    break;
						    }
						    case "tg":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    summ = Convert.ToDouble(Math.Tan(Convert.ToDouble(a)));
							    break;
						    }
						    case "ctg":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    summ = Convert.ToDouble(1 / Math.Tan(Convert.ToDouble(a)));
							    break;
						    }
						    case "ln":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    summ = Convert.ToDouble(Math.Log(Convert.ToDouble(a)));
							    break;
						    }
						    case "log10":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    summ = Convert.ToDouble(Math.Log10(Convert.ToDouble(a)));
							    break;
						    }
						    case "exp":
						    {
							    double a = Convert.ToDouble(stack.Pop());
							    summ = Convert.ToDouble(Math.Exp(Convert.ToDouble(a)));
							    break;
						    }
						    default:
						    {
							    throw new Exception("Error enter data");

						    }
					    }


					    stack.Push(summ.ToString());

				    }

				    if (queue.Count > 0)
					    str = queue.Dequeue();
				    else
					    break;
			    }
		    }
		    catch (Exception ex)
		    {
			    //MessageBox.Show(ex.Message);
			    return 0;
		    }

		    return Convert.ToDouble(stack.Pop());
	    }
    }
}
