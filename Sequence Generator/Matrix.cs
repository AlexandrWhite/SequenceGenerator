using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
namespace Sequence_Generator
{
    public class Matrix:DataTable
    {

        public Matrix(int rank = 0) : base() {
            for(int i=0;i<rank;i++)
                AddOrder();
        }


        public void AddOrder()
        {
            Rows.Add(this.NewRow());
            Columns.Add((Columns.Count).ToString(), typeof(int));
            for (int i = 0; i < Rows.Count; i++)
                Rows[i][Rows.Count-1] = 0;
            for (int i = 0; i < Columns.Count; i++)
                Rows[Columns.Count-1][i] = 0;
        }

        public void RemoveOrder()
        {
            if (Rows.Count == 0) { return; }
            Rows.RemoveAt(Rows.Count - 1);
            Columns.RemoveAt(Columns.Count - 1);
        }





        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix res = new Matrix();
          
            for (int i = 0; i < m1.Rows.Count; i++)
                res.Rows.Add(res.NewRow());

            for (int i = 0; i < m2.Columns.Count; i++)
                res.Columns.Add(i.ToString(), typeof(int));

            for (int i = 0; i < m1.Rows.Count; i++)
            {
                for (int j = 0; j < m2.Columns.Count; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < m1.Columns.Count; k++)                    
                        sum += Int32.Parse(m1.Rows[i][k].ToString()) * Int32.Parse(m2.Rows[k][j].ToString());
                    
                    res.Rows[i][j] = sum;
                }
            }
            return res;
        }

        public static Matrix operator %(Matrix m1,int mod)
        {
            Matrix res = new Matrix();

            for (int i = 0; i < m1.Rows.Count; i++)
                res.Rows.Add(res.NewRow());

            for (int i = 0; i < m1.Columns.Count; i++)
                res.Columns.Add(i.ToString(), typeof(int));

            for (int i = 0; i < m1.Rows.Count; i++)
                for (int j = 0; j < m1.Columns.Count; j++)
                    if ((int)m1.Rows[i][j] >= 0)
                        res.Rows[i][j] = (int)m1.Rows[i][j] % mod;
                    else
                        res.Rows[i][j] = mod - ((-(int)m1.Rows[i][j]) % mod);
            
            return res;
        }

        public static Matrix operator + (Matrix m1,Matrix m2)
        {
            Matrix res = new Matrix();

            for (int i = 0; i < m1.Rows.Count; i++)
                res.Rows.Add(res.NewRow());

            for (int i = 0; i < m1.Columns.Count; i++)
                res.Columns.Add(i.ToString(), typeof(int));

            for (int i = 0; i < m1.Rows.Count; i++)            
                for (int j = 0; j < m1.Columns.Count; j++)                
                    res.Rows[i][j] = (int)m1.Rows[i][j] + (int)m2.Rows[i][j];               
            
            return res;
        }

        public static Matrix operator - (Matrix m1, Matrix m2)
        {
            Matrix res = new Matrix();

            for (int i = 0; i < m1.Rows.Count; i++)
                res.Rows.Add(res.NewRow());

            for (int i = 0; i < m1.Columns.Count; i++)
                res.Columns.Add(i.ToString(), typeof(int));

            for (int i = 0; i < m1.Rows.Count; i++)            
                for (int j = 0; j < m1.Columns.Count; j++)                
                    res.Rows[i][j] = (int)m1.Rows[i][j] - (int)m2.Rows[i][j];
                           
            return res;
        }

        public void WriteMatrix(StreamWriter sw)
        {
            sw.WriteLine(TableName);
            for (int i = 0; i < Rows.Count; i++)
            {
                for (int j = 0; j < Columns.Count; j++)
                {                   
                    sw.Write(Rows[i][j].ToString() + " ");
                }

                sw.Write("\n");
            }
            sw.WriteLine();
        }

        public void ReadMatrix(StreamReader sr)
        {
           
            for (int i = 0; i < Rows.Count; i++)
            {               
                string[] line = sr.ReadLine().Split();

                for (int j = 0; j < Columns.Count; j++)                
                    Rows[i][j] = Int32.Parse(line[j]);                
            }
            sr.ReadLine();
        }

    }
}
