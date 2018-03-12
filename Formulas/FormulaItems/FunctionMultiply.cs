using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class FunctionMultiply : FunctionItem
    {

		public List<bool> Flags = new List<bool>();

        public override double GetValue()
        {

			double result = 0;

			for (int i = 0 ; i < Arguments.Count ; ++i) {

				FormulaItem item = Arguments[i];
				double value = item.GetValue();
				if (i == 0) {
					result = value;
					continue;
				}

				bool flag = Flags[i];
				if (flag) {
					result *= value;
				} else {
					result /= value;
				}

			}

			return result;

        }

		

		public override FormulaItem Clone(List<SymbolValuePair> values)
		{
			FunctionMultiply cloned = (FunctionMultiply)base.Clone(values);

			foreach (bool flag in Flags)
				cloned.Flags.Add(flag);

			return cloned;

		}
 
    }
}
