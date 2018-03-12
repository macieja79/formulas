using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formulas
{

	#region <builder>
	public class FormulaBuilder
    {

		List<FormulaPattern> _patterns = new List<FormulaPattern>();

		public FormulaBuilder()
		{
			initPatterns();
		}

		void initPatterns()
		{
		
			_patterns.Add(new AddSubstrPattern());
			_patterns.Add(new MultiplyDividePattern());
			_patterns.Add(new PowerPattern());
			_patterns.Add(new BracketPattern());
			_patterns.Add(new AbsPattern());
			_patterns.Add(new FunctionPattern());
			_patterns.Add(new SymbolPattern());
			_patterns.Add(new ValuePattern());
		}



		public FormulaItem Create(string strPattern)
		{

			FormulaItem item;

			foreach (FormulaPattern pattern in _patterns) {

				if (!pattern.TryCreate(strPattern, this, out item))
					continue;

				return item;

			}

			return null;


		}
    
    }




	
	#endregion

	#region <patterns>


	

	public abstract class FormulaPattern
    {

        #region <to override>
        
        public abstract bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item);

      
        
        #endregion

    }

	public class AddSubstrPattern : FormulaPattern
	{

		public override bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item)
		{

			item = null;

			List<string> elements = FormulaBuilderTools.Instance.GetElements(pattern, OperationType.AddSubstr);
			if (elements.Count < 3)
				return false;

			item = new FunctionSum();
			item.Pattern = pattern;

			for (int i = 0 ; i < elements.Count ; ++i) {

				string element = elements[i];
				if (element == "+" || element == "-")
					continue;


				double coeff = 1.0;
				if (i > 0) {
					coeff = (elements[i - 1] == "+") ? 1.0 : -1.0;
				}

				FormulaItem arg = builder.Create(element);
				(item as FunctionSum).Arguments.Add(arg);
				(item as FunctionSum).Coeffs.Add(coeff);

			}

			return true;

		}
	}

	public class MultiplyDividePattern : FormulaPattern
	{

		public override bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item)
		{

			item = null;

			List<string> elements = FormulaBuilderTools.Instance.GetElements(pattern, OperationType.MultiDivide);
			if (elements.Count < 3)
				return false;

			item = new FunctionMultiply();
			item.Pattern = pattern;

			for (int i = 0 ; i < elements.Count ; ++i) {

				string element = elements[i];
				if (element == "*" || element == "/")
					continue;


				bool  coeff = true;
				if (i > 0) {
					coeff = (elements[i - 1] == "*") ? true : false;
				}

				FormulaItem arg = builder.Create(element);
				(item as FunctionMultiply).Arguments.Add(arg);
				(item as FunctionMultiply).Flags.Add(coeff);

			}

			return true;

		}
	}

	public class ValuePattern : FormulaPattern
	{





		public override bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item)
		{
			item = null;

			string trimmed = pattern.Trim();

			ValueInfo valueInfo = FormulaBuilderTools.Instance.GetValueInfo(trimmed);
			if (!valueInfo.IsOk)
				return false;

			item = new ValueItem();
			item.Pattern = pattern;
			(item as ValueItem).Value = valueInfo.Value;

			UnitItem unit = new UnitItem();
			unit.Unit = valueInfo.Unit;
			unit.Pattern = valueInfo.Unit;
			(item as ValueItem).Arguments.Add(unit);
			


			return true;

		}

	}

	public class AbsPattern : FormulaPattern
	{

		string REGEX = @"^\|(?<args>.*)\|$";

		public override bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item)
		{

			item = null;
			pattern = pattern.Trim();
			string args = RegexTools.GetValue(pattern, REGEX, "args");
			if (string.IsNullOrEmpty(args)) return false;

			item = new FunctionAbs();
			item.Pattern = pattern;
			FormulaItem arg = builder.Create(args);
			(item as FunctionAbs).Arguments.Add(arg);

			return true;



		}
	}

	public class SymbolPattern : FormulaPattern
	{


		string REGEX = @"^[a-zA-Z]*\.[a-zA-Z0-9]*$";
		string REGEX2 = @"^[a-zA-Z]*$";

		public override bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item)
		{
			string trimmed = pattern.Trim();

			item = null;
			bool isSymbol = Regex.IsMatch(trimmed, REGEX) || Regex.IsMatch(trimmed, REGEX2);
			if (!isSymbol)
				return false;

			item = new SymbolItem();
			item.Pattern = pattern;
			(item as SymbolItem).Symbol = trimmed;

			return true;


		}
	}

	public class FunctionPattern : FormulaPattern
	{

		Dictionary<string, Type> _functions = new Dictionary<string, Type>();

		const char FUNCTION_ARGUMENT_SEPARATOR = ';';

		public FunctionPattern()
		{
			_functions["sin"] = typeof(FunctionSin);
			_functions["cos"] = typeof(FunctionCos);
			_functions["min"] = typeof(FunctionMin);
			_functions["max"] = typeof(FunctionMax);
			_functions["sqrt"] = typeof(FunctionSqrt);
			_functions["tan"] = typeof(FunctionTan);
		}


		public override bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item)
		{

			item = null;


			FunctionInfo functionInfo = FormulaBuilderTools.Instance.GetFunctionElements(pattern);
			if (!functionInfo.IsOK)
				return false; 

			string functionName = functionInfo.Name;
			if (!_functions.Keys.Contains(functionName))
				return false;

			Type functionType = _functions[functionName];
			item = (FunctionItem)Activator.CreateInstance(functionType);
			item.Pattern = pattern;

			string[] args = functionInfo.Arguments.Split(FUNCTION_ARGUMENT_SEPARATOR);

			foreach (string arg in args) {
				FormulaItem argItem = builder.Create(arg);
				(item as FunctionItem).Arguments.Add(argItem);
			}
			
			return true;
		}


	}

	public class PowerPattern : FormulaPattern
	{


		public override bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item)
		{

			item = null;

			List<string> elements = FormulaBuilderTools.Instance.GetElements(pattern, OperationType.Power);
			if (elements.Count != 3 || elements[1] != "^")
				return false;

			FormulaItem baseItem = builder.Create(elements[0]);
			FormulaItem exponentItem = builder.Create(elements[2]);

			item = new FunctionPow();
			item.Pattern = pattern;
			(item as FunctionItem).Arguments.Add(baseItem);
			(item as FunctionItem).Arguments.Add(exponentItem);

			return true;

		}
	}

	public class BracketPattern : FormulaPattern
	{


		string REGEX_ROUND = @"^\((?<args>.*)\)$";
		string REGEX_SQUARED = @"^\[(?<args>.*)\]$";
	
		public override bool TryCreate(string pattern, FormulaBuilder builder, out FormulaItem item)
		{
			item = null;
			string trimmed = pattern.Trim();

           

			bool isMatchRound = Regex.IsMatch(trimmed, REGEX_ROUND);
			bool isMatchSquared = Regex.IsMatch(trimmed, REGEX_SQUARED);
			bool isAnyMatch = isMatchRound || isMatchSquared;


            
			if (!isAnyMatch)
				return false;

			item = new BracketItem();
			item.Pattern = pattern;

			string args = null;
			if (isMatchRound)
				args = RegexTools.GetValue(pattern, REGEX_ROUND, "args");
			
			if (isMatchSquared)
				args = RegexTools.GetValue(pattern, REGEX_SQUARED, "args");

			FormulaItem argsItem = builder.Create(args);
			(item as BracketItem).Arguments.Add(argsItem);
			if (isMatchRound)
				(item as BracketItem).TypeOfBracket = BracketShapeType.Round;

			if (isMatchSquared)
				(item as BracketItem).TypeOfBracket = BracketShapeType.Squared;


			return true;

		}
	}

	

	#endregion

    
}
