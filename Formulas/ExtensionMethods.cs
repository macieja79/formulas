using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public static class ExtensionMethods
	{

		public static void AddWhenNotNull<T>(this List<T> collection, params T[] elements)
		{

			foreach (T element in elements) {
				if (element != null)
					collection.Add(element);
			}
		}
	
	}
}
