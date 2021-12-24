using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabEight
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 30; i++)
            {
                this.dataGridView1.Columns.Add($"Column{i}", $"{i}");
            }
            this.dataGridView1.Rows.Add();
            this.dataGridView1.Rows.Add();
        }
        public delegate double MyDelegate(double x, double y1, double y2);
        double SQRN(double a, double b)
        {
            if (a != 0) return Math.Exp(b * Math.Log(a));
            return 0;
        }
        double f1(double x, double y1, double y2)
        {
            return x / (Math.Sqrt(1 + Math.Pow(x, 2) + Math.Pow(y2, 2)));
        }
        double f2(double x, double y1, double y2)
        {
            return y2 / (Math.Sqrt(1 + Math.Pow(x, 2) + Math.Pow(y1, 2)));
        }
        void eiler(double a, double b, int n, int kolfun, double x, MyDelegate[] f, double[,] y_1)
        {
            double t = (b - a) / n;
            x = x + t;
            for (int i = 1; i < n; i++)
            {
                for (int k = 0; k < kolfun; k++)
                {
                    y_1[k, i] = y_1[k, i - 1] + t * f[k](x, y_1[0, i - 1], y_1[1, i - 1]);
                }
                dataGridView1.Columns[i].HeaderCell.Value = "X = " + x;
                dataGridView1.Rows[0].Cells[i].Value = y_1[0, i];
                dataGridView1.Rows[1].Cells[i].Value = y_1[1, i];
                x += t;
            }
            dataGridView1.Columns[0].HeaderCell.Value = "X = " + a;
            dataGridView1.Rows[0].Cells[0].Value = y_1[0, 1];
            dataGridView1.Rows[1].Cells[0].Value = y_1[1, 2];
        }
        void prognoz(double a, double b, int n, int kolfun, double x, MyDelegate[] f, double[,] y_1)
        {
            double t = (b - a) / n;
            x = x + t;
            for (int i = 1; i < n; i++)
            {
                for (int k = 0; k < kolfun; k++)
                {
                    y_1[k, i] = y_1[k, i - 1] + t * f[k](x + 0.5 * t, y_1[0, i - 1] + 0.5 * t * y_1[0, i - 1], y_1[1, i - 1] + 0.5 * t * y_1[1, i - 1]);
                }
                dataGridView1.Columns[i].HeaderCell.Value = "X = " + x;
                dataGridView1.Rows[0].Cells[i].Value = y_1[0, i];
                dataGridView1.Rows[1].Cells[i].Value = y_1[1, i];
                x += t;
            }
            dataGridView1.Columns[0].HeaderCell.Value = "X = " + a;
            dataGridView1.Rows[0].Cells[0].Value = y_1[0, 1];
            dataGridView1.Rows[1].Cells[0].Value = y_1[1, 2];
        }
        double[,] Runge_Kut(double a, double h, int n, double x, double[,] y_1, MyDelegate[] FMas)
        {
            double[] k = new double[4];
            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    k[0] = FMas[j](x, y_1[0, i - 1], y_1[1, i - 1]);
                    k[1] = FMas[j](x + h / 2, y_1[0, i - 1] + (h * k[1]) / 2, y_1[1, i - 1] + (h * k[1]) / 2);
                    k[2] = FMas[j](x + h / 2, y_1[0, i - 1] + (h * k[2]) / 2 , y_1[1, i - 1] + (h * k[2] / 2));
                    k[3] = FMas[j](x + h, y_1[0, i - 1] + h * k[3], y_1[1, i - 1] + h * k[3]);
                    y_1[j, i] = y_1[j, i - 1] + (h * (k[0] + 2 * k[1] + 2 * k[2] + k[3])) / 6 ;
                }
                x = x + h;
                dataGridView1.Columns[i].HeaderCell.Value = "X = " + x;
                dataGridView1.Rows[0].Cells[i].Value = y_1[0, i];
                dataGridView1.Rows[1].Cells[i].Value = y_1[1, i];
            }
            dataGridView1.Columns[0].HeaderCell.Value = "X = " + a;
            dataGridView1.Rows[0].Cells[0].Value = y_1[0, 1];
            dataGridView1.Rows[1].Cells[0].Value = y_1[1, 2];
            return y_1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double[,] arr = new double[2, 30];
            arr[0, 0] = .2;
            arr[1, 0] = .0;
            MyDelegate[] f = new MyDelegate[] { f1, f2 };
            eiler(-1.0, 1.0, 30, 2, -1.0, f, arr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[,] arr = new double[2, 30];
            arr[0, 0] = .2;
            arr[1, 0] = .0;
            MyDelegate[] f = new MyDelegate[] { f1, f2 };
            prognoz(-1.0, 1.0, 30, 2, -1.0, f, arr);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double[,] arr = new double[2, 30];
            arr[0, 0] = .2;
            arr[1, 0] = .0;
            MyDelegate[] f = new MyDelegate[] { f1, f2 };
            Runge_Kut(-1.0, (2.0/30), 30, -1.0, arr, f);
        }
    }
}
