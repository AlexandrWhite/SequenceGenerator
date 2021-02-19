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
using ControlLib;

namespace Sequence_Generator
{
    public class FibbGenerator : MatrixGenerator
    {
        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
        int p = 2;
        int q = 1;
        int rank = 3;
        NumericUpDown PnumericUpDown = new NumericUpDown();
        NumericUpDown QnumericUpDown = new NumericUpDown();


        private void SetParametrPanel()
        {
            PnumericUpDown.Value = p;
            QnumericUpDown.Value = q;

            PnumericUpDown.VerticalAlignment = VerticalAlignment.Center;
            QnumericUpDown.VerticalAlignment = VerticalAlignment.Center;

            PnumericUpDown.MinValue = 1;
            QnumericUpDown.MinValue = 1;

            PnumericUpDown.ValueChanged += PnumericUpDown_ValueChanged;
            QnumericUpDown.ValueChanged += QnumericUpDown_ValueChanged;

            parametrsPanel = new StackPanel();

            parametrsPanel.Margin = new Thickness(10);

            StackPanel s1 = new StackPanel();
            s1.Orientation = Orientation.Horizontal;

            StackPanel s2 = new StackPanel();
            s2.Orientation = Orientation.Horizontal;

            Label pLabel = new Label();
            Label qLabel = new Label();

            pLabel.Content = "p";
            qLabel.Content = "q";

            s1.Children.Add(pLabel);
            s1.Children.Add(PnumericUpDown);
            s2.Children.Add(qLabel);
            s2.Children.Add(QnumericUpDown);

            parametrsPanel.Children.Add(s1);
            parametrsPanel.Children.Add(s2);
        }

        private void QnumericUpDown_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SetGenerator((int)PnumericUpDown.Value, (int)QnumericUpDown.Value);

        }

        private void PnumericUpDown_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SetGenerator((int)PnumericUpDown.Value, (int)QnumericUpDown.Value);
        }

        public FibbGenerator()
        {
            SetParametrPanel();
            main.ParametrExpander.Content = parametrsPanel;            
            main.ParametrExpander.Visibility = Visibility.Visible;
            SetGenerator(p,q);
        }

        private void SetGenerator(int p, int q)
        {
            this.p = p;
            this.q = q;
            SetTabs(Math.Max(p, q), rank);
        }

        protected override void SetTabs(params int[] list)
        {
            main.TabStackPanel.Children.Clear();
            SeqElements.Clear();

            int n = list[0];

            for (int i = 0; i < n; i++)
            {
                string name = String.Format("U({0})", i);
                Matrix m = new Matrix(list[1]);
                MatrixTab mTab = new MatrixTab(name, m);
                main.TabStackPanel.Children.Add(mTab);
                m.TableName = name;               
                (main.TabStackPanel.Children[0] as MatrixTab).IsChecked = true;
            }
        }

         
        public override void GenerateElements(int mod, int count)
        {
            SeqElements.Clear();
            for(int i=0;i<Math.Min(main.TabStackPanel.Children.Count,count);i++)
            {
                Matrix matrix = (main.TabStackPanel.Children[i] as MatrixTab).Matrix;
                matrix %= mod;
                matrix.TableName = (main.TabStackPanel.Children[i] as MatrixTab).Matrix.TableName;
                SeqElements.Add(matrix);
            }
            int inputCount = SeqElements.Count;
            for (int i = 0; i < count-inputCount; i++)
            {
                GenerateElement(mod);
            }
        }

        public override void GenerateElement(int mod)
        {
            int n = SeqElements.Count;
            Matrix element = ( ((SeqElements[n - p] * SeqElements[n - q]) % mod) -((SeqElements[n - q] * SeqElements[n - p]) % mod) )%mod;
            element.TableName = String.Format("U({0})", n);
            SeqElements.Add(element);
        }

        public override void WriteResultToFile()
        {
            StreamWriter sw = new StreamWriter("FibbOutput.txt", false, System.Text.Encoding.Default);
            sw.WriteLine("Генератор Фибоначчи p={0} q={1}\n",p,q);

            foreach (Matrix dt in SeqElements)
                dt.WriteMatrix(sw);

            sw.Close();
            Process.Start(@"FibbOutput.txt");
        }

        public override void ReadFromFile(string path)
        {           
                StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
                rank = Int32.Parse(sr.ReadLine());
                string[] parametrs = sr.ReadLine().Split();

                PnumericUpDown.Value = Int32.Parse(parametrs[0]);
                QnumericUpDown.Value = Int32.Parse(parametrs[1]);

                foreach (UIElement mt in main.TabStackPanel.Children)
                {
                    Matrix matrix = (mt as MatrixTab).Matrix;
                    matrix.ReadMatrix(sr);
                    SeqElements.Add(matrix);
                }
        }

        public override void ClearResults()
        {
         
        }

        public override void ClearAll()
        {
            SeqElements.Clear();
            PnumericUpDown.Value = PnumericUpDown.MinValue;
            QnumericUpDown.Value = QnumericUpDown.MinValue;
        }
    }
}
