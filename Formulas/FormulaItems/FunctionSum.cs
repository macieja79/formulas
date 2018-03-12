using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class FunctionSum : FunctionItem
    {

		public FunctionSum()
		{
			Coeffs = new List<double>();
		}

        public override double GetValue()
        {

			double sum = 0.0;
			for (int i = 0 ; i < Arguments.Count ; ++i) {

				double value = Arguments[i].GetValue();
				double coeff = Coeffs[i];

				sum += value * coeff;
			}

			return sum;

        }


		public List<double> Coeffs { get; set; }

		public override FormulaItem Clone(List<SymbolValuePair> values)
		{
			FunctionSum cloned = (FunctionSum)base.Clone(values);

			foreach (double coeff in Coeffs)
				cloned.Coeffs.Add(coeff);

			return cloned;

		}

    }
}
