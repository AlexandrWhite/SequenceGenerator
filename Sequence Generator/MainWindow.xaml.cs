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
using ControlLib;

namespace Sequence_Generator
{
   
    public partial class MainWindow : Window
    {
      
        Brush correctBrush;
        Brush wrongBrush;
        Brush defaultBrush;



        public MatrixGenerator mg;

        public MainWindow()
        {       
            InitializeComponent();
           
            correctBrush = (Brush)Application.Current.MainWindow.FindResource("CorrectBrush");
            wrongBrush = (Brush)Application.Current.MainWindow.FindResource("WrongBrush");
            defaultBrush = (Brush)Application.Current.MainWindow.FindResource("DefaultBrush");

            mg = new LinearGenerator();
            SelectModeComboBox.SelectedItem = SelectModeComboBox.Items[0];
           
            
            
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
            mg.ClearResults();
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
            int mod = GetNumber();
            if (mod != 0)
            {
                //mg.ClearResults();               
                mg.GenerateElements(mod,(int)numericUpDown.Value);

                if (OutputCheckBox.IsChecked == true)
                    mg.WriteResultToFile();                
            }
        }     
        
        public void MatrixDataGridRefresh(DataTable d)
        {
            MatrixDataGrid.DataContext = null;
            MatrixDataGrid.DataContext = d;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Matrix targetTable = MatrixDataGrid.DataContext as Matrix;
            targetTable.AddOrder();           
            MatrixDataGridRefresh(targetTable);

            if (targetTable.Rows.Count == 0)
                RemoveButton.IsEnabled = false;
            else
                RemoveButton.IsEnabled = true;

            mg.ClearResults();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Matrix targetTable = MatrixDataGrid.DataContext as Matrix;
            targetTable.RemoveOrder();

            MatrixDataGridRefresh(targetTable);

            if (targetTable.Rows.Count == 0)
                RemoveButton.IsEnabled = false;
            else
                RemoveButton.IsEnabled = true;

            mg.ClearResults();
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
            }
            catch
            {
                GpTextBoxTooltipText.Text = "Неверный ввод";
                GpTextBoxTooltipText.Background = wrongBrush;
                GpTextBoxTooltip.IsOpen = true;
                
            }
            return 0;
        
        }           

      

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true) {                
                    mg.ReadFromFile(openFileDialog.FileName);                                        
            }              
        }

 

        private void MatrixDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            GpTextBox.Text = "";
            mg.ClearAll();
            MatrixDataGridRefresh(MatrixDataGrid.DataContext as DataTable);
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            mg = new LinearGenerator();          
        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            mg = new FibbGenerator();   
        }

        private void ParametrExpander_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (ParametrExpander.Content != null)
            {
                ParametrExpander.Visibility = Visibility.Visible;
            }
            else
            {
                ParametrExpander.Visibility = Visibility.Hidden;
            }
        }

        private void RandomMatrixFillClick(object sender, RoutedEventArgs e)
        {
            int n = GetNumber();
            if (n != 0)
            {
                for(int i = 0; i < TabStackPanel.Children.Count; i++)
                {
                    (TabStackPanel.Children[i] as MatrixTab).Matrix.SetOreder((int)RandomFillNumericUpDown.Value);
                    (TabStackPanel.Children[i] as MatrixTab).Matrix.RandomFill(n);
                    
                }
            }
        }
    }        
}
