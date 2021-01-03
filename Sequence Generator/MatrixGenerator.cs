﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sequence_Generator
{
    public abstract class MatrixGenerator
    {
        protected List<Matrix> SeqElements =new List<Matrix>();
        protected abstract void SetTabs();
        protected abstract void GenerateElement(int mod);
        protected abstract void WriteResultToFile();
        protected abstract void ReadResultFromFile(string path);   
    }
}
