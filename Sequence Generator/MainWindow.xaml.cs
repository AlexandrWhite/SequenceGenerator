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

namespace Sequence_Generator
{
   
    public partial class MainWindow : Window
    {
      
        Brush correctBrush;
        Brush wrongBrush;
        Brush defaultBrush;

        Matrix dt1= new Matrix();
        Matrix dt2 = new Matrix();
        Matrix u0 = new Matrix();
        Matrix result= new Matrix();

        public MainWindow()
        {
            
            InitializeComponent();
           
            correctBrush = (Brush)Application.Current.MainWindow.FindResource("CorrectBrush");
            wrongBrush = (Brush)Application.Current.MainWindow.FindResource("WrongBrush");
            defaultBrush = (Brush)Application.Current.MainWindow.FindResource("DefaultBrush");


            SetDt2(3, 3, dt1, 0);
            SetDt2(3, 3, dt2, 0);
            SetDt2(3, 3, u0, 0);
            Algorithms.CongSeqElements.Add(u0);
            
            MatrixDataGrid.DataContext = dt1;
            u0.ColumnChanged += U0_ColumnChanged;
            dt1.ColumnChanged += Dt1_ColumnChanged;
            dt2.ColumnChanged += Dt2_ColumnChanged;
            u0.TableName = "U0";
            dt1.TableName = "A";
            dt2.TableName = "B";


            Tab1.IsChecked = true;
        }

        private void Dt2_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            clearResults();
        }

