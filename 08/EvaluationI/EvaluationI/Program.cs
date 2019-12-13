using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvaluateLib;

namespace Evaluation_I {
    class Program {
        static void Main(string[] args) {
            Controler controler = new Controler();

            controler.GetData();
            controler.BuildTree();
            if (controler.IsCorrect()) {
                controler.ComputeTree();
            }
            controler.PrintResult();
        }
    }
}
