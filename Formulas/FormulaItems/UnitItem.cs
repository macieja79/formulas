using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	class UnitItem : FormulaItem
	{


		public string Unit { get; set; }
		
		public override double GetValue()
		{
			return double.NaN;
		}



		public override FormulaItem Clone(List<SymbolValuePair> values)
		{
			UnitItem cloned = new UnitItem();
			cloned.Unit = Unit;
			return cloned;
		}
	}
}
