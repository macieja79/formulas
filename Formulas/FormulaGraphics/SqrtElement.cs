using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Formulas
{
	public class SqrtElement : Element
	{

		const float DIST = 3f;

		public override void Paint(IGraphics g, PointF point)
		{

			Element elem = Arguments.First();
			

			BoundaryInfo info = elem.GetBoundary(g);
			g.PaintBoundary(info, point);

			PointF p1 = new PointF(point.X, point.Y + info.Height - DIST);
			PointF p2 = new PointF(point.X + DIST, point.Y + info.Height);
			PointF p3 = new PointF(point.X + 2 * DIST, point.Y);
			PointF p4 = new PointF(point.X + 3 * DIST + info.Width, point.Y);

			g.DrawLine(p1, p2);
			g.DrawLine(p2, p3);
			g.DrawLine(p3, p4);
			
			PointF argPoint = new PointF(point.X + 3 * DIST, point.Y + DIST);
			elem.Paint(g, argPoint);

		}

		public override BoundaryInfo GetBoundary(IGraphics g)
		{

			Element elem = Arguments.First();
			BoundaryInfo info = elem.GetBoundary(g);

			SizeF rect = new SizeF(info.Width + 3 * DIST, info.Height + 2 * DIST); 
			float baseLine = info.BaseLine + DIST;

			return new BoundaryInfo
			{
				Size = rect,
				BaseLine = baseLine
			};
		}
	}
}
