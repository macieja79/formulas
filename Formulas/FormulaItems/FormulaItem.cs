using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public abstract class FormulaItem
    {
        public string Pattern { get; set; }

        // czy bedzie potrzebne?
        public FormulaItem Parent { get; set; }

        public abstract double GetValue();

        public int Level
        {
            get
            {

                int level = 1;
                FormulaItem parent = Parent;
                while (parent != null)
                {
                    level++;
                    parent = parent.Parent;
                }

                return level;

            }
        }

		public virtual bool IsSymbol
		{
			get
			{
				return false;
			}
		}

		public abstract FormulaItem Clone(List<SymbolValuePair> values);
		



		



		public override string ToString()
		{
			return Pattern;
		}


	}

}
