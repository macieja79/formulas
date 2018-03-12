using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class EquationBlock : CalculationItem
	{
		public EquationBlock()
		{
			Equations = new List<CalculationItem>();
		}

		public List<CalculationItem> Equations { get; set; }

		public override void Calculate(CalculationItemStack calculationStack, bool isToAdd = true)
		{

			foreach (CalculationItem item in Equations) {

				item.Calculate(calculationStack);

			}
			
		}
	}
}
