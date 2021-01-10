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



        MatrixGenerator mg;

        public MainWindow()
        {       
            InitializeComponent();
           
            correctBrush = (Brush)Application.Current.MainWindow.FindResource("CorrectBrush");
            wrongBrush = (Brush)Application.Current.MainWindow.FindResource("WrongBrush");
            defaultBrush = (Brush)Application.Current.MainWindow.FindResource("DefaultBrush");

            mg = new LinearGenerator();
            LinearRadioButton.IsChecked = true;               
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
                for (int i = 0; i < 10; i++)
                    mg.GenerateElement(mod);               

                if (OutputCheckBox.IsChecked == true)
                    mg.WriteResultToFile();
                
            }
        }     
        
        private void MatrixDataGridRefresh(DataTable d)
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

        private void SequenceTab_Checked(object sender, RoutedEventArgs e)
        {
            //SequenceMenu.Visibility = Visibility.Visible;
            //MatrixControl.Height = new GridLength(0);
        }     

        private void SequenceTab_Unchecked(object sender, RoutedEventArgs e)
        {
            //SequenceMenu.Visibility = Visibility.Hidden;
            //MatrixControl.Height = new GridLength(21);
        }    
      
        private void btnAddElem_Click(object sender, RoutedEventArgs e)
        {
            //AddCongSeqElement();   
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            //result = Algorithms.CongSeqElements[(int)((sender as Button).Tag)];             
            //MatrixDataGridRefresh(result);
            //ResultTab.Tag = String.Format("U({0})", (int)((sender as Button).Tag));
            //ResultTab.IsChecked = true;
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
                    mg.ReadFromFile(openFileDialog.FileName);                                        
            }              
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
             mg = new FibbGenerator();                       
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            mg = new LinearGenerator();
        }

        private void MatrixDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            GpTextBox.Text = "";
            mg.ClearAll();
            MatrixDataGrid.DataContext = null;
        }
    }


    

      
}
