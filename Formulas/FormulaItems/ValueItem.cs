using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class ValueItem : FunctionItem
    {

		public ValueItem()
		{
			Value = double.NaN;
		}
		
        public double Value { get; set; }

        public override double GetValue()
        {
            return Value;
        }

		public override FormulaItem Clone(List<SymbolValuePair> values)
		{
			ValueItem cloned = (ValueItem)base.Clone(values);
			cloned.Value = Value;
			return cloned;

		}



    }

}
