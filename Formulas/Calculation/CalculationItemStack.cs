using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class CalculationItemStack
	{

		public CalculationItemStack()
		{
			Items = new List<CalculationItem>();
		}

		public List<CalculationItem> Items { get; set; }

		public List<SymbolValuePair> GetAsSymbolValueList()
		{
			List<SymbolValuePair> collection = new List<SymbolValuePair>();

			List<Equation> equations = Items
						.OfType<Equation>()
						.Where(e => e.Symbol != null)
						.ToList();

			equations.ForEach(i => collection.Add(i.GetSymbolSymbolValuePair()));

			collection.Reverse();

			return collection;
		}

	}

}
