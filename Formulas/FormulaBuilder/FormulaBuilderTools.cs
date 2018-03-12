using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	  public struct FunctionInfo
        {
            public string Name { get; set; }
            public string Arguments { get; set; }

            public bool IsOK
            {
                get
                {
                    return (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Arguments));
                }

            }

        }

	  public struct PowerInfo
        {

            public string Base { get; set; }
            public string Exponent { get; set; }

            public bool IsOk
            {
                get
                {
                    return (!string.IsNullOrEmpty(Base) && !string.IsNullOrEmpty(Exponent));
                }

            }
        }

	  public struct ValueInfo
	  {
		  public double Value { get; set; }
		  public string Unit { get; set; }


		  public bool IsOk
		  {
			  get
			  {
				  return (!(double.IsNaN(Value)));
			  }
		  }
	  }

	public class FormulaBuilderTools
	{

		#region <singleton>

		FormulaBuilderTools() { }

		static FormulaBuilderTools _instance;

		public static FormulaBuilderTools Instance
		{
			get
			{
				if (null == _instance)
					_instance = new FormulaBuilderTools();

				return _instance;
			}

		}

		#endregion

		#region <public>



		public  List<int> GetSingsPositions(string str, OperationType operation)
        {
            List<int> operators = new List<int>();
			
            int lenght = str.Length;

            //bool isInBracket = false;
            int bracketLevel = 0;
            int bracketStart = 0;



            for (int charPosition = 0; charPosition < lenght; charPosition++)
            {
                char c = str[charPosition];

                if (IsBracket(str, BracketType.Left, charPosition))
                {
                    bracketLevel++;
                    bracketStart = charPosition;
                }

                if (bracketLevel == 0)
                {

                    if (IsOperator(c, operation))
                    {

                        bool isOk = true;

                        if (c == '/' || c == 'x')
                        {
                            if (charPosition < lenght - 1 && charPosition > 0)
                            {
                                char nextChar = str[charPosition + 1];
                                char previousChar = str[charPosition - 1];

                                if (IsLetter(previousChar) && IsLetter(nextChar))
                                    isOk = false;


                            }
                        }

                        if (c == 'x')
                        {
                            if (charPosition > 0)
                            {
                                char previousChar = str[charPosition - 1];
                                if (char.IsLetter(previousChar)) isOk = false;
                            }

                        }


                        if (isOk)
                        {
                            operators.Add(charPosition);
                        }

                    }

                }
                else
                {

                    if (IsBracket(str, BracketType.Right, charPosition))
                    {
                        if (charPosition > bracketStart)
                        {
                            bracketLevel--;
                        }

                    }
                }

            }

            return operators;

        }
		
		public List<string> GetElements(string str, OperationType operation)
        {

            List<string> elements = new List<string>();
            List<int> positions = GetSingsPositions(str, operation);

            if (positions.Count == 0)
            {
                elements.Add(str);
            }
            else
            {
                int pos = positions[0];
                string element = str.Substring(0, pos);
                elements.Add(element);

                for (int i = 1; i <= positions.Count; i++)
                {

                    int k, k1;

                    if (i < positions.Count)
                    {
                        k = positions[i - 1];
                        k1 = positions[i];
                    }
                    else
                    {
                        k = positions[i - 1];
                        k1 = str.Length;
                    }

                    string sign = str.Substring(k, 1);
                    string elem = str.Substring(k + 1, k1 - k - 1);

                    elements.Add(sign);
                    elements.Add(elem);


                }

            }

            if (operation == OperationType.AddSubstr)
            {
                return GetElementsConverted(elements);

            }

            return elements;

        }

        public List<string> GetElementsConverted(List<string> elements)
        {
            List<string> cloned = elements.Select(e => e).ToList();

        start:
            int n = cloned.Count;
            for (int i = 0; i < n - 3; i++)
            {
                string el1 = cloned[i];
                string sep = cloned[i + 1];
                string el2 = cloned[i + 2];

                if (el1 == "-" && el2 == "-" && string.IsNullOrEmpty(sep.Trim()))
                {
                    cloned[i + 3] = "(-" + cloned[i + 3] + ")";
                    cloned.RemoveAt(i + 1);
                    cloned.RemoveAt(i + 1);
                    goto start;
                }
            }

            return cloned;
        }

		public FunctionInfo GetFunctionElements(string inpStr)
        {
            FunctionInfo f = new FunctionInfo();
			
            string str = inpStr.ToLower();
            str = str.Trim();

            int start = -1;
            int end = -1;
            for (int i = 0; i < str.Length; i++)
            {

                if (start < 0 && IsBracket(str, BracketType.Left, i))
                {
                    start = i;
                    continue;

                }

                if (IsBracket(str, BracketType.Right, i))
                {
                    end = i;

                }
            }

            if (start < 0) return f;
            if (end == -1) end = str.Length;

            f.Name = str.Substring(0, start);
            f.Arguments = str.Substring(start + 1, (end - start - 1));
            return f;
        }

		public PowerInfo GetPowerElements(string powerString)
        {
            PowerInfo p = new PowerInfo();

            string str = powerString;
            int firstIndexAfterPow = -1;

            if (str.Contains("pow"))
                firstIndexAfterPow = str.IndexOf("pow") + "pow".Length;

            if (str.Contains("Pow"))
                firstIndexAfterPow = str.IndexOf("Pow") + "Pow".Length;

            if (firstIndexAfterPow < 0 || firstIndexAfterPow >= str.Length)
                return p;

            str = str.Substring(firstIndexAfterPow);
            str = GetOutBrackets(str);

            int separatorIndex = -1;

            int bracketLevel = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '(')
                {
                    bracketLevel++;
                    continue;
                }

                if (str[i] == ')')
                {
                    bracketLevel--;
                    continue;
                }

                if (str[i] == ',' && bracketLevel == 0)
                {
                    separatorIndex = i;
                    break;
                }
            }




            if (separatorIndex < 0) return p;

            p.Base = str.Substring(0, separatorIndex);
            p.Exponent = str.Substring(separatorIndex + 1);
            return p;

        }

		#endregion

		#region <prv>
		bool IsOperator(char c, OperationType operation)
        {

            if (operation == OperationType.AddSubstr)
            {
                if (c == '+' || c == '-') return true;
            }
            else if (operation == OperationType.MultiDivide)
            {
                if (c == '/' || c == '*' || c == 'x') return true;
            } else if (operation == OperationType.Power) {

				if (c == '^')
					return true;
			}

            return false;
        }

        bool IsFormulaInBracket(string formula)
        {

            if (string.IsNullOrEmpty(formula))
                return false;

			
            string trimmedFormula = formula.Trim();
            int lenght = trimmedFormula.Length;

            char cL = trimmedFormula[0];
            char cR = trimmedFormula[lenght - 1];

            return (IsBracket(formula, BracketType.Left, 0) && IsBracket(formula, BracketType.Right, lenght - 1));
        }

        bool IsBracket(string pattern, BracketType bracket, int index)
        {

			char c = pattern[index];
			List<Item> straightBracketInfo = GetStraightBracketPositions(pattern);

			string leftBrackets = "({[";
			string rightBrackets = ")}]";


			if (leftBrackets.Contains(c)) {
				if (bracket == BracketType.Left)
					return true;
			}

			if (rightBrackets.Contains(c)){
				if (bracket == BracketType.Right)
					return true;
			}

			if (c == '|')
			{
				BracketType typeOfBracket = straightBracketInfo.FirstOrDefault(i => i.Pos == index).TypeOfBracket;
				return typeOfBracket == bracket;
			}

			return false;
        }

        bool IsLetter(char c)
        {
            int a = (int)c;
            return (a >= 65 && a <= 90) || (a >= 97 && a <= 122);
		}

		string GetOutBrackets(string formula)
        {
            formula = formula.Trim();
			
			if (IsBracket(formula, BracketType.Left, 0) && IsBracket(formula, BracketType.Right, formula.Length - 1))
            {
                int lenght = formula.Length;
                return formula.Substring(1, lenght - 2);
            }
            return formula;
        }

				
		class Item
		{
			public int Pos { get; set; }
			public BracketType TypeOfBracket { get; set; }
		}

		

		static List<Item> GetStraightBracketPositions(string pattern)
		{

			List<Item> dict = new List<Item>();

			for (int i = 0 ; i < pattern.Length ; ++i) {

				char c = pattern[i];

				if (c == '|')
					dict.Add(new Item { Pos = i });
			}

			
			
			for (int i = 0 ; i < dict.Count ; ++i) {
			
				// pierwszy jest open
				if (i == 0) {
					dict[i].TypeOfBracket = BracketType.Left;
					continue;
				}

				// ostatni jest close
				if (i == dict.Count - 1) {
					dict[dict.Count - 1].TypeOfBracket = BracketType.Right;
					continue;
				}

				int prevIndex = dict[i - 1].Pos;
				int curIndex = dict[i].Pos;
				int nextIndex = dict[i + 1].Pos;
				char[] operators = {'*', '/', '+', '-'};
				char[] neutral = {' '};

				// jezeli przed nawiasem jest operator, to znaczy ze jest open
				string prevFragment = StringTools.GetSubstring(pattern, prevIndex, curIndex, false);
				for (int j = prevFragment.Length - 1 ; j > 0 ; --j) {

					char c = prevFragment[j];

					if (char.IsLetterOrDigit(c) && (!(c == ' ')))
						break; 
					if (neutral.Contains(c))
						continue;
					if (operators.Contains(c)) {
						dict[i].TypeOfBracket = BracketType.Left;
					}
				}

				// jezeli za nawiasem jest operator, to znaczy ze jest close
				string nextFragment = StringTools.GetSubstring(pattern, curIndex, nextIndex, false);
				for (int j = 0 ; j < nextFragment.Length ; ++j) {

					char c = nextFragment[j];
					if (char.IsLetterOrDigit(c))
						break; 
					if (neutral.Contains(c))
						continue;
					if (operators.Contains(c)) {
						dict[i].TypeOfBracket = BracketType.Right;
					}
				}

			}

			return dict;

		}

		public ValueInfo GetValueInfo(string pattern)
		{

			string trimmed = pattern.Trim();

			int lenght = trimmed.Length;
			double value = double.NaN;

			int index = 0;
			bool isBroke = false;

			for (int i = 1 ; i <= lenght ; ++i) {

				string sub = trimmed.Substring(0, i);
				double tmpValue;
				if (double.TryParse(sub, out tmpValue)) {
					value = tmpValue;
				} else {
					index = i-1;
					isBroke = true;
					break;
				}


			}

			string unit = string.Empty;
			if (isBroke) {
				if (index < trimmed.Length) {
					unit = trimmed.Substring(index);
				}
			}

			ValueInfo info = new ValueInfo
			{
				Unit = unit,
				Value = value
			};

			return info;

			

			



		}

		#endregion


	}
}
