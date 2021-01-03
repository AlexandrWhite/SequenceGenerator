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

        Matrix u0 = new Matrix();
        Matrix u1 = new Matrix();

        public FibbGenerator()
        {
            SetTabs();            
        }

        protected override void SetTabs()
        {   
            
            RadioButton u0 = new RadioButton();
            RadioButton u1 = new RadioButton();
            u0.Tag = "U(0)";
            u1.Tag = "U(1)";
            u0.Template = Application.Current.MainWindow.Resources["TabTemplate"] as ControlTemplate;
            u1.Template = Application.Current.MainWindow.Resources["TabTemplate"] as ControlTemplate;
            u0.Checked += U0_Checked;
            u1.Checked += U1_Checked;
            main.TabStackPanel.Children.Clear();
            main.TabStackPanel.Children.Add(u0);
            main.TabStackPanel.Children.Add(u1);
        }

        protected override void GenerateElement(int mod) {
            int n = SeqElements.Count;
            Matrix element = ((SeqElements[n-2]*SeqElements[n-1]%mod) - (SeqElements[n-1]*SeqElements[n-2]%mod)%mod);
            element.TableName = String.Format("U({0})", n-1); 
        }

        private void U1_Checked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).MatrixControl.DataContext = u1;
        }

        private void U0_Checked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).MatrixControl.DataContext = u0;
        }   

        protected override void WriteResultToFile()
        {
            StreamWriter sw = new StreamWriter("FibbOutput.txt", false, System.Text.Encoding.Default);          

            foreach (Matrix dt in SeqElements)
                dt.WriteMatrix(sw);

            sw.Close();
            Process.Start(@"FibbOutput.txt");
        }

        protected override void ReadResultFromFile(string path)
        {
           //Пока не дописал
        }
    }
}
