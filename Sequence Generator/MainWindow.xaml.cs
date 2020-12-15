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

namespace Sequence_Generator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
      
        Brush correctBrush;
        Brush wrongBrush;
        Brush defaultBrush;

        DataTable dt1= new DataTable();
        DataTable dt2 = new DataTable();
        DataTable result= new DataTable();

        public MainWindow()
        {
            
            InitializeComponent();
           
            correctBrush = (Brush)Application.Current.MainWindow.FindResource("CorrectBrush");
            wrongBrush = (Brush)Application.Current.MainWindow.FindResource("WrongBrush");
            defaultBrush = (Brush)Application.Current.MainWindow.FindResource("DefaultBrush");


            SetDt2(2, 3, dt1, 6);
            SetDt2(3, 2, dt2, 5);

            dt1.Rows[0][0] = 1;
            dt1.Rows[0][1] = 2;
            dt1.Rows[0][2] = 1;

            dt1.Rows[1][0] = 0;
            dt1.Rows[1][1] = 1;
            dt1.Rows[1][2] = 2;

            dt2.Rows[0][0] = 1;
            dt2.Rows[0][1] = 0;
            dt2.Rows[1][0] = 0;
            dt2.Rows[1][1] = 1;
            dt2.Rows[2][0] = 1;
            dt2.Rows[2][1] = 1;

            Matrix.DataContext = dt1;

            Calculate();

            Tab1.IsChecked = true;
        }


        void SetDt2(int RowCount,int ColumnCount,DataTable dt2,int value) {
            if (dt2.Rows.Count != RowCount)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    dt2.Rows.Add(dt2.NewRow());
                }
            }

            if (dt2.Columns.Count != ColumnCount)
            {
                for (int i = 0; i < ColumnCount; i++)
                {
                    dt2.Columns.Add(i.ToString(), typeof(int));
                }
            }

            for(int i = 0; i < RowCount; i++)
            {
                for(int j = 0; j < ColumnCount; j++)
                {
                    dt2.Rows[i][j] = value;
                }
            }

        }
        

        void Sum()
        {
            int rCount = dt1.Rows.Count;
            int cCount = dt1.Columns.Count;
            SetDt2(rCount, cCount, result, (int)dt1.Rows[0][0] + (int)dt2.Rows[0][0]);
        }


        private void GpTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (GpTextBox.Text == "") { 
                GpTextBoxTooltip.IsOpen = false;
                GpTextBoxBorder.BorderBrush = defaultBrush;
                return;
            }
            
            int input = Int32.Parse(GpTextBox.Text);         

            if (Algorithms.IsPrime(input))
            {
                GpTextBoxTooltipText.Text = String.Format("Число {0}  является простым числом ", input);
                GpTextBoxTooltipText.Background = correctBrush;
                GpTextBoxBorder.BorderBrush = correctBrush;
            }
            else
            {
                int[] near = Algorithms.nearPrimes(input);
                if(near!=null)
                    GpTextBoxTooltipText.Text = String.Format("Число {0} не является простым числом\nБлижайшие простые: {1} {2}", input,near[0],near[1]);
                else
                    GpTextBoxTooltipText.Text = String.Format("Число {0} не является простым числом",input);

               GpTextBoxTooltipText.Background = wrongBrush;
               GpTextBoxBorder.BorderBrush = wrongBrush;
            }
            
            GpTextBoxTooltip.IsOpen = true;
          
        }

        

        private void GpTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            GpTextBoxTooltip.IsOpen = false;
        }



        private void GpTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
              GpTextBoxTooltip.IsOpen = false;
        }

        private void GpTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (GpTextBox.Text == "") { GpTextBoxTooltip.IsOpen = false; return; }
            GpTextBoxTooltip.IsOpen = true;
        }

        private void GpTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Matrix.DataContext = dt1;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Matrix.DataContext = dt2;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            Matrix.DataContext = result;
        }

        private void Matrix_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {        
           
            //Calculate();

        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable targetTable = Matrix.DataContext as DataTable;
            targetTable.Rows.Add(targetTable.NewRow());
            for(int i = 0; i < targetTable.Columns.Count; i++)
            {
                targetTable.Rows[targetTable.Rows.Count-1][i] = 0;
            }
        }

        private void AddColumnButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable targetTable = Matrix.DataContext as DataTable;
            int t = targetTable.Columns.Count ;
            targetTable.Columns.Add(t+"", typeof(int));
            for(int i = 0; i < targetTable.Rows.Count; i++)
            {
                targetTable.Rows[i][targetTable.Columns.Count - 1] = 0;
            }
        }

        void Calculate()
        {
            if (Algorithms.MatrixMultiply(dt1, dt2, 1) != null)
            {
                try
                {
                    if (Algorithms.IsPrime(Int32.Parse(GpTextBox.Text)))
                    {
                        result = Algorithms.MatrixMultiply(dt1, dt2, Int32.Parse(GpTextBox.Text));

                    }
                    else
                    {
                        GpTextBoxTooltip.IsOpen = true;
                    }
                    ResultTab.IsChecked = true;
                }
                catch
                {
                    GpTextBoxTooltipText.Text = "Неверный ввод";
                    GpTextBoxTooltipText.Background = wrongBrush;
                    GpTextBoxTooltip.IsOpen = true;
                }
            }
            else
            {
                MessageBox.Show("Матрицы не совместны");
            }
        }
    }
}
