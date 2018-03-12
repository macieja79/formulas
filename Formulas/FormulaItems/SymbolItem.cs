using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class SymbolItem : FormulaItem
	{

		public string Symbol { get; set; }

		public override bool IsSymbol
		{
			get
			{
				return true;
			}
		}



		public override double GetValue()
		{

			return double.NaN;
		}

		public override FormulaItem Clone(List<SymbolValuePair> values)
		{

			SymbolValuePair matched = values.FirstOrDefault(p => p.Symbol.Symbol == Symbol);
			if (null != matched) {

				ValueItem item = (ValueItem)matched.Value.Clone(values);
				return item;
			}

			return this;

		}
	}
}
