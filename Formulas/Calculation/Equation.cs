using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class Equation : CalculationItem
	{
		public SymbolItem Symbol { get; set; }
		public FormulaItem Symbolic { get; set; }
		public FormulaItem Numbers { get; set; }
		public ValueItem Value { get; set; }

		public SymbolValuePair GetSymbolSymbolValuePair()
		{
			return new SymbolValuePair { Symbol = Symbol, Value = Value };
		}



		public override string ToString()
		{
			if (null == Symbol)
				return Symbol.Symbol;

			return base.ToString();	
		}



		public override void Calculate(CalculationItemStack calculationStack, bool isToAdd = true)
		{

			if (null == Value) {


				if (null == Numbers) {

					if (null != Symbolic) {
						List<SymbolValuePair> symbolValueList = calculationStack.GetAsSymbolValueList();
						Numbers = Symbolic.Clone(symbolValueList);
					} else {

						if (null != Symbol) {
							List<SymbolValuePair> symbolValueList = calculationStack.GetAsSymbolValueList();
							Value = (ValueItem)Symbol.Clone(symbolValueList);
							return;
						}
					}
				}

				double value = Numbers.GetValue();
				Value = new ValueItem();
				Value.Value = value;
			}

			if (isToAdd)
				calculationStack.Items.Add(this);

		}


	}
}
