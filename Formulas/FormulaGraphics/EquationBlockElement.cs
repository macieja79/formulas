using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class EquationBlockElement : Element
	{
	
	
		public override void Paint(IGraphics g, PointF point)
		{
			float dY =  point.Y;
			foreach (EquationElement element in Arguments) {

				PointF iPoint = new PointF(point.X, dY);
				element.Paint(g, iPoint);
				BoundaryInfo iBounds = element.GetBoundary(g);
				dY += iBounds.Height;
			}

		}

		public override BoundaryInfo GetBoundary(IGraphics g)
		{
			BoundaryInfo[] boundaries = Arguments.Select(e => e.GetBoundary(g)).ToArray();

			float height = BoundaryInfo.GetHeightSummed(boundaries);
			float width = BoundaryInfo.GetWidthMax(boundaries);

			return new BoundaryInfo()
			{
				Size = new System.Drawing.SizeF { Height = height, Width = width },
				BaseLine = height
			};
		}
	}
}
