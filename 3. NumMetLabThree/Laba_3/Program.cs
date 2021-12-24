using System;
//using Laba_1;
using System;
using System.Collections.Generic;
using System.Text;


class Laba_3
{
    public static void Main()
    {
        int Row =2;
        int Colum = 2;
        int N = 3;
        double[] B = new double[2] { 2.55, 2.45 };
        double[,] C = new double[2, 2] { { 1.07, 0.995 }, { 0.991, 0.944} };

        Console.WriteLine("A[2,2]:");
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Console.Write($"{C[i, j]}\t");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nB[2]:");

        foreach(double num in B)
        {
            Console.WriteLine($"{num}");
        }

        Console.WriteLine();

        Console.WriteLine("Метод регуляризации:");
        Console.WriteLine("   {0}", String.Join(" ", regul(Row, C, B)));
        Console.WriteLine("Метод Гивенса:");
        Console.WriteLine("   {0}", String.Join(" ", Givens(C, B)));
    }
    static double[] Givens(double[,] A, double[] B)
    {
        int Nn = 2;
        double[] x = new double[Nn];
        double A_0_1 = A[0, 1];
        double M = 0.0;
        double L, R;
        for (int i = 0; i < Nn - 1; i++)
        {
            for (int k = i + 1; k < Nn; k++)
            {
                M = Math.Sqrt(A[i, i] * A[i, i] + A[k, i] * A[k, i]);
                L = A[k, i] / M; //Вычислили A12
                M = A[i, i] / M; //Вычислили B12
                for (int j = 0; j < Nn; j++)
                {
                    R = A[i, j];
                    A[i, j] = M * A[i, j] + L * A[k, j]; // {получили a1j}
                    A[k, j] = M * A[k, j] - L * R; // {получили a2j}
                }
                R = B[i];
                B[i] = M * B[i] + L * B[k];
                B[k] = M * B[k] - L * R;
            }
        }
        x[1] = B[1] / A[1, 1];
        B[0] = 2.53;
        A[0, 0] = 1.05;
        x[0] = (B[0] - A_0_1 * x[1]) / A[0, 0];
        return x;
    }
    public static double[] Gauss(int Row, int Colum, double[] B, double[,] A)
    {
        var RightPart = new double[Row];
        var Answer = new double[Row];
        var Matrix = new double[Row][];
        for (int i = 0; i < Row; i++)
            Matrix[i] = new double[Colum];
        var RowCount = Row;
        var ColumCount = Colum;
        //обнулим массив
        for (int i = 0; i < Row; i++)
        {
            Answer[i] = 0;
            RightPart[i] = 0;
            for (int j = 0; j < Colum; j++)
                Matrix[i][j] = 0;
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
static double[] regul(int n, double[,] a, double[] b)
    {
        double[] result;
        double[,] a1 = new double[n, n];
        double[,] a2 = new double[n, n];
        double[] b1 = new double[n];
        double[] x0 = new double[n];
        double eps = 0.0001;
        double s;
        int k;
        for (int i = 0; i < n; i++)
        {
            for (k = 0; k < n; k++)
            {
                s = 0;
                for (int j = 0; j < n; j++)
                {
                    s += a[j, i] * a[j, k];
                }
                a1[i, k] = s;
            }
        }
        for (int i = 0; i < n; i++)
        {
            s = 0;
            for (int j = 0; j < n; j++)
            {
                s +=  a[j, i] * b[j];
            }
            b1[i] = s;
        }
        double alfa = 0;
        double[] b2 = new double[n];
        b2 = vozm(n, eps, b2);
        double max;
        do
        {
            alfa += 0.00000001;
            a2 = a1;
            for (int i = 0; i < n; i++)
            {
                a2[i, i] = a1[i, i] + alfa;
                b2[i] = b1[i] + alfa * x0[i];
            }
            a1 = a2; 
            b1 = b2;
            
            b2 = Gauss(2,n, b2, a2);
            a2 = a1; 
            result = b2;
            x0 = result;
            b2 = b1;
            b2 = Gauss(2,n, b2, a2); 
            max = Math.Abs(b2[1] - result[1]);
            for (int i = 1; i < n; i++)
            {
                if ((Math.Abs(b2[i] - result[i])) > max)
                {
                    max = Math.Abs(b2[i] - result[i]);
                }
            }
        } while (max >= eps);
        return result;
    }
    static double[] vozm(int n, double eps, double[] b)
    {
        double[] b2 = new double[n];
        for (int i = 0; i < n; i++)
        {
            b2[i] = b2[i] + eps;
        }
        return b2;
    }
}
