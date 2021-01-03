using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Sequence_Generator
{
    static class Algorithms
    {
        public static List<Matrix> CongSeqElements = new List<Matrix>();

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
         
        public static void GenerateNewElement(Matrix a,Matrix b,int mod)
        {          
            Matrix element = ((a * CongSeqElements.Last()%mod) - (CongSeqElements.Last()*a%mod) +b)%mod ;
            element.TableName = String.Format("U{0}", CongSeqElements.Count);
            CongSeqElements.Add(element);
        }

        private static void WriteMatrix(Matrix dt,StreamWriter sw) {
            sw.WriteLine(dt.TableName);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sw.Write(dt.Rows[i][j].ToString() + " ");
                }
                sw.Write("\n");
            }
            sw.WriteLine();
            
        }

        public static void WriteResultToFile(Matrix a, Matrix b, List<Matrix> dataTables)
        {
            StreamWriter sw = new StreamWriter("output.txt",false, System.Text.Encoding.Default);

            WriteMatrix(a, sw);
            WriteMatrix(b, sw);

            sw.WriteLine("\n");

            foreach (Matrix dt in dataTables)            
               WriteMatrix(dt, sw);
          
            sw.Close();
            Process.Start(@"output.txt");

        }

    }
}
