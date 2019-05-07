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
                tblRes.Text = "Вес равен " + weight(vec)+"\n";
                int[] Furie = spectrF(vec);
                int[] WA = spectrWA(vec);

                for (int i = 0; i < Furie.Length; i++)
                    tblRes.Text += Furie[i] + " ";
                tblRes.Text += "\n";
                for (int i = 0; i < Furie.Length; i++)
                    tblRes.Text += WA[i] + " ";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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
    }
}
