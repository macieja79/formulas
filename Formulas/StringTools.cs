using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class StringTools
	{


		public static string GetSubstring(string str, int start, int end, bool withIndexes)
		{

			int length = str.Length;
			if (withIndexes) {
				int i1 = start;
				int l = end - start + 1;
				return str.Substring(i1, l);
			} else {
				int i1 = start + 1;
				int l = end - start - 1;
				return str.Substring(i1, l);
			}



		}
	
	}
}
