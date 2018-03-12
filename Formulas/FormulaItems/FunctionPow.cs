using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class FunctionPow : FunctionItem
	{

	

		public override double GetValue()
		{

			FormulaItem baseItem = Arguments[0];
			FormulaItem exponentItem = Arguments[1];

			double baseValue = baseItem.GetValue();
			double exponentValue = exponentItem.GetValue();

			return Math.Pow(baseValue, exponentValue);

		}
	}
}
