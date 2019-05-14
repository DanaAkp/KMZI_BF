using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KMZI_spectr
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string AlphabetENG = "abcdefghijklmnopqrstuvwxyz";
        int NumberOfVariables;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnFurie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int[] vec = GetVector(tbFunc.Text.Trim());
                if (!CheckVector(vec)) throw new Exception("Ошибка");
                tblRes.Text = "Вес равен " + weight(vec)+"\nСпектр Фурье: \n";
                int[] Furie = spectrF(vec);
                int[] WA = spectrWA(vec);

                for (int i = 0; i < Furie.Length; i++)
                    tblRes.Text += Furie[i] + " ";
                tblRes.Text += "\nСпектр Уолша-Адамара: \n";
                for (int i = 0; i < Furie.Length; i++)
                    tblRes.Text += WA[i] + " ";
                tblRes.Text += "\nНелинейность равна " + Nf(WA);
                tblRes.Text += "\nАНФ: " + ANF(vec);
                KorrIm(vec);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private int Nf(int[] WA)
        {
            int max =Math.Abs( WA[0]);
            foreach (int x in WA)
                if (max < Math.Abs(x)) max = Math.Abs(x);
            return (int)Math.Pow(2, NumberOfVariables - 1) - max / 2;
        }
        private int[] spectrF(int[] vec)
        {
            int[] spectr = new int[vec.Length];
            for (int i = 0; i < vec.Length; i++)
            {
                int[] u = getBinaryCode(i);
                for (int j = 0; j < vec.Length; j++)
                {
                    int[] x = getBinaryCode(j);
                    int buf = 0;
                    for (int k = 0; k < x.Length; k++)
                    {
                        if (x[k] == 1 && u[k] == 1) buf++;
                    }
                    buf %= 2;
                    buf = (int)Math.Pow(-1, buf);
                    buf *= vec[j];
                    spectr[i] += buf;
                }
            }
            return spectr;
        }
        private int[] spectrWA(int[] vec)
        {
            int[] spectr = new int[vec.Length];
            for (int i = 0; i < vec.Length; i++)
            {
                int[] u = getBinaryCode(i);
                for (int j = 0; j < vec.Length; j++)
                {
                    int[] x = getBinaryCode(j);
                    int buf = 0;
                    for (int k = 0; k < x.Length; k++)
                    {
                        if (x[k] == 1 && u[k] == 1) buf++;
                    }
                    buf += vec[j];
                    buf %= 2;
                    buf = (int)Math.Pow(-1, buf);
                    spectr[i] += buf;
                }
            }
            return spectr;
        }
        private int[] getBinaryCode(int j)
        {
            int[] num=new int[NumberOfVariables];
            string Binarycode = Convert.ToString(j, 2);

            for (int i = Binarycode.Length-1; i >=0; i--)
                num[Binarycode.Length -1- i] = int.Parse(Binarycode[i].ToString());
            
                Array.Reverse(num);
            return num;
        }
        private int weight(int[] vec)
        {
            int c = 0;
            foreach (int x in vec) if (x == 1) c++;
            return c;
        }
        private int[] GetVector(string s)
        {
            int[] vec = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
                vec[i] = int.Parse(s[i].ToString());
            return vec;
        }
        private bool CheckVector(int[] vec)
        {
            for (int i = 0; i < vec.Length; i++)
            {
                if (vec[i] != 1 && vec[i] != 0)
                    return false;
            }
            for (int i = 0; i < vec.Length; i++)
            {
                if (vec.Length == Math.Pow(2, i))
                {
                    NumberOfVariables = i;
                    return true;
                }
            }
            return false;
        }
        private string ANF(int[] vec)
        {
            string s = "";
            int deg = 0;
            int[] res = new int[vec.Length];
            int[] buf = vec;
            int k = vec.Length;
            int j = 0;
            while (k != 0)
            {
                res[j] = buf[0];
                for (int i = 0; i < k - 1; i++)
                {
                    buf[i] = buf[i] + buf[i + 1];
                    buf[i] %= 2;
                }
                k--; j++;
            }
            if (res[0] == 1)
                s += "1 +";
            for (int i = 1; i < vec.Length; i++)
            {
                bool check = false;
                int c = 0;
                if (res[i] == 1)
                {
                    string binaryCode = Convert.ToString(i, 2);
                    string num;
                    if (binaryCode.Length != NumberOfVariables)
                    {
                        num = new string('0', NumberOfVariables - binaryCode.Length);
                        num += binaryCode;
                    }
                    else num = binaryCode;
                    for (j = 0; j < num.Length; j++)
                    {
                        if (num[j] == '1')
                        {
                            s += AlphabetENG[j].ToString();
                            check = true;
                            c++;
                        }
                    }
                }
                if (c > deg) deg = c; 
                if (check)
                    s += " +";
            }
            string t = "";
            for (int i = 0; i < s.Length-1; i++) t += s[i];
            t += "\nСтепень равна " + deg;
            if (deg == NumberOfVariables) t += "\nФункция обладает максимальной алгебраической степенью";
            else t += "\nФункция не обладает максимальной алгебраической степенью";
            return t;
        }
        private int KorrIm(int[] vec)
        {
            int[] polin = polinom(vec);
            int m = 0;


            for (int i = 1; i < vec.Length - 1; i++)
            {
                int[] f = getBinaryCode(i);//массив с фиксированными переменными, 1 -фикс
                int[] res = new int[vec.Length];
                for (int j = 0; j < vec.Length; j++)//идет по наборам вектора значений
                {
                    int[] nabor = getBinaryCode(j);//набор значений всех переменных

                    for (int l = 1; l < vec.Length; l++)//идет по массиву многочлена
                    {
                        if (polin[l] == 1)
                        {
                            int[] x = getBinaryCode(l);
                            int buf = 1;
                            for (int k = 0; k < nabor.Length; k++)
                            {
                                if (f[k] == 1)
                                    buf *= f[k] * x[k];
                                else
                                    buf *= nabor[k] * x[k];
                            }
                            res[j] += buf;
                            res[j] %= 2;
                        }
                    }
                }
                int b = 0;
            }
            return m;
        }
        private bool IsBalanced(int[] vec)
        {
            if (weight(vec) == vec.Length / 2) return true;
            return false;
        }
        private int[] polinom(int[] vec)
        {
            int[] res = new int[vec.Length];
            int[] buf = vec;
            int k = vec.Length;
            int j = 0;
            while (k != 0)
            {
                res[j] = buf[0];
                for (int i = 0; i < k - 1; i++)
                {
                    buf[i] = buf[i] + buf[i + 1];
                    buf[i] %= 2;
                }
                k--; j++;
            }
            return res;
        }
    }
}
