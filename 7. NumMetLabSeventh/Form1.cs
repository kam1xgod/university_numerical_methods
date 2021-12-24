using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Leto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("Y", "Y");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            double[] y = new double[dataGridView1.RowCount - 1];
            double[] x = new double[dataGridView1.RowCount - 1];
            for (int i =0;i<dataGridView1.RowCount-1;i++)
            {
                x[i] = i+1;
                y[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            double Polinom = 0;
            for (int i = 0; i < y.Length; i++)
            {
                double p = 1;
                for (int j = 0; j < y.Length; j++)
                    if (i != j)
                    {
                        p = p * (Convert.ToDouble(textBox1.Text) - x[j]) / (x[i] - x[j]);
                    }
                Polinom += y[i] * p;
            }
            for (int i = 0; i < y.Length; i++)
            {
                chart1.Series[0].Points.AddXY(x[i], y[i]);
            }
            chart1.Series[0].Points.AddXY(y.Length, Polinom);
            textBox2.Text = Polinom.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            double[] y = new double[dataGridView1.RowCount-1];
            double[] x = new double[dataGridView1.RowCount-1];
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                x[i] =i+1; 
                y[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            BuildSpline(x, y, y.Length);
            for (int i = 0; i < y.Length; i++)
            {
                chart1.Series[0].Points.AddXY(x[i], y[i]);
            }
            double result = Interpolate(Double.Parse(textBox1.Text));
            chart1.Series[0].Points.AddXY(y.Length, result);
            textBox2.Text = result.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            double[] y = new double[dataGridView1.RowCount - 1];
            double[] x = new double[dataGridView1.RowCount - 1];
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                x[i] = i+1;
                y[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            double result, s, p;
            double[,] m = new double[y.Length * 2, y.Length * 2];
            for (int i = 1; i < y.Length; i++)
            {
                m[0, i - 1] = y[i] - y[i - 1];
            }
            for (int i = 1; i < y.Length - 1; i++)
            {
                int f = 1;
                for (int k = i + 1; k >= 1; k--)
                {
                    f *= k;
                }
                for (int j = 1; j < y.Length - i; j++)
                {
                    m[i, j - 1] = (m[i - 1, j] - m[i - 1, j - 1]) / f;
                }
            }
            if ((x[y.Length - 1] - Convert.ToDouble(textBox1.Text)) - (Convert.ToDouble(textBox1.Text) - x[1]) < 0)
            {
                s = y[y.Length - 1];
                for (int i = 0; i < y.Length - 1; i++)
                {
                    p = 1;
                for(int j = 0; j <= i; j++)
                    {
                        p = p * (Convert.ToDouble(textBox1.Text)- (y.Length- j));

                    }
                    s = s + p * m[i, y.Length-i-2];

                }
                result = s;

            }
            else

            {
                s = y[0];
                for (int i = 0; i < y.Length - 1; i++)
                {
                    p = 1;
                    for
                    (int j = 0; j <= i; j++)

                    {
                        p = p * (Convert.ToDouble(textBox1.Text)- x[j]);

                    }
                    s = s + p * m[i, 0];

                }
                result = s;

            }
            for (int i = 0; i < y.Length; i++)
            {
                chart1.Series[0].Points.AddXY(x[i], y[i]);
            }
            chart1.Series[0].Points.AddXY(y.Length, result);
            textBox2.Text = result.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           chart1.Series[0].Points.Clear();
            double[] y = new double[dataGridView1.RowCount - 1];
            double[] x = new double[dataGridView1.RowCount - 1];
            for (int i =0;i<dataGridView1.RowCount-1;i++)
            {
                x[i] = i+1;
                y[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[0].Value.ToString());
            }
            double result = 0.0;
            double q = Convert.ToDouble(textBox1.Text);
            double[,] a = new double[3,3];
            double[] b = new double[3], c = new double[3];
            int n = y.Length;
            a[0,0]=n;
            for(int i=0;i<n;i++)
            {
                a[0, 1] += x[i];
                a[1, 0] += x[i];
                a[0, 2] += x[i] * x[i];
                a[1, 1] += x[i] * x[i];
                a[2, 0] += x[i] * x[i];
                a[1, 2] += x[i] * x[i] * x[i];
                a[2, 1] += x[i] * x[i] * x[i];
                a[2, 2] += x[i] * x[i] * x[i] * x[i];
                b[0] += y[i];
                b[1] += y[i] * x[i];
                b[2] += y[i] * x[i] * x[i];
            }
            b = gaus(a,b);
            result = b[0]+b[1]*q+b[2]*q*q;
            for (int i = 0; i < y.Length; i++)
            {
                chart1.Series[0].Points.AddXY(x[i], y[i]);
            }
            chart1.Series[0].Points.AddXY(y.Length, result);
            textBox2.Text = result.ToString();
        }
        public double[] gaus(double[,] A, double[] B)
        {
            double[] X = new double[3];
            for(int i =0;i<B.Length;i++)
            {
                sort_rows(i,A,B);
                for(int j=i+1;j<B.Length;j++)
                {
                    if (A[i,i]!= 0)
                    {
                        double mult_element = A[j,i]/A[i,i];
                        for(int k=i;k<B.Length;k++)
                        {
                            A[j,k]-=(A[i,k]*mult_element);
                        }
                        B[j]-=(B[i]*mult_element);
                    }
                }
            }
            for(int k = 2;k>0;k--)
            {
                X[k] = B[k];
                for(int j=2;j>k;j--)
                {
                    X[k] -= A[k, j] * X[j];
                }
                X[k] /= A[k, k];
            }
            return X;
        }
        public void sort_rows(int sort_index, double[,] A, double[] B)
        {
            double max_element = A[sort_index,sort_index];
            int max_element_index = sort_index;
            for (int i = sort_index+1; i<B.Length;i++)
            {
                if (A[i,sort_index]>max_element)
                {
                    max_element = A[i,sort_index];
                    max_element_index = i;
                }
            }
            if (max_element_index > sort_index)
            {
                double temp = B[max_element_index];
                B[max_element_index] = B[sort_index];
                B[sort_index]=temp;
                for(int i =0;i<B.Length;i++)
                {
                    temp=A[max_element_index,i];
                    A[max_element_index,i]=A[sort_index,i];
                    A[sort_index,i]=temp;
                }
            }
        }
        SplineTuple[] splines;

        private struct SplineTuple
        {
            public double a, b, c, d, x;
        }

        
        public void BuildSpline(double[] x, double[] y, int n)
        {
            // Инициализация массива сплайнов
            splines = new SplineTuple[n];
            for (int i = 0; i < n; ++i)
            {
                splines[i].x = x[i];
                splines[i].a = y[i];
            }
            splines[0].c = splines[n - 1].c = 0.0;

            // Решение СЛАУ относительно коэффициентов сплайнов c[i] методом прогонки для трехдиагональных матриц
            // Вычисление прогоночных коэффициентов - прямой ход метода прогонки
            double[] alpha = new double[n - 1];
            double[] beta = new double[n - 1];
            alpha[0] = beta[0] = 0.0;
            for (int i = 1; i < n - 1; ++i)
            {
                double hi = x[i] - x[i - 1];
                double hi1 = x[i + 1] - x[i];
                double A = hi;
                double C = 2.0 * (hi + hi1);
                double B = hi1;
                double F = 6.0 * ((y[i + 1] - y[i]) / hi1 - (y[i] - y[i - 1]) / hi);
                double z = (A * alpha[i - 1] + C);
                alpha[i] = -B / z;
                beta[i] = (F - A * beta[i - 1]) / z;
            }

            // Нахождение решения - обратный ход метода прогонки
            for (int i = n - 2; i > 0; --i)
            {
                splines[i].c = alpha[i] * splines[i + 1].c + beta[i];
            }

            // По известным коэффициентам c[i] находим значения b[i] и d[i]
            for (int i = n - 1; i > 0; --i)
            {
                double hi = x[i] - x[i - 1];
                splines[i].d = (splines[i].c - splines[i - 1].c) / hi;
                splines[i].b = hi * (2.0 * splines[i].c + splines[i - 1].c) / 6.0 + (y[i] - y[i - 1]) / hi;
            }
        }

        // Вычисление значения интерполированной функции в произвольной точке
        public double Interpolate(double x)
        {
            if (splines == null)
            {
                return double.NaN; // Если сплайны ещё не построены - возвращаем NaN
            }

            int n = splines.Length;
            SplineTuple s;

            if (x <= splines[0].x) // Если x меньше точки сетки x[0] - пользуемся первым эл-тов массива
            {
                s = splines[0];
            }
            else if (x >= splines[n - 1].x) // Если x больше точки сетки x[n - 1] - пользуемся последним эл-том массива
            {
                s = splines[n - 1];
            }
            else // Иначе x лежит между граничными точками сетки - производим бинарный поиск нужного эл-та массива
            {
                int i = 0;
                int j = n - 1;
                while (i + 1 < j)
                {
                    int k = i + (j - i) / 2;
                    if (x <= splines[k].x)
                    {
                        j = k;
                    }
                    else
                    {
                        i = k;
                    }
                }
                s = splines[j];
            }

            double dx = x - s.x;
            // Вычисляем значение сплайна в заданной точке по схеме Горнера (в принципе, "умный" компилятор применил бы схему Горнера сам, но ведь не все так умны, как кажутся)
            return s.a + (s.b + (s.c / 2.0 + s.d * dx / 6.0) * dx) * dx;
        }
    }
}
