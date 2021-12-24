using System;

namespace NumMetLabTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            const int Row = 4;
            const int Col = 4;
            const int N = 4;
            double[] B = new double[N] { 4.0316, 4.3135, 4.2353, 3.7969 };
            double[,] A = new double[Row, Col] { { 5.6000, .0268, .0331, .0393}, { .0147, 4.7, .0271, .0334}, { .0087, .0150, 3.8, .0274 }, { .0028, .009, .0153, 2.9 } };

            Console.WriteLine("B[4]:");
            foreach (double num in B)
            {
                Console.WriteLine($"{num}");
            }

            Console.WriteLine("\nA[4, 4]:");

            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    Console.Write($"{A[i, j]}\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Jacobi:\t\t{0}", String.Join("\t", Jacobi(A, B)));
            Console.WriteLine("Zejdel:\t\t{0}", String.Join("\t", Zejdel(A, B)));
            Console.WriteLine("Relaxation:\t{0}", String.Join("\t", Relaxation(A, B)));
        }

        static double[] Jacobi(double[,] A, double[] B)
        {
            int N = 4;
            double[] X = new double[N];
            X[0] = 1; X[1] = 1; X[2] = 1;
            double[] TempX = new double[N];
            double norm; // норма, определяемая как наибольшая разность компонент столбца иксов соседних итераций.
            do
            {
                for (int i = 0; i < N; i++)
                {
                    TempX[i] = -B[i];
                    for (int g = 0; g < N; g++)
                    {
                        if (i != g)
                            TempX[i] += A[i, g] * X[g];
                    }
                    TempX[i] /= -A[i, i];
                }
                norm = Math.Abs(X[0] - TempX[0]);
                for (int h = 0; h < N; h++)
                {
                    if (Math.Abs(X[h] - TempX[h]) > norm)
                        norm = Math.Abs(X[h] - TempX[h]);
                    X[h] = TempX[h];
                }
            } while (norm > double.Epsilon);
            return X;
        }
        static double[] Zejdel(double[,] A, double[] B)
        {
            int n = 4;
            double[] z = new double[n];
            double[] x = new double[n];
            int i, j;
            for (i = 0; i < n; ++i)
            {
                B[i] /= A[i, i];
                z[i] = A[i, i];
                for (j = 0; j < n; ++j)
                    A[i, j] /= z[i];
            }
            x[1] = x[2] = 0;
            i ^= i;
            do
            {
                x[0] = B[0] - A[0, 1] * x[1] - A[0, 2] * x[0];
                x[1] = B[1] - A[1, 0] * x[0] - A[1, 2] * x[1];
                x[2] = B[2] - A[2, 0] * x[1] - A[2, 1] * x[2];
                x[3] = B[3] - A[3, 0] * x[2] - A[2, 2] * x[3];
            } while ((x[0] == x[1]) & (x[2] == x[0]) & (x[2] == x[1]));
            return x;
        }
        static double[] Relaxation(double[,] A, double[] B)
        {
            int n = 4;
            double[] x0 = new double[n];
            double[] x = new double[n];
            int step = 0;
            double e = 0;
            //Параметр релаксации
            double w = 0.2;
            for (int i = 0; i < n; i++)
            {
                x0[i] = B[i] / A[i, i];
            }
            do
            {
                for (int i = 0; i < n; i++)
                {
                    x[i] = w * B[i] / A[i, i] + (1 - w) * x0[i];//результат с прошлой итерации
                    for (int j = 0; j <= i - 1; j++)
                    {
                        x[i] = x[i] - w * A[i, j] * x[j] / A[i, i];
                    }
                    for (int j = i + 1; j < n; j++)
                    {
                        x[i] = x[i] - w * A[i, j] * x0[j] / A[i, i];
                    }
                }
                e = 0;
                for (int i = 0; i < n; i++)
                {
                    if (Math.Abs(x[i] - x0[i]) > e)
                    {
                        e = Math.Abs(x[i] - x0[i]);
                    }
                    x0[i] = x[i];
                }
                step++;
            }
            while (e >= double.Epsilon);
            return x;
        }
    }
}