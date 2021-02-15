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
    class MatrixTab:RadioButton
    {
        Matrix matrix;
        MainWindow main = (MainWindow)System.Windows.Application.Current.MainWindow;
        public Matrix Matrix { get { return matrix; } }

        public MatrixTab(string name,Matrix matrix) : base() {
            this.Tag = name;
            this.matrix = matrix;
            this.Template = Application.Current.MainWindow.Resources["TabTemplate"] as ControlTemplate;            
        }
        public void  SetMatrix(DataGrid parent)
        {
            parent.DataContext = this;
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            main.MatrixDataGrid.DataContext = matrix;
        }


    }
}
