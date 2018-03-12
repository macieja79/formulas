using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class BracketItem : FunctionItem
	{

	


		public BracketShapeType TypeOfBracket { get; set; }

		public override double GetValue()
		{
			return Arguments.First().GetValue();
		}

	}
}
