using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabFive
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            matr = new double[4, 4];
            dataGridView1.Rows.Add(.11107E+01, .10590E+2, .79845E+01, -.37547E+01);
            dataGridView1.Rows.Add(.33082E+00, .30775E+01, .23188E+01, -.11183E+01);
            dataGridView1.Rows.Add(-.17127E+00, -.17300E+01, -.13043E+01, .57899E+00);
            dataGridView1.Rows.Add(.29074E+00, .27030E+01, .20379E+01, -.98286E+00);
            for (int col = 0; col < dataGridView1.RowCount - 1; col++)
            {
                for (int row = 0; row < dataGridView1.RowCount - 1; row++)
                {
                    matr[col, row] = Convert.ToDouble(dataGridView1[col, row].Value);
                }
            }
            dataGridView2.Rows.Add();
            dataGridView2.Rows.Add();
            dataGridView2.Rows.Add();
            dataGridView2.Rows.Add();

            dataGridView3.Rows.Add();
            dataGridView3.Rows.Add();
            dataGridView3.Rows.Add();
            dataGridView3.Rows.Add();
        }

        public Form1(DataGridView dataGridView1, DataGridView dataGridView3, DataGridView dataGridView2)
        {
            this.dataGridView1 = dataGridView1;
            this.dataGridView2 = dataGridView2;
            this.dataGridView3 = dataGridView3;
        }

        //проверяет входит ли в массив зна-чение Znach, 
        //используется при вычислении определителя
        bool Vkl(int[] Per, int Znach, int Kol)
        {
            bool result = false;
            for (int i = 0; i <= Kol - 1; i++)
            {
                if (Per[i] == Znach)
                {
                    result = true;
                }
            }
            return result;
        }
        //для определителя указывает знак с каким входит 
        //в сумму очередное слагаемое
        bool Perestanovka(int[] Per, int n)
        {
            int kol = 0;
            for (int i = 0; i <= n - 2; i++)
            {
                for (int j = i + 1; j <= n - 1; j++)
                {
                    if (Per[i] > Per[j])
                    {
                        kol++;
                    }
                }
            }
            if (kol % 2 == 0)
            {
                return false;
            }
            return true;
        }
        //формирует очеред-ное слагаемое в определителе
        double SumMatrToPer(double[,] Matr, int[] Per, int n)
        {
            double result = 1;
            for (int i = 0; i <= n - 1; i++)
            {
                result *= Matr[i, Per[i]];
            }
            if (Perestanovka(Per, n))
            {
                result *= -1;
            }
            return result;
        }
        //рекурсивно формирует перестановки и ищет определитель
        double DetRec(double[,] Matr, int n, int[] Per, int n0)
        {
            double result = 0;
            for (int i = 0; i <= n - 1; i++)
            {
                if (Vkl(Per, i, n0))
                {
                    continue;
                }
                else
                {
                    Per[n0] = i;
                    if (n0 == n - 1)
                    {
                        result = SumMatrToPer(Matr, Per, n);
                    }
                    else
                    {
                        result += DetRec(Matr, n, Per, n0 + 1);
                    }
                }
            }
            return result;
        }
        // подготавливает массив и запускает ре-курсию
        //для нахождения определителя
        double Det(double[,] Matr, int n)
        {
            double result = 0;
            int[] Per = new int[n];
            Per[0] = 1;
            result = DetRec(Matr, n, Per, 0);
            return result;
        }
        //возводит в степень B число A
        double SQRN(double a, int b)
        {
            double result = 0;
            if (a > 0)
            {
                result = Math.Exp(b * Math.Log(a));
            }
            else
            {
                if (a != 0)
                {
                    if (b % 2 != 0)
                    {
                        result = -Math.Exp(b * Math.Log(Math.Abs(a), Math.E));
                    }
                    else
                    {
                        result = Math.Exp(b * Math.Log(Math.Abs(a), Math.E));
                    }
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }
        //перемножение матриц
        double[,] MatrUmn(double[,] a, double[,] b, int n, int m, int k)
        {
            double[,] c = new double[n, k];
            double s;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    s = 0;
                    for (int l = 0; l < m; l++)
                    {
                        s += a[i, l] * b[l, j];
                    }
                    c[i, j] = s;
                }
            }
            return c;
        }
        // считает производную многочлена, переданного в массиве
        double[] Proizv(double[] xar)
        {
            double[] result;
            double[] proizv = new double[xar.Length - 1];
            for (int i = 0; i < xar.Length - 2; i++)
            {
                proizv[i] = xar[i] * (xar.Length - i - 1);
            }
            proizv[xar.Length - 1 - 1] = xar[xar.Length - 1 - 1];
            result = proizv;
            return result;
        }
        // делит многочлен на одночлен (корень), тем самым уменьшая его степень
        double[] Delenie(double[] f, double koren)
        {
            double[] result;
            double[] otv = new double[f.Length];
            otv[0] = f[0];
            for (int i = 1; i < f.Length; i++)
            {
                otv[i] = (koren * otv[i - 1]) + f[i];
            }
            result = otv;
            return result;
        }
        //подставляет число в многочлен
        double Podstanovka(double[] xar, double kor)
        {
            double result = 0;
            for (int i = 0; i < xar.Length - 1; i++)
            {
                result += SQRN(kor, xar.Length - 1 - i) * xar[i];
            }
            result = result + xar[xar.Length - 1];
            return result;
        }
        int High(double[] xar)
        {
            return xar.Length;
        }
        int High(double[,] xar)
        {
            return xar.Length / 2;
        }
        //находит решение многочлена
        double[] Resh(double[] xar)
        {
            double[] result;
            int p;
            double xn, dx, xn1;
            double[] f1, otv = new double[xar.Length - 1];
            p = xar.Length;
            for (int i = 1; i < p; i++)
            {
                xn = 0.00001;
                f1 = (Proizv(xar));
                do
                {
                    dx = -(Podstanovka(xar, xn)) / (Podstanovka(f1, xn));
                    Console.WriteLine("dx = " + dx);
                    xn1 = dx + xn;
                    xn = xn1;
                } while (Math.Abs(dx) > 0.00001);
                //} while ((Math.Abs(dx) > 0.00001) || (Podstanovka(xar, xn1) != 0));
                xar = Delenie(xar, xn1);
                //SetLength(xar, High(xar));
                double[] xar2 = new double[xar.Length - 1];
                for (int l = 0; l < xar2.Length; l++)
                {
                    xar2[l] = xar[l];
                }
                xar = xar2;
                otv[i - 1] = xn1;
                //Заполняем таблицу
                dataGridView2.Rows[i - 1].Cells[0].Value = xn1.ToString();
                //Form1.StringGrid2.Cells[1,i]:=FloatToStrF(xn1,ffExponent,6,13);
            }
            result = otv;
            return result;
        }
        //находит значение очередного не-известного, считая сумму 
        //последующих элементов и деля её на элемент на главной диа-гонали
        double sum(double[,] Matr, double[] Mas, int p)
        {
            double result = 0;
            for (int i = p + 1; i < dataGridView1.ColumnCount; i++)
            {
                result += Matr[p, i] * Mas[i];
            }
            result = -result / Matr[p, p];
            return result;
        }
        bool Perest(double[,] Matr, int p, int i)
        {
            bool result = false;
            double rec;
            for (int u = p + 1; u < dataGridView1.ColumnCount; i++)
            {
                if (Matr[u, i] != 0)
                {
                    for (int l = 0; l < dataGridView1.ColumnCount; l++)
                    {
                        rec = Matr[p, l];
                        Matr[p, l] = Matr[u, l];
                        Matr[u, l] = rec;
                    }
                    result = true;
                    break;
                }
            }
            return result;
        }
        //заменят все элементы в матрице меньше 0.0001 на 0
        double[,] Minim(double[,] Matr)
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (Math.Abs(Matr[i, j]) < 0.0001)
                    {
                        Matr[i, j] = 0;
                    }
                }
            }
            return Matr;
        }
        // делается проверка, если решение до этого было выбрано любое, 
        //а теперь выясняется что оно не подходит, то оно заменяет-ся 0
        void Prov(double[,] Matr, double[] b1, int k, int l)
        {
            for (int i = l + 1; i < dataGridView1.ColumnCount; i++)
            {
                if (Matr[k, i] != 0)
                {
                    b1[i] = 0;
                }
            }
        }
        //приводим матрицу к ступенчатому виду и нахо-дим любое частное решение
        double[] Stup(double[,] Matr)
        {
            double[] b1;
            double b;
            for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
            {
                for (int k = i + 1; k < dataGridView1.ColumnCount; k++)
                {
                    if (Math.Abs(Matr[i, i]) == 0)
                    {
                        if (!Perest(Matr, i, i))
                        {
                            break;
                        }
                    }
                    b = -Matr[k, i] / Matr[i, i];
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        double i_j = Matr[i, j];
                        double k_j = Matr[k, j];
                        //test = Matr[i, j] * b + Matr[k, j];
                        Matr[k, j] = Matr[i, j] * b + Matr[k, j];
                    }
                    Matr = Minim(Matr);
                }
            }
            b1 = new double[dataGridView1.ColumnCount];
            for (int i = dataGridView1.ColumnCount - 1; i >= 0; i--)
            {
                if (Math.Abs(Matr[i, i]) == 0)
                {
                    b1[i] = 1;
                    Prov(Matr, b1, i, i);
                }
                else
                {
                    b1[i] = sum(Matr, b1, i);
                }
            }
            return b1;
        }
        //копируем матрицу
        double[,] Copy1(double[,] Matr)
        {
            double[,] Matr1 = new double[dataGridView1.ColumnCount, dataGridView1.ColumnCount];
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    Matr1[i, j] = Matr[i, j];
                }
            }
            return Matr1;
        }
        //копируем масcив
        double[] CopyMas(double[] Mas)
        {
            double[] result = new double[Mas.Length + 1];
            for (int i = 0; i <= High(Mas); i++)
            {
                result[i] = Mas[i];
            }
            return Mas;
        }
        //очищаем массив
        double[] Clear1(double[] Mas)
        {
            double[] result = new double[Mas.Length + 1];
            for (int i = 0; i < Mas.Length; i++)
            {
                result[i] = 0;
            }
            return result;
        }
        //нормализуем массив
        void OutPut(double[] otv1, int p)
        {
            double s = 0;
            for (int i = 0; i < otv1.Length; i++)
            {
                s += Math.Pow(otv1[i], 2);
            }
            s = Math.Sqrt(s);
            for (int i = 0; i < otv1.Length; i++)
            {
                otv1[i] = otv1[i] / s;
                //Form1.StringGrid3.Cells[p,i+1]:=FloatToStrF(otv1[i],ffExponent,6,13);
                dataGridView3.Rows[i].Cells[p - 1].Value = otv1[i].ToString();
            }
        }
        //находим собственные вектора для соб-ственных значений
        void SobVect(double[,] Matr, double[] otv)
        {
            double[,] Matr1;
            double[] otv1 = new double[dataGridView1.ColumnCount + 1];
            for (int k = 0; k < otv.Length; k++)
            {
                Matr1 = Copy1(Matr);
                for (int i = 0; i < otv.Length; i++)
                {
                    Matr1[i, i] = Matr[i, i] - otv[k];
                }
                Matr1 = Minim(Matr1);
                //otv1 = Clear1(otv1);36
                otv1 = Stup(Matr1);
                OutPut(otv1, k + 1);
            }
        }

        // Лаверрье.
        public void Leverre(double[,] Matr)
        {
            double[,] Matr1;
            double[] s = new double[dataGridView1.ColumnCount], 
                p = new double[dataGridView1.ColumnCount + 1];
            Matr1 = Copy1(Matr);
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = 0;
                if (i != 0)
                {
                    Matr1 = MatrUmn(Matr1, Matr, s.Length, s.Length, s.Length);
                }
                for (int j = 0; j < s.Length; j++)
                {
                    s[i] = s[i] + Matr1[j, j];
                }
            }
            for (int i = 0; i < s.Length; i++)
            {
                p[i + 1] = s[i];
                for (int j = 0; j <= i - 1; j++)
                {
                    p[i + 1] = p[i + 1] + p[j + 1] * s[i - j - 1];
                }
                p[i + 1] *= (-1.0 / (i + 1));
                //Console.WriteLine("-1/(i+1) = " + (-1 / (i + 1)));
            }
            p[0] = 1;
            p = Resh(p);
            SobVect(Matr, p);
        }

        // Фадеев.
        public void Fadeev(double[,] Matr)
        {
            double[,] Matr1, Matr2;
            double[] s = new double[dataGridView1.ColumnCount], p = new
           double[dataGridView3.ColumnCount + 1];
            Matr1 = Copy1(Matr);
            Matr2 = Copy1(Matr);
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                s[i] = 0;
                for (int j = 0; j < s.Length; j++)
                {
                    s[i] = s[i] + Matr2[j, j];
                }
                s[i] = s[i] / (i + 1);
                Matr1 = Copy1(Matr2);
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    Matr1[j, j] = Matr2[j, j] - s[i];
                }
                Matr2 = MatrUmn(Matr, Matr1, dataGridView1.ColumnCount, dataGridView1.ColumnCount, dataGridView1.ColumnCount);
            }
            for (int i = 0; i < s.Length; i++)
            {
                p[i + 1] = -s[i];
            }
            p[0] = 1;
            p = Resh(p);
            SobVect(Matr, p);
        }
        // находим произведение матрицы на массив
        double[] MatrUmnMas(double[,] a, double[] b, int n, int m, int k)
        {
            double s;
            double[] c = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    s = 0;
                    for (int l = 0; l < m; l++)
                    {
                        s += a[i, l] * b[l];
                    }
                    c[i] = s;
                }
            }
            return c;
        }
        //копируем и транспонируем матрицу, удаляя последнюю строчку
        double[,] CopyTrans(double[,] a)
        {
            double[,] result = new double[dataGridView1.ColumnCount,
           dataGridView1.ColumnCount];
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    result[i, j] = a[j + 1, i];
                }
            }
            return result;
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
        // Крылов.
        public void Krilov(double[,] Matr)
        {
            double[] p = new double[dataGridView1.ColumnCount + 1];
            int l = 1;
            double[,] a = new double[dataGridView1.ColumnCount - 1, dataGridView1.ColumnCount - 1],
            y = new double[dataGridView3.ColumnCount + 1, dataGridView1.ColumnCount];
            double[] b = new double[dataGridView1.ColumnCount + 1];
            do
            {
                a = Copy1(Matr);
                Random rand = new Random();
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    do
                    {
                        y[dataGridView1.ColumnCount, j] = Math.Truncate(l * 10 * Convert.ToDouble(rand.Next(100)) / 100);
                    } while (y[dataGridView1.ColumnCount, j] < 5);
                }
                l++;
                double[] temp = new double[dataGridView1.ColumnCount];
                for (int i = 1; i <= dataGridView1.ColumnCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        temp[j] = y[dataGridView1.ColumnCount - i + 1, j];
                    }
                    temp = MatrUmnMas(a, temp, dataGridView1.ColumnCount, dataGridView1.ColumnCount, dataGridView1.ColumnCount);
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        y[dataGridView3.ColumnCount - i, j] = temp[j];
                    }
                }

                for (int j = 0; j < dataGridView3.ColumnCount; j++)
                {
                    b[j] = y[0, j];
                }
                a = CopyTrans(y);
            } while (Det(a, dataGridView1.ColumnCount) == 0);

            double[,] a2 = new double[dataGridView1.ColumnCount, dataGridView1.ColumnCount + 1];
            for (int k = 0; k < dataGridView1.ColumnCount; k++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    a2[k, j] = a[k, j];
                }
            }
            a = a2;
            b = Gauss(b.Length - 1, b.Length - 1, b, a);
            for (int i = 1; i < p.Length; i++)
            {
                p[i] = -1.0 * b[i - 1];
            }
            p[0] = 1;
            p = Resh(p);
            SobVect(Matr, p);
        }
        double[,] SetLength(double[,] ar, int n, int k)
        {
            double[,] new_ar = new double[n, k];
            for (int i = 0; i <= n - 1; i++)
            {
                for (int j = 0; j <= k - 1; j++)
                {
                    new_ar[i, j] = ar[i, j];
                }
            }
            return new_ar;
        }
        double[] SetLength(double[] ar, int n)
        {
            double[] new_ar = new double[n];
            for (int i = 0; i <= ar.Length; i++)
            {
                new_ar[i] = ar[i];
            }
            return new_ar;
        }

        public double[,] matr;

        private void button1_Click(object sender, EventArgs e) // Leverre
        {
            Leverre(matr);
        }

        private void button2_Click(object sender, EventArgs e) // Fadeev
        {
            Fadeev(matr);
        }

        private void button3_Click(object sender, EventArgs e) // Krilov
        {
            Krilov(matr);
        }
    }
}
