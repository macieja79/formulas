using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formulas
{


	public abstract class CalculationPattern
	{

		public abstract bool TryCreate(List<string> lines, CalculationBuilder builder, out CalculationItem item);

	}


	public class EquationPattern : CalculationPattern
	{

		string REGEX_PATTERN = @"^(?<left>.+)=(?<right>.+)$";
		FormulaBuilder _formulaBuilder = new FormulaBuilder();

		public override bool TryCreate(List<string> lines, CalculationBuilder builder, out CalculationItem item)
		{

			string pattern = lines.First();

			bool isMatch = Regex.IsMatch(pattern, REGEX_PATTERN);
			if (!isMatch) {


				FormulaItem supposedFormula = _formulaBuilder.Create(pattern);
				if (supposedFormula is ValueItem) {
					Equation iEquation = new Equation();
					iEquation.Value = (ValueItem)supposedFormula;
					item = iEquation;
					builder.RemoveLines(lines, 1);
					return true;
				}

				if (supposedFormula is SymbolItem) {
					Equation iEquation = new Equation();
					iEquation.Symbol = (SymbolItem)supposedFormula;
					item = iEquation;
					builder.RemoveLines(lines, 1);
					return true;
				}

				item = null;
				return false;
			}

			string left = RegexTools.GetValue(pattern, REGEX_PATTERN, "left");
			string right = RegexTools.GetValue(pattern, REGEX_PATTERN, "right");

			FormulaItem symbol = _formulaBuilder.Create(left);
			FormulaItem formula = _formulaBuilder.Create(right);

			Equation equation = new Equation();
			equation.Symbol = (SymbolItem)symbol;

			if (formula is ValueItem) {
				equation.Value = (ValueItem)formula;
			} else {
				if (formula.IsSymbol) {
					equation.Symbolic = formula;
				} else {
					equation.Numbers = formula;
				}
			}

			item = equation;

			builder.RemoveLines(lines, 1);

			return true;
		}
	}

	public class IfPattern : CalculationPattern
	{

		string REGEX_PATTERN = @"^if\((?<left>.+)(?<sign>==|>=|<=|<|>)(?<right>.+)\)$";


		public override bool TryCreate(List<string> lines, CalculationBuilder builder, out CalculationItem item)
		{
			
			string pattern = lines.First();

			bool isMatch = Regex.IsMatch(pattern, REGEX_PATTERN);
			if (!isMatch) {
				item = null;
				return false;
			}

			string left = RegexTools.GetValue(pattern, REGEX_PATTERN, "left");
			string right = RegexTools.GetValue(pattern, REGEX_PATTERN, "right");
			string sign = RegexTools.GetValue(pattern, REGEX_PATTERN, "sign");


			If ifStatement = new If();
			Condition condition = new Condition();
			List<string> leftList = new List<string>();
			leftList.Add(left);
			condition.Left = (Equation)builder.Create(leftList);
			List<string> rightList = new List<string>();
			rightList.Add(right);
			condition.Right = (Equation)builder.Create(rightList);
			condition.TypeOfCondition = getType(sign);

			ifStatement.Condition = condition;
			
			builder.RemoveLines(lines, 1);
			ifStatement.TrueBlock = (EquationBlock)builder.Create(lines);

			string nextLine = lines.First();
			if (nextLine == "else") {

				builder.RemoveLines(lines, 1);
				ifStatement.FalseBlock = (EquationBlock)builder.Create(lines);
			}


			item = ifStatement;
			return true;
			

		}


		ConditionType getType(string sign)
		{
			if (sign == "==") return ConditionType.Equals;
			if (sign == ">") return ConditionType.Greater;
			if (sign == ">=") return ConditionType.GreaterOrEqual;
			if (sign == "<") return ConditionType.Less;
			if (sign == "<=") return ConditionType.LessOrEqual;

			throw new Exception("Bad sign");
		}


	}

	public class EquationBlockPattern : CalculationPattern
	{
		public override bool TryCreate(List<string> lines, CalculationBuilder builder, out CalculationItem item)
		{




			bool isMatch = (lines.First() == "{");

			if (!isMatch) {
				item = null;
				return false;
			}


			EquationBlock block = new EquationBlock();

			int level = 0;
			builder.RemoveLines(lines, 1);
			for ( ; ; ) {

				
				CalculationItem eq = builder.Create(lines);
				block.Equations.Add(eq);

				string currentLine = lines.First();
				if (currentLine == "}") {
					if (level == 0) {
						break;
					} else {
						level--;
					}
				}

				


			}

			builder.RemoveLines(lines, 1);
			item = block;
			return true;

			
		}
	}





	public class CalculationBuilder
    {

		List<CalculationPattern> _patterns = new List<CalculationPattern>();

		public CalculationBuilder()
		{
			initPatterns();
		}

		void initPatterns()
		{
			_patterns.Add(new EquationBlockPattern());
			_patterns.Add(new IfPattern());
			_patterns.Add(new EquationPattern());
		}

	


		public CalculationItem Create(List<string> lines)
		{

			CalculationItem item = null;

			while (lines.Count > 0) {

				

				foreach (CalculationPattern pattern in _patterns) {

					if (!pattern.TryCreate(lines, this, out item))
						continue;

					return item;
				}
			}

			return null;
		
		}
		
		public void RemoveLines(List<string> lines, int numberOfLines)
		{
			lines.RemoveRange(0, numberOfLines);
		}

		public int GetLineNumber(List<string> lines, string pattern)
		{
			for (int i = 0 ; i < lines.Count ; ++i) {

				if (lines[i] == pattern)
					return i;
			}

			return -1;
		}


		public List<string> GetLines(List<string> lines, int firstNumber, int lastNumber)
		{
			int count = lastNumber - firstNumber+1;
			return lines.GetRange(firstNumber, count);
		}








		public List<string> getCleaned(List<string> lines)
		{

			List<string> cleaned = new List<string>();
			foreach (string line in lines) {

				if (string.IsNullOrEmpty(line))
					continue;

				cleaned.Add(line.Trim());
			}

			return cleaned;

		}
    
    }


}
