using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class FunctionSqrt : FunctionItem
    {


	

        public override double GetValue()
        {
            FormulaItem value = Arguments.First();
            double arg = value.GetValue();
			double result = Math.Sqrt(arg);
            return result;
        }
    }
}
