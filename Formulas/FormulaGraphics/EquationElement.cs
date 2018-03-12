using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class EquationElement : Element
	{

	
		public override void Paint(IGraphics g, PointF point)
		{




			List<Element> elements = new List<Element>();
			for (int i = 0 ; i < Arguments.Count - 1 ; i++) {

				elements.Add(Arguments[i]);
				elements.Add(Element.Eq);
			}
			elements.Add(Arguments.Last());
		
			BoundaryInfo.PaintElements(g, point, elements);

		}

		public override BoundaryInfo GetBoundary(IGraphics g)
		{

			List<Element> elements = new List<Element>();
			for (int i = 0 ; i < Arguments.Count - 1 ; i++) {

				elements.Add(Arguments[i]);
				elements.Add(Element.Eq);
			}
			elements.Add(Arguments.Last());

			BoundaryInfo info = BoundaryInfo.GetElementsBoundary(g, elements);
			return info;

		}
	}
}
