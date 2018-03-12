using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formulas
{
	public class RegexTools
	{

		// np REGEX = @"^(?<fun>.*)\((?<body>.*)\)$";
		// grupę oznaczymy (?<fun>_____)
		// .* - dowolny ciąg znaków
		// \( - escape dla nawiasu
		public static string GetValue(string input, string regex, string groupName)
		{
			
			Match match = Regex.Match(input, regex);
			if (null == match)
				return null;

			Group funGroup = match.Groups[groupName];
			if (null == funGroup || (!funGroup.Success))
				return null;

			string functionName = funGroup.Value;
			return functionName;

			

		}


		


	
	}
}
