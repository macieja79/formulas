using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Common.CS;

namespace Formulas
{
    public static class MtFormulaParser
    {

        #region <nested types>
        public struct PowerElements
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
        struct FunctionElements
        {
            public string Name { get; set; }
            public string Arguments { get; set; }

            bool IsOK
            {
                get
                {
                    return (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Arguments));
                }

            }

        }
        #endregion

        #region <enums>
        enum BracketType
        {
            Left,
            Right
        }

        enum OperationType
        {
            AddSubstr,
            MultiDivide
        }

        enum FunctionType
        {
            Min,
            Max
        }
        #endregion

        #region <public>
        public static double Parse(string formula)
        {
            return GetResult(formula, OperationType.AddSubstr);

        }

        public static string GetWithoutZeros(string formula)
        {
            return GetWithoutZeros(formula, OperationType.AddSubstr);
        }

        // method used in MtHtmlSupport as public
        public static PowerElements GetPowerElements(string powerString)
        {
            PowerElements p = new PowerElements();

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

        #region <get/set methods>

        static double GetResult(string formula, OperationType operation)
        {

            double result = 0.0;
            List<string> elements = GetElements(formula, operation);
            string firstElement;

            formula = formula.Trim();

            if (formula.Length == 0)
                return result;

            int nElements = elements.Count;
            int nOperators = (nElements - 1) / 2;

            if (nElements > 0)
            {
                firstElement = elements.First();

                if (operation == OperationType.MultiDivide)
                    result = GetValue(firstElement);
                else
                    result = GetResult(firstElement, OperationType.MultiDivide);

                for (int i = 0; i < nOperators; i++)
                {
                    string strOperator = elements[i * 2 + 1];
                    string nextElement = elements[i * 2 + 2];
                    double nextValue = 0.0;

                    if (operation == OperationType.MultiDivide)
                        nextValue = GetValue(nextElement);
                    else
                        nextValue = GetResult(nextElement, OperationType.MultiDivide);

                    GetCalculation(ref result, strOperator, nextValue);


                }

            }

            return result;





        }

        static double GetValue(string str)
        {
            str = str.Trim();

            string outbracketed;
            int n = str.Length;

            if (IsMin(str)) return GetMin(str);

            if (IsMax(str)) return GetMax(str);

            if (IsPower(str)) return GetPower(str);

            if (IsSqrt(str)) return GetSqrt(str);

            if (IsTrigonometric(str)) return GetTrigonometric(str);

            if (IsAbsolute(str)) return GetAbsolute(str);

            if (IsFormulaInBracket(str))
            {
                outbracketed = GetOutBrackets(str);
                return Parse(outbracketed);
            }

            for (int i = n; i > 0; i--)
            {
                string rightString = str.Substring(0, i);
                string unit = "";
                if (i < n)
                    unit = str.Substring(i);

                double parsed = double.NaN;

                if (double.TryParse(rightString, out parsed))
                {
                    double coeff = GetUnitCoeff(unit);
                    return parsed * coeff;
                }

            }

            return double.NaN;

        }

        static List<int> GetSingsPositions(string str, OperationType operation)
        {
            List<int> operators = new List<int>();

            int lenght = str.Length;

            //bool isInBracket = false;
            int bracketLevel = 0;
            int bracketStart = 0;

            for (int charPosition = 0; charPosition < lenght; charPosition++)
            {
                char c = str[charPosition];

                if (IsBracket(c, BracketType.Left))
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

                    if (IsBracket(c, BracketType.Right))
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

        static List<int> GetSeparatorPositions(string str, char separator)
        {
            List<int> separators = new List<int>();

            int lenght = str.Length;

            int bracketLevel = 0;

            for (int charPosition = 0; charPosition < lenght; charPosition++)
            {
                char c = str[charPosition];

                if (IsBracket(c, BracketType.Left))
                    bracketLevel++;

                if (bracketLevel == 0)
                {
                    if (c == separator)
                        separators.Add(charPosition);
                }
                else
                {
                    if (IsBracket(c, BracketType.Right))
                        bracketLevel--;
                }
            }

            return separators;
        }

        static List<string> GetElements(string str, OperationType operation)
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

        static List<string> GetElementsConverted(List<string> elements)
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

        static List<string> GetArgumentsSplited(string inpStr)
        {

            string str = GetOutBrackets(inpStr);
            List<int> separatorPositions = GetSeparatorPositions(str, ',');

            List<string> arguments = new List<string>();

            int start = 0;
            int end = -1;

            for (int i = 0; i < separatorPositions.Count; i++)
            {
                if (end >= 0)
                    start = end + 2;

                end = separatorPositions[i] - 1;

                string arg = str.Substring(start, (end - start + 1));
                arguments.Add(arg);

            }

            string lastArg = str.Substring(end + 2);
            arguments.Add(lastArg);

            return arguments;

        }

        static List<double> Parse(List<string> strings)
        {
            List<double> doubles = new List<double>();
            foreach (string str in strings)
            {
                double d = Parse(str);
                doubles.Add(d);
            }
            return doubles;
        }

        static FunctionElements GetFunctionElements(string inpStr)
        {
            FunctionElements f = new FunctionElements();

            string str = inpStr.ToLower();
            str = str.Trim();

            int start = -1;
            int end = -1;
            for (int i = 0; i < str.Length; i++)
            {

                if (start < 0 && IsBracket(str[i], BracketType.Left))
                {
                    start = i;
                    continue;

                }

                if (IsBracket(str[i], BracketType.Right))
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



        static void GetCalculation(ref double value1, string strOperator, double value2)
        {
            switch (strOperator)
            {
                case "+":
                    value1 = value1 + value2;
                    break;
                case "-":
                    value1 = value1 - value2;
                    break;
                case "*":
                case "x":
                    value1 = value1 * value2;
                    break;
                case "/":
                    value1 = value1 / value2;
                    break;
            }

        }

        static double GetPower(string strInp)
        {

            string str = strInp;

            PowerElements pow = GetPowerElements(str);

            double baseValue = 0;
            double exponentValue = 1.0;

            baseValue = Parse(pow.Base);
            exponentValue = Parse(pow.Exponent);

            return Math.Pow(baseValue, exponentValue);

        }

        static double GetSqrt(string str)
        {

            FunctionElements f = GetFunctionElements(str);

            double value = Parse(f.Arguments);

            return Math.Sqrt(value);
        }

        static double GetAbsolute(string str)
        {

            FunctionElements f = GetFunctionElements(str);


            string valueToEvaluate = string.Empty;
            if (string.IsNullOrEmpty(f.Name))
                valueToEvaluate = "(" + f.Arguments + ")";
            else
                valueToEvaluate = f.Arguments;

            double value = Parse(valueToEvaluate);

            return Math.Abs(value);
        }

        static double GetTrigonometric(string str)
        {

            FunctionElements f = GetFunctionElements(str);

            double value = Parse(f.Arguments);
            double angleInRad = (value / 180.0) * Math.PI;

            switch (f.Name)
            {
                case "sin":
                    return Math.Sin(angleInRad);
                case "cos":
                    return Math.Cos(angleInRad);
                case "tan":
                    return Math.Tan(angleInRad);
                case "cot":
                case "ctg":
                    return 1 / Math.Tan(angleInRad);
                case "asin":
                    return Math.Asin(value);
                case "atan":
                    return Math.Atan(value);
            }



            return double.NaN;

        }

        static double GetMin(string inpStr)
        {
            FunctionElements f = GetFunctionElements(inpStr);
            List<string> arguments = GetArgumentsSplited(f.Arguments);
            List<double> doubles = Parse(arguments);

            double min = double.NaN;
            foreach (double d in doubles)
            {
                if (double.IsNaN(min))
                    min = d;
                else
                {
                    if (d < min)
                        min = d;

                }
            }
            return min;
        }

        static double GetMax(string inpStr)
        {

            FunctionElements f = GetFunctionElements(inpStr);
            List<string> arguments = GetArgumentsSplited(f.Arguments);
            List<double> doubles = Parse(arguments);

            double max = double.NaN;
            foreach (double d in doubles)
            {
                if (double.IsNaN(max))
                    max = d;
                else
                {
                    if (d > max)
                        max = d;

                }
            }
            return max;
        }

        static double GetUnitCoeff(string str)
        {
            str = str.Trim();

            switch (str)
            {
                case "cm": return 0.01;
                case "cm2": return 0.0001;
                case "cm3": return 0.000001;
                case "cm4": return 0.00000001;
                case "mm": return 0.001;
                case "kg": return 0.01;
                case "Pa": return 0.001;
                case "MPa": return 1000;
                case "GPa": return 1000000;
                case "%": return 0.01;
            }

            return 1;

        }

        static string GetOutBrackets(string formula)
        {
            formula = formula.Trim();
            if (IsBracket(formula[0], BracketType.Left) && IsBracket(formula[formula.Length - 1], BracketType.Right))
            {
                int lenght = formula.Length;
                return formula.Substring(1, lenght - 2);
            }
            return formula;
        }

        static string GetWithoutZeros(string formula, OperationType operation)
        {

            double result = 0.0;
            List<string> elements = GetElements(formula, operation);
            string firstElement;

            formula = formula.Trim();

            if (formula.Length == 0)
                return string.Empty;

            int nElements = elements.Count;
            int nOperators = (nElements - 1) / 2;

            if (nElements > 0)
            {
                firstElement = elements.First();

                if (operation == OperationType.MultiDivide)
                    result = GetValue(firstElement);
                else
                {
                    if (!string.IsNullOrEmpty(firstElement))
                    {
                        result = GetResult(firstElement, OperationType.MultiDivide);
                        if (MtMath.isZero(result, 0.0000001))
                        {
                            elements.RemoveAt(0);
                            return GetWithoutZeros(string.Join("", elements.ToArray()), operation);

                        }
                    }

                }

                for (int i = 0; i < nOperators; i++)
                {
                    string strOperator = elements[i * 2 + 1];
                    string nextElement = elements[i * 2 + 2];
                    double nextValue = 0.0;

                    if (operation == OperationType.MultiDivide)
                        nextValue = GetValue(nextElement);
                    else
                    {
                        nextValue = GetResult(nextElement, OperationType.MultiDivide);
                        if (MtMath.isZero(nextValue, 0.0000001))
                        {
                            elements.RemoveAt(i * 2 + 1);
                            elements.RemoveAt(i * 2 + 1);
                            return GetWithoutZeros(string.Join("", elements.ToArray()));

                        }

                    }




                }

            }

            return formula;





        }




        #endregion

        #region <testing methods>

        static bool IsOperator(char c, OperationType operation)
        {

            if (operation == OperationType.AddSubstr)
            {
                if (c == '+' || c == '-') return true;
            }
            else if (operation == OperationType.MultiDivide)
            {
                if (c == '/' || c == '*' || c == 'x') return true;
            }

            return false;
        }

        static bool IsFormulaInBracket(string formula)
        {

            if (string.IsNullOrEmpty(formula))
                return false;

            string trimmedFormula = formula.Trim();
            int lenght = trimmedFormula.Length;

            char cL = trimmedFormula[0];
            char cR = trimmedFormula[lenght - 1];

            return (IsBracket(cL, BracketType.Left) && IsBracket(cR, BracketType.Right));
        }

        static bool IsBracket(char c, BracketType bracket)
        {
            return ((bracket == BracketType.Left && (c == '(' || c == '[' || c == '|' || c == '{')) ||
                     bracket == BracketType.Right && (c == ')' || c == ']' || c == '|' || c == '}'));
        }

        static bool IsLetter(char c)
        {
            int a = (int)c;
            return (a >= 65 && a <= 90) || (a >= 97 && a <= 122);
        }

        static bool IsTrigonometric(string str)
        {

            string[] functions = { "sin", "cos", "tan", "cot", "ctg", "atan" };

            string trimmed = str.Trim();

            foreach (string f in functions)
            {
                if (trimmed.Length >= f.Length && f == trimmed.Substring(0, f.Length))
                    return true;
            }

            return false;

        }

        static bool IsPower(string str)
        {
            string trimmed = str.Trim();

            if (trimmed.Length < 3) return false;

            string function = str.Substring(0, 3);

            return (function.ToLower() == "pow");

        }

        static bool IsSqrt(string str)
        {
            string trimmed = str.Trim();

            if (trimmed.Length < 4) return false;

            string function = str.Substring(0, 4);

            return (function.ToLower() == "sqrt");

        }

        static bool IsAbsolute(string str)
        {
            string trimmed = str.Trim();

            if (trimmed.Length < 3) return false;

            if (str.Length >= 3)
            {
                string function = str.Substring(0, 3);
                if (function.ToLower() == "abs")
                    return true;
            }

            char first = str[0];
            char last = str[str.Length - 1];

            if (first == '|' && last == '|') return true;

            return false;


        }

        static bool IsMin(string str)
        {
            FunctionElements f = GetFunctionElements(str);
            return (f.Name == "min");
        }

        static bool IsMax(string str)
        {
            FunctionElements f = GetFunctionElements(str);
            return (f.Name == "max");
        }

        #endregion


    }
}
