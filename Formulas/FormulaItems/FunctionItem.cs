using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public abstract class FunctionItem : FormulaItem
	{


		public FunctionItem()
		{
			Arguments = new List<FormulaItem>();
		
		}

		public override bool IsSymbol
		{
			get
			{
				return Arguments.Any(a => a.IsSymbol);
			}
		}

		public string Pattern { get; set; }
		public List<FormulaItem> Arguments { get; set; }


		public override FormulaItem Clone(List<SymbolValuePair> values)
		{
			FunctionItem cloned = (FunctionItem)Activator.CreateInstance(this.GetType());
			foreach (FormulaItem argument in Arguments) {
				FormulaItem clonedArg = (FormulaItem)argument.Clone(values);
				cloned.Arguments.Add(clonedArg);
			}

			return cloned;

		}
	}
}
