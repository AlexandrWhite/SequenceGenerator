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



        protected override void GenerateElement(int mod)
        {
            throw new NotImplementedException();
        }

        protected override void SetTabs()
        {
            throw new NotImplementedException();
        }

        protected override void ReadResultFromFile(string path)
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

        protected override void WriteResultToFile()
        {
            
        }

    }        
       
}
