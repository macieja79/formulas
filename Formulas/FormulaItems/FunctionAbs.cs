﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class FunctionAbs : FunctionItem
    {
		
        public override double GetValue()
        {
            FormulaItem value = Arguments.First();
            double arg = value.GetValue();
            double result = Math.Abs(arg);
            return result;
        }
    }
}
