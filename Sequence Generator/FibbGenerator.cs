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
    public class FibbGenerator:MatrixGenerator
    {
        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;

        Matrix u0 = new Matrix(3);
        Matrix u1 = new Matrix(3);

        public FibbGenerator()
        {
            SetTabs();
            u0.TableName = "U(0)";
            u1.TableName = "U(1)";
            SeqElements.Add(u0);
            SeqElements.Add(u1);
        }

        protected override void SetTabs()
        {
            MatrixTab u0Tab = new MatrixTab("U(0)",u0);
            MatrixTab u1Tab = new MatrixTab("U(1)",u1);
            u0Tab.Checked += U0Tab_Checked;
            u1Tab.Checked += U1Tab_Checked;

            main.TabStackPanel.Children.Clear();
            main.TabStackPanel.Children.Add(u0Tab);
            main.TabStackPanel.Children.Add(u1Tab);
            
            u0Tab.IsChecked = true;
        }

        private void U1Tab_Checked(object sender, RoutedEventArgs e)
        {
            main.MatrixDataGrid.DataContext = u1;
        }

        private void U0Tab_Checked(object sender, RoutedEventArgs e)
        {
            main.MatrixDataGrid.DataContext = u0;
        }

        public override void GenerateElement(int mod) {
            int n = SeqElements.Count;
            Matrix element = ((SeqElements[n-2]*SeqElements[n-1]%mod) - (SeqElements[n-1]*SeqElements[n-2]%mod)%mod);
            element.TableName = String.Format("U({0})", n-1);
            SeqElements.Add(element);
        }       

        public override void WriteResultToFile()
        {
            StreamWriter sw = new StreamWriter("FibbOutput.txt", false, System.Text.Encoding.Default);          

            foreach (Matrix dt in SeqElements)
                dt.WriteMatrix(sw);

            sw.Close();
            Process.Start(@"FibbOutput.txt");
        }

        public override void ReadFromFile(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
                int rank = Int32.Parse(sr.ReadLine());
                Matrix currentMatrix = main.MatrixDataGrid.DataContext as Matrix;

                u0.Rows.Clear();
                u0.Columns.Clear();
                u1.Rows.Clear();
                u1.Columns.Clear();


                for (int i = 0; i < rank; i++)
                {
                    u0.Columns.Add(i.ToString(), typeof(int));
                    u1.Columns.Add(i.ToString(), typeof(int));
                }

                for (int i = 0; i < rank; i++)
                {
                    u0.Rows.Add(u0.NewRow());
                    u1.Rows.Add(u1.NewRow());
                }

                u0.ReadMatrix(sr);
                u1.ReadMatrix(sr);

                main.MatrixDataGrid.DataContext = null;
                main.MatrixDataGrid.DataContext = currentMatrix;
            }
            catch
            {
                MessageBox.Show("Произошла ошибка ввода. Проверьте правильность введнных данных");
            }
        }

        public override void ClearResults()
        {
            SeqElements.Clear();
            SeqElements.Add(u0);
            SeqElements.Add(u1);
        }

        public override void ClearAll()
        {
            ClearResults();
            u0.Rows.Clear();
            u0.Columns.Clear();
        }
    }
}
