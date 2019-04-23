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
                if (!CheckVector(vec)) throw new Exception("Error");
                int[] spectr = new int[vec.Length];

                for (int i = 0; i < vec.Length; i++)
                {
                    int[] x = getBinaryCode(i);
                    for(int j = 0; j < vec.Length; j++)
                    {
                        int[] u = getBinaryCode(j);
                        int buf = 0;
                        for (int k = 0; k < NumberOfVariables; k++)
                        {
                            buf += x[k] * u[k];
                            buf %= 2;
                        }

                        spectr[i] += (int)Math.Pow(-1, buf) * vec[i];
                    }
                    
                }

                tblRes.Text = "";
                for (int i = 0; i < spectr.Length; i++) tblRes.Text += spectr[i] + " ";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private int[] getBinaryCode(int j)
        {
            int[] num=new int[NumberOfVariables];
            string Binarycode = Convert.ToString(j, 2);
            for (int i = 0; i < Binarycode.Length; i++)
                num[NumberOfVariables-i-1] = int.Parse(Binarycode[i].ToString());
            return num;
        }
        //private int x_u(int[] x, int[] u)
        //{
        //    int x_u = 0;
        //    for (int i = 0; i < x.Length; i++)
        //    {
        //        x_u += x[i] * u[i];
        //        x_u %= 2;
        //    }
        //    return x_u;
        //}
        private int weight(int[] vec)
        {
            int c = 0;
            foreach (int x in vec) if (x == 1) c++;
            return c;
        }
        private void BtnWolsh_Adamar_Click(object sender, RoutedEventArgs e)
        {
            int[] vec = GetVector(tbFunc.Text.Trim());

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
