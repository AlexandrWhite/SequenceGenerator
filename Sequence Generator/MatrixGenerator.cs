using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Sequence_Generator
{
    public abstract class MatrixGenerator
    {
        
        public List<Matrix> SeqElements = new List<Matrix>();
        public Panel parametrsPanel;
        protected abstract void SetTabs(params int[] list);
        public abstract void GenerateElement(int mod);
        public abstract void ClearResults();
        public abstract void ClearAll();
        public abstract void WriteResultToFile();
        public abstract void ReadFromFile(string path);   
    }
}
