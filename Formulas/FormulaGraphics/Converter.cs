using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class Converter
	{

		

		public Converter()
		{
		
		}


		public Element Convert(CalculationItem item)
		{

			
			if (item is EquationBlock) {
				EquationBlock bl = (EquationBlock)item;

				EquationBlockElement blockElement = new EquationBlockElement();
				foreach (CalculationItem eq in bl.Equations) {

					Element eqElem = (Element)Convert(eq);
					blockElement.Arguments.Add(eqElem);
				}

				return blockElement;
			}

			if (item is Equation) {

				Equation eq = (Equation)item;

				EquationElement eqElem = new EquationElement();
				if (null != eq.Symbol) eqElem.Arguments.Add(Convert(eq.Symbol));
				if (null != eq.Symbolic) eqElem.Arguments.Add(Convert(eq.Symbolic));
				if (null != eq.Numbers) eqElem.Arguments.Add(Convert(eq.Numbers));
				if (null != eq.Value) eqElem.Arguments.Add(Convert(eq.Value));
				return eqElem;

			}

			return null;

		}


		public Element Convert(FormulaItem item)
		{

			
			

			Element element = null;

			if (item is ValueItem) {

				ValueItem i = (ValueItem)item;
				ValueElement e = new ValueElement();
				e.Value = i.Value.ToString();
				element = e;
			}

			if (item is SymbolItem) {

				SymbolItem i = (SymbolItem)item;
				SymbolElement e = new SymbolElement();

				if (!string.IsNullOrEmpty(i.Symbol)) {
					string[] splited = i.Symbol.Split('.');
					e.Symbol = splited[0];
					if (splited.Length == 2)
						e.Suffix = splited[1];
				}

				element = e;
			}

			if (item is UnitItem) {

				UnitItem u = (UnitItem)item;
				UnitElement e = new UnitElement();
				e.Unit = u.Unit;
				element = e;
			}

			

			if (item is FunctionSum) {

				FunctionSum sum = (FunctionSum)item;
				SumElement sumElem = new SumElement();

				for (int i = 0 ; i < sum.Coeffs.Count ; ++i) {
					if (i == 0)
					{
						sumElem.Signs.Add(string.Empty);
						continue;
					}
					double coeff = sum.Coeffs[i];
					string sign = (coeff > 0) ? "+" : "-";
					sumElem.Signs.Add(sign);
				}

				element = sumElem;
			}

			if (item is FunctionMultiply) {

				FunctionMultiply multi = (FunctionMultiply)item;

				if (multi.Arguments.Count == 2 && multi.Flags[1] == false) {
					FractionElement fraction = new FractionElement();
					element = fraction;
				} else {

					SumElement sumElem = new SumElement();

					for (int i = 0 ; i < multi.Flags.Count ; ++i) {
						if (i == 0) {
							sumElem.Signs.Add(string.Empty);
							continue;
						}
						bool flag = multi.Flags[i];
						string sign = flag ? "x" : "/";
						sumElem.Signs.Add(sign);
					}

					element = sumElem;

				}

				
			}

			if (item is FunctionSqrt) {
				element = new SqrtElement();
			}

			if (item is FunctionPow) {
				element = new PowElement();
			}

			if (item is FunctionMax) {
				element = createElement("max");
			}

			if (item is FunctionMin) {
				element = createElement("min");
			}

			if (item is FunctionSin) {
				element = createElement("sin");
			}

			if (item is FunctionCos) {
				element = createElement("cos");
			}

			if (item is FunctionTan) {
				element = createElement("tan");
			}


			if (item is BracketItem) {

				element = createElement("");
			}



			if (item is FunctionItem) {

				FunctionItem function = (FunctionItem)item;
				foreach (FormulaItem arg in function.Arguments) {

					Element argElement = Convert(arg);
					element.Arguments.Add(argElement);
				}
			}

			return element;
		}

		FunctionElement createElement(string name)
		{
			FunctionElement function = new FunctionElement();
			function.FunctionName = name;
			return function;
		}
	
	}
}
