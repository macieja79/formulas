using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class For : CalculationItem
	{
		public Equation Start { get; set; }
		public Condition Condition { get; set; }
		public Equation Equation { get; set; }
		public CalculationItem Body { get; set; }

		public override void Calculate(CalculationItemStack calculationStack, bool isToAdd = true)
		{
		
		}
	}
}
