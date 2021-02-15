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
        Matrix a = new Matrix(3);
        Matrix b = new Matrix(3);
        Matrix u0 = new Matrix(3);

        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
   
        public LinearGenerator()
        {
            SetTabs();
            a.TableName = "A";
            b.TableName = "B";
            u0.TableName = "U(0)";
            SeqElements.Add(u0);
           
        }

      

        public override void GenerateElement(int mod)
        {
            int n = SeqElements.Count;
            Matrix element = ((a * SeqElements[n-1] % mod) - (SeqElements[n-1] * a % mod) + b) % mod;
            element.TableName = String.Format("U({0})", SeqElements.Count);
            SeqElements.Add(element);
        }


        

        protected override void SetTabs(params int[] list)
        {                    
            MatrixTab aTab = new MatrixTab("Матрица A", a);
            MatrixTab bTab = new MatrixTab("Матрица B", b);
            MatrixTab u0Tab = new MatrixTab("Матрица U(0)", u0);

            aTab.Checked += ATab_Checked; ;
            bTab.Checked += BTab_Checked;
            u0Tab.Checked += U0Tab_Checked;

            main.TabStackPanel.Children.Clear();
            main.TabStackPanel.Children.Add(aTab);
            main.TabStackPanel.Children.Add(bTab);
            main.TabStackPanel.Children.Add(u0Tab);
           
            aTab.IsChecked = true;
        }

        

        private void U0Tab_Checked(object sender, RoutedEventArgs e)
        {
            main.MatrixDataGrid.DataContext = u0;
        }

        private void BTab_Checked(object sender, RoutedEventArgs e)
        {
            main.MatrixDataGrid.DataContext = b;
        }

        private void ATab_Checked(object sender, RoutedEventArgs e)
        {
            main.MatrixDataGrid.DataContext = a;
        }

        public override void ReadFromFile(string path)
        {
            
            Matrix currentMatrix = main.MatrixDataGrid.DataContext as Matrix ;
            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);

            int rank = 0;
            try
            {
                rank = Int32.Parse(sr.ReadLine());
            }
            catch
            {
                MessageBox.Show("Неверный ввод. Файл должен начинаться с числа.");
            }

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

            
                a.ReadMatrix(sr);
                b.ReadMatrix(sr);
                u0.ReadMatrix(sr);
           
            main.MatrixDataGrid.DataContext = null;
            main.MatrixDataGrid.DataContext = currentMatrix;


        }

        public override void WriteResultToFile()
        {
            Console.Beep(); 
            StreamWriter sw = new StreamWriter("output.txt", true, System.Text.Encoding.Default);
            sw.WriteLine("Линейно-конгруэнтный генератор \n");
            a.WriteMatrix(sw);
            b.WriteMatrix(sw);
            sw.WriteLine("\n");

            foreach (Matrix dt in SeqElements)
                dt.WriteMatrix(sw);

            
            Process.Start(@"output.txt");
            sw.Close();
        }

        public override void ClearResults()
        {
            SeqElements.Clear();
            SeqElements.Add(u0);
        }

        public override void ClearAll()
        {
            a.Rows.Clear();
            a.Columns.Clear();
            b.Rows.Clear();
            b.Columns.Clear();
            ClearResults();
        }

    }        
       
}
