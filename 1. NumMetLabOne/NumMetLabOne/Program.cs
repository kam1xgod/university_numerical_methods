using System;

namespace NumMetLabOne
{
    class Program
    {
        public static void Main()
        {

            double[] B = new double[3] { -5.13, -3.69, -5.98 };
            double[,] A = new double[3, 3] { { 1.53, 1.61, 1.43 }, { 2.35, 2.31, 2.07 }, { 3.83, 3.73, 3.45 } };

            Console.WriteLine("B[3]:\n");
            foreach(double num in B)
            {
                Console.WriteLine(num);
            }

            Console.WriteLine("\nA[3, 3]:\n");

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    Console.Write(A[i, j] + "\t");
                }
                Console.WriteLine("");
            }

            Console.WriteLine("");
            Console.WriteLine("Gauss:   {0}", String.Join(" ", Gauss(3, 3, B, A)));
            Console.WriteLine("Cholesky   {0}", String.Join(" ", Holetskiy(3, A, B)));
        }
        static double[] Gauss(int Row, int Colum, double[] B, double[,] A)
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
                for (int j = i + 1; j < RowCount; j++)
                {
                    if (Matrix[i][i] != 0) //если главный элемент не 0, то производим вычисления
                    {
                        double MultElement = Matrix[j][i] / Matrix[i][i];
                        for (int k = i; k < ColumCount; k++)
                            Matrix[j][k] -= Matrix[i][k] * MultElement;
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
                    Answer[i] -= Matrix[i][j] * Answer[j];
                Answer[i] /= Matrix[i][i];
            }
            return Answer;
        }

        static double[] Holetskiy(int N, double[,] A1, double[] B1)
        {
            double summ;
            double[,] c = new double[N, N + 1], L = new double[N, N + 1];
            double[] y = new double[N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N + 1; j++)
                {
                    c[i, j] = 0;
                    L[i, j] = 0;
                    y[i] = 0;
                }
            }
            //Умножение матрицы на транспонированную
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    summ = 0.0;
                    for (int t = 0; t < N; t++)
                    {
                        summ = A1[t, j] * A1[t, i] + summ;
                    }
                    c[i, j] = summ;

                }
            }
            //{умножение правой части на транспонированную м-цу}
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    y[i] = A1[j, i] * B1[j] + y[i];
                }
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    A1[i, j] = c[i, j];
                    B1[i] = y[i];
                }
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    summ = 0;
                    for (int t = 0; t <= j - 1; t++)
                    {
                        summ = summ + L[i, t] * L[j, t];
                    }
                    if (i != j)
                    {
                        L[i, j] = (A1[i, j] - summ) / L[j, j];
                    }
                    else
                    {
                        L[i, i] = Math.Sqrt(A1[i, i] - summ);
                    }
                }
            }
            for (int i = 0; i < N; i++)
            {
                L[i, N] = B1[i];
            }
            B1[0] = L[0, N] / L[0, 0];
            for (int i = 1; i < N; i++)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    L[i, N] = L[i, N] - L[i, j] * B1[j];
                }
                B1[i] = L[i, N] / L[i, i];
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = i + 1; j < N; j++)
                {
                    L[i, j] = L[j, i];
                    L[j, i] = 0;
                }
                L[i, N] = B1[i];
            }
            B1[N - 1] = L[N - 1, N] / L[N - 1, N - 1];
            for (int i = N - 1 - 1; i >= 0; i--)
            {
                for (int j = i + 1; j < N; j++)
                {
                    L[i, N] = L[i, N] - L[i, j] * B1[j];
                }
                B1[i] = L[i, N] / L[i, i];
            }
            return B1;
        }
    }
}
