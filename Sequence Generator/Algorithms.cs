using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence_Generator
{
    static class Algorithms
    {
        public static bool IsPrime(int n)
        {
            if (n == 1) { return false; }
            for(int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0) { return false; }
            }
            return true;
        }

        public static int[] nearPrimes(int n) {
            if (n < 4) { return null; }

            int nearLeft = n;
            int nearRight = n;
         
            while (!IsPrime(nearLeft))
            {
                nearLeft--;
            }
           

            while (!IsPrime(nearRight))
            {
                nearRight++;
            }

            int[] result = { nearLeft, nearRight };
            return result;
        }
        
        public static DataTable MatrixMultiply(DataTable m1,DataTable m2,int mod)
        {
            DataTable res = new DataTable();
            if (m1.Columns.Count == m2.Rows.Count)
            {
                for (int i = 0; i < m1.Rows.Count; i++)                
                    res.Rows.Add(res.NewRow());

                for (int i = 0; i < m2.Columns.Count; i++)                
                    res.Columns.Add(i.ToString(), typeof(int));
                
                for(int i = 0; i < m1.Rows.Count;i++)
                {                    
                    for(int j = 0; j < m2.Columns.Count;j++)
                    {
                        int sum = 0;
                        for (int k = 0; k < m1.Columns.Count; k++)
                        {
                            sum += Int32.Parse(m1.Rows[i][k].ToString()) * Int32.Parse(m2.Rows[k][j].ToString());
                        }
                        res.Rows[i][j] = sum%mod;
                    }
                }
                return res;
            }
            return null;
        }
    }
}
