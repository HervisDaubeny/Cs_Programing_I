using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvaluateLib;

namespace EvaluateLib.Tests {
    class Utils {
        public bool CompareExpressions(Expression First, Expression Second) {
            if (First.GetValue() == Second.GetValue()) {
                CompareExpressions(First.LeftSon, Second.LeftSon)
            }
            else {
                return false;
            }
        }
    }
}
