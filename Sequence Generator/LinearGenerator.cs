using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Threading;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace Sequence_Generator
{
    class LinearGenerator : MatrixGenerator
    {
        Matrix a = new Matrix();
        Matrix b = new Matrix();
        Matrix u0 = new Matrix();

        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;

        public LinearGenerator()
        {
            SetTabs();
        }

        protected override void GenerateElement(int mod)
        {
            int n = SeqElements.Count;
            Matrix element = ((a * SeqElements[n-1] % mod) - (SeqElements[n-1] * a % mod) + b) % mod;
            element.TableName = String.Format("U{0}", SeqElements.Count);
            SeqElements.Add(element);
        }

        protected override void SetTabs()
        {
            RadioButton aTab = new RadioButton();
            RadioButton bTab = new RadioButton();
            RadioButton u0Tab = new RadioButton();

            aTab.Tag = "Матрица A";
            bTab.Tag = "Матрица B";
            u0Tab.Tag = "Матрица U(0)";

            aTab.Template = Application.Current.MainWindow.Resources["TabTemplate"] as ControlTemplate;
            bTab.Template = Application.Current.MainWindow.Resources["TabTemplate"] as ControlTemplate;
            u0Tab.Template = Application.Current.MainWindow.Resources["TabTemplate"] as ControlTemplate;

            aTab.Checked += ATab_Checked; ;
            bTab.Checked += BTab_Checked;
            u0Tab.Checked += U0Tab_Checked;

            aTab.IsChecked = true;
            main.TabStackPanel.Children.Clear();
            main.TabStackPanel.Children.Add(aTab);
            main.TabStackPanel.Children.Add(bTab);
            main.TabStackPanel.Children.Add(u0Tab);
        }

        private void U0Tab_Checked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).MatrixControl.DataContext = u0;
        }

        private void BTab_Checked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).MatrixControl.DataContext = b;
        }

        private void ATab_Checked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).MatrixControl.DataContext = a;
        }

        public override void ReadResultFromFile(string path)
        {
            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
            int rank = Int32.Parse(sr.ReadLine());


            a.Rows.Clear();
            a.Columns.Clear();
            b.Rows.Clear();
            b.Columns.Clear();
            u0.Rows.Clear();
            u0.Columns.Clear();


            for (int i = 0; i < rank; i++)
            {
                a.Columns.Add(i.ToString(), typeof(int));
                b.Columns.Add(i.ToString(), typeof(int));
                u0.Columns.Add(i.ToString(), typeof(int));
            }

            for (int i = 0; i < rank; i++)
            {
                a.Rows.Add(a.NewRow());
                b.Rows.Add(b.NewRow());
                u0.Rows.Add(u0.NewRow());
            }

            for (int i = 0; i < rank; i++)
            {
                var line = sr.ReadLine().Split();
                for (int j = 0; j < rank; j++)
                {
                    a.Rows[i][j] = Int32.Parse(line[j]);
                }
            }

            sr.ReadLine();

            for (int i = 0; i < rank; i++)
            {
                var line = sr.ReadLine().Split();
                for (int j = 0; j < rank; j++)
                    b.Rows[i][j] = Int32.Parse(line[j]);
            }

            sr.ReadLine();

            for (int i = 0; i < rank; i++)
            {
                var line = sr.ReadLine().Split();
                for (int j = 0; j < rank; j++)
                    u0.Rows[i][j] = Int32.Parse(line[j]);
            }
        }

        public override void WriteResultToFile()
        {
            StreamWriter sw = new StreamWriter("output.txt", false, System.Text.Encoding.Default);

            a.WriteMatrix(sw);
            b.WriteMatrix(sw);

            sw.WriteLine("\n");

            foreach (Matrix dt in SeqElements)
                dt.WriteMatrix(sw);

            sw.Close();
            Process.Start(@"output.txt");
        }

    }        
       
}
