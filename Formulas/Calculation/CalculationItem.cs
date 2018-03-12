using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public abstract class CalculationItem : BaseItem
	{

		public CalculationItem()
		{
			
		}


		public abstract void Calculate(CalculationItemStack calculationStack, bool isToAdd = true);

	

	
	}
}