        private void Dt1_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            clearResults();
        }

        void clearResults() {
            Algorithms.CongSeqElements.Clear();
            SequencePanel.Children.RemoveRange(1, SequencePanel.Children.Count);
            Algorithms.CongSeqElements.Add(u0);
        }

        private void U0_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            clearResults();
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
            clearResults();
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
            if (GetNumber() != 0)
            {
                for (int i = 0; i < 5; i++)
                    AddCongSeqElement();

                SequenceTab.IsChecked = true;

                if (OutputCheckBox.IsChecked == true)                
                    Algorithms.WriteResultToFile(dt1,dt2,Algorithms.CongSeqElements);
                
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MatrixDataGrid.DataContext = dt1;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            MatrixDataGrid.DataContext = dt2;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            MatrixDataGrid.DataContext = result;
        }

        private void MatrixDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {        
           
            //Calculate();

        }

        private void MatrixDataGridRefresh(DataTable d)
        {
            MatrixDataGrid.DataContext = null;
            MatrixDataGrid.DataContext = d as DataTable;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable targetTable = MatrixDataGrid.DataContext as DataTable;
            dt1.Rows.Add(dt1.NewRow());
            dt2.Rows.Add(dt2.NewRow());
            u0.Rows.Add(u0.NewRow());
            int t = targetTable.Columns.Count;

            dt1.Columns.Add(t + "", typeof(int));
            dt2.Columns.Add(t + "", typeof(int));
            u0.Columns.Add(t + "", typeof(int));

            for (int i = 0; i < targetTable.Columns.Count; i++)
            {
                dt1.Rows[targetTable.Rows.Count-1][i] = 0;
                dt2.Rows[targetTable.Rows.Count - 1][i] = 0;
                u0.Rows[targetTable.Rows.Count - 1][i] = 0;

            }

            for (int i = 0; i < targetTable.Rows.Count; i++)
            {
                dt1.Rows[i][targetTable.Columns.Count - 1] = 0;
                dt2.Rows[i][targetTable.Columns.Count - 1] = 0;
                u0.Rows[i][targetTable.Columns.Count - 1] = 0;
            }

            if (dt1.Rows.Count == 0)
                RemoveButton.IsEnabled = false;
            else
                RemoveButton.IsEnabled = true;

            MatrixDataGridRefresh(targetTable);
            clearResults();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable targetTable = MatrixDataGrid.DataContext as DataTable;
            
            dt1.Rows.Remove(dt1.Rows[dt1.Rows.Count - 1]);
            dt1.Columns.Remove(dt1.Columns[dt1.Columns.Count - 1]);
            dt2.Rows.Remove(dt2.Rows[dt2.Rows.Count - 1]);
            dt2.Columns.Remove(dt2.Columns[dt2.Columns.Count - 1]);

            u0.Rows.Remove(u0.Rows[u0.Rows.Count - 1]);
            u0.Columns.Remove(u0.Columns[u0.Columns.Count - 1]);

            MatrixDataGridRefresh(targetTable);

            if (dt1.Rows.Count == 0)
                RemoveButton.IsEnabled = false;
            else
                RemoveButton.IsEnabled = true;

            clearResults();
        }

        

        int GetNumber()
        {
            try
            {
                if (Algorithms.IsPrime(Int32.Parse(GpTextBox.Text)))
                { 

                    return Int32.Parse(GpTextBox.Text);
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
            return 0;
        
        }
           
        

        

        private void SequenceTab_Checked(object sender, RoutedEventArgs e)
        {
            SequenceMenu.Visibility = Visibility.Visible;
            MatrixControl.Height = new GridLength(0);
        }     

        private void SequenceTab_Unchecked(object sender, RoutedEventArgs e)
        {
            SequenceMenu.Visibility = Visibility.Hidden;
            MatrixControl.Height = new GridLength(21);
        }

        private void U0Matrix_Checked(object sender, RoutedEventArgs e)
        {
            MatrixDataGrid.DataContext = u0;            
        }



        void AddCongSeqElement() {
            Button b = new Button();
            b.Content = String.Format("U({0})", Algorithms.CongSeqElements.Count);
            b.Width = 60;
            b.Height = 22;
            b.Margin = new Thickness(2);
            b.Tag = Algorithms.CongSeqElements.Count;
            b.Click += B_Click;
            if (GetNumber() != 0)
            {
                Algorithms.GenerateNewElement(dt1, dt2, GetNumber());
                SequencePanel.Children.Add(b);
            }
            else
            {
                GpTextBoxTooltipText.Text = "Неверный ввод";
                GpTextBoxTooltipText.Background = wrongBrush;
                GpTextBoxTooltip.IsOpen = true;
            }
        }


        private void btnAddElem_Click(object sender, RoutedEventArgs e)
        {
            AddCongSeqElement();   
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            result = Algorithms.CongSeqElements[(int)((sender as Button).Tag)];
             
            MatrixDataGridRefresh(result);
            ResultTab.Tag = String.Format("U({0})", (int)((sender as Button).Tag));
            ResultTab.IsChecked = true;
        }

        private void ResultTab_Checked(object sender, RoutedEventArgs e)
        {
            MatrixControl.Height = new GridLength(0);
        }

        private void ResultTab_Unchecked(object sender, RoutedEventArgs e)
        {
            MatrixControl.Height = new GridLength(21);
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true) {
                
                StreamReader sr = new StreamReader(openFileDialog.FileName,System.Text.Encoding.Default);
                int rank = Int32.Parse(sr.ReadLine());
                

                dt1.Rows.Clear();
                dt1.Columns.Clear();
                dt2.Rows.Clear();
                dt2.Columns.Clear();
                u0.Rows.Clear();
                u0.Columns.Clear();


                for (int i = 0; i < rank; i++)
                {
                    dt1.Columns.Add(i.ToString(), typeof(int));
                    dt2.Columns.Add(i.ToString(), typeof(int));
                    u0.Columns.Add(i.ToString(), typeof(int));
                }

                for (int i = 0; i < rank; i++)
                {
                    dt1.Rows.Add(dt1.NewRow());
                    dt2.Rows.Add(dt2.NewRow());
                    u0.Rows.Add(u0.NewRow());
                }



                for (int i = 0; i < rank; i++)
                {
                    var line = sr.ReadLine().Split();
                    for(int j = 0; j < rank; j++) {
                        dt1.Rows[i][j] = Int32.Parse(line[j]);
                    }
                }

                sr.ReadLine();

                for (int i = 0; i < rank; i++)
                {
                    var line = sr.ReadLine().Split();
                    for (int j = 0; j < rank; j++)                    
                        dt2.Rows[i][j] = Int32.Parse(line[j]);                    
                }

                sr.ReadLine();

                for (int i = 0; i < rank; i++)
                {
                    var line = sr.ReadLine().Split();
                    for (int j = 0; j < rank; j++)
                        u0.Rows[i][j] = Int32.Parse(line[j]);
                }

                MatrixDataGridRefresh(dt1);









                // MessageBox.Show("Невернно введены данные");

            }
              
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            FibbGenerator fg = new FibbGenerator();     
            
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            LinearGenerator l = new LinearGenerator();
        }
    }
}
