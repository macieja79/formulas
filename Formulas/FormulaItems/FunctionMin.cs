using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class FunctionMin : FunctionItem
    {

		

        public override double GetValue()
        {

            List<double> values = Arguments.Select(a => a.GetValue()).ToList();
            double result = values.Min();
            return result;
        }
    }
}
