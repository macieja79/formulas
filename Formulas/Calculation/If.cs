using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class If : CalculationItem
	{

		public Condition Condition { get; set; }
		public EquationBlock TrueBlock { get; set; }
		public EquationBlock FalseBlock { get; set; }

		public override void Calculate(CalculationItemStack calculationStack, bool isToAdd = true)
		{

			Condition.Calculate(calculationStack, false);

			if (Condition.IsTrue.Value) {

				TrueBlock.Calculate(calculationStack);
			}
			else
			{
				if (null != FalseBlock) {

					FalseBlock.Calculate(calculationStack);
				}
			}

		}
	}

}
