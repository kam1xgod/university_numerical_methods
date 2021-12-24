using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public double[] Gauss(int Row, int Colum, double[] B, double[,] A)
        {
            var RightPart = new double[Row];
            var Answer = new double[Row];
            var Matrix = new double[Row][];
            for (int i = 0; i < Row; i++)
            {
                Matrix[i] = new double[Colum];
            }
            var RowCount = Row;
            var ColumCount = Colum;
            //обнулим массив
            for (int i = 0; i < Row; i++)
            {
                Answer[i] = 0;
                RightPart[i] = 0;
                for (int j = 0; j < Colum; j++)
                {
                    Matrix[i][j] = 0;
                }
            }
            var ReturnVal = new double[Row];
            //заполняем правую часть
            for (int i = 0; i < Row; i++)
            {
                RightPart[i] = B[i];
            }
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Row; j++)
                {
                    Matrix[j][i] = A[j, i];
                }
            }
            for (int i = 0; i < RowCount - 1; i++)
            {
                // SortRows(i);
                for (int j = i + 1; j < RowCount; j++)
                {
                    if (Matrix[i][i] != 0) //если главный элемент не 0, то производим вычисления
                    {
                        double MultElement = Matrix[j][i] / Matrix[i][i];
                        for (int k = i; k < ColumCount; k++)
                        {
                            Matrix[j][k] -= Matrix[i][k] * MultElement;
                        }
                        RightPart[j] -= RightPart[i] * MultElement;
                    }
                    //для нулевого главного элемента просто пропускаем данный шаг
                }
            }
            //ищем решение
            for (int i = (int)(RowCount - 1); i >= 0; i--)
            {
                Answer[i] = RightPart[i];
                for (int j = (int)(RowCount - 1); j > i; j--)
                {
                    Answer[i] -= Matrix[i][j] * Answer[j];
                }
                Answer[i] /= Matrix[i][i];
            }
            return Answer;
        }

        double Norma(double[,] a, int n)
        {
            double res = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    res += a[i, j] * a[i, j];
                }
            }
            res = Math.Sqrt(res);
            return res;
        }
        double func(double[] x, int i)
        {
            double res = 0;
            switch (i)
            {
                case 0:
                    res = 0.8 - Math.Sin(x[1] + 1);
                    break;
                case 1:
                    res = 1.3 - Math.Sin(x[0] - 1);
                    break;
            }
            return res;
        }
        double MatrJacobi(double[] x, int i, int j)
        {
            double res = 0; switch (i)
            {
                case 0:
                    switch (j)
                    {
                        case 0:
                            res = Math.Cos(x[0] + 1);
                            break;
                        case 1:
                            res = 0;
                            break;
                    }
                    break;
                case 1:
                    switch (j)
                    {
                        case 0:
                            res = 0;
                            break;
                        case 1:
                            res = Math.Cos(x[1] + 1);
                            break;
                    }
                    break;
            }
            return res;
        }

        void vivod_vectr(double[] vect)
        {
            int n = 2;
            for (int i = 0; i < n; i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = vect[i];
            }
        }

        public void simpte_iter()
        {
            int n = 2;
            int iter = 0;
            double[] x0 = new double[n];
            double[] x = new double[n];
            double[,] a = new double[n, n];
            x0[0] = 0;
            x0[1] = 0;
            double max, eps = 1e-4;
            do
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        a[i, j] = MatrJacobi(x0, i, j);
                    }
                }
                vivod_vectr(x0);
                //Console.WriteLine("Norma = " + Norma(a, n));
                //Console.WriteLine("Nomer iterazii = " + iter);
                //Console.WriteLine("=============");
                for (int i = 0; i < n; i++)
                {
                    x[i] = func(x0, i);

                }
                max = Math.Abs(x[0] - x0[0]);
                for (int i = 1; i < n; i++)
                {
                    if (Math.Abs(x[i] - x0[i]) > max)
                    {
                        max = Math.Abs(x[i] - x0[i]);
                    }
                }
                x0 = x; iter++;
                //Console.ReadLine();
            } while ((max > eps) || (iter < 20));
            //Console.ReadLine();
        }

        double jacobian(double[] x, int i, int j)
        {
            double res = 0;
            switch (i)
            {
                case 0:
                    switch (j)
                    {
                        case 0:
                            res = 1;
                            break;
                        case 1:
                            res = Math.Cos(x[1] - 1);
                            break;
                    }
                    break;
                case 1:
                    switch (j)
                    {
                        case 0:
                            res = -Math.Cos(x[0] + 1);
                            break;
                        case 1:
                            res = 1;
                            break;
                    }
                    break;
            }
            return res;
        }
        double func2(double[] x, int i)
        {
            double res = 0;
            switch (i)
            {
                case 0:
                    res = Math.Sin(x[1] - 1) + x[0] - 1.3;
                    break;
                case 1:
                    res = x[1] - Math.Sin(x[0] + 1) - 0.8;
                    break;
            }
            return res;
        }

        public void Nyuton()
        {
            int n = 2;
            int iter = 0;
            double[] dx = new double[n];
            double[] x = new double[n];
            double[] f = new double[n];
            double[,] a = new double[n, n];
            x[0] = 0; x[1] = 0;
            double max, eps = 0.0001;
            do
            {
                //Console.WriteLine("Nomer iterazii = " + iter);
                //Console.WriteLine("=============");
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        a[i, j] = jacobian(x, i, j);
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    f[i] = (-1) * func2(x, i);
                }
                dx = Gauss(n, n, f, a);
                max = Math.Abs(dx[0]);
                for (int i = 1; i < n; i++)
                {
                    if (Math.Abs(dx[i]) > max)
                    {
                        max = Math.Abs(dx[i]);
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    x[i] = x[i] + dx[i];
                }
                iter++;
                //Console.ReadLine();
            }
            while (max > eps & iter < 5);
            vivod_vectr(x);
            //Console.ReadLine();
        }


        void button1_Click(object sender, EventArgs e)
        {
            Nyuton();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            simpte_iter();
        }
    }
}