using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class FractionElement : Element
	{
		public override void Paint(IGraphics g, System.Drawing.PointF point)
		{
			
			Element numerator = Arguments[0];
			Element denominator = Arguments[1];

			BoundaryInfo numeratorInfo = numerator.GetBoundary(g);
			BoundaryInfo denominatorInfo = denominator.GetBoundary(g);
			BoundaryInfo total = GetBoundary(g);

			float numeratorOffset = (total.Width - numeratorInfo.Width) * 0.5f;
			float denominatorOffset = (total.Width - denominatorInfo.Width) * 0.5f;

			PointF nPoint = new PointF(point.X + numeratorOffset, point.Y);
			numerator.Paint(g, nPoint);

			PointF dPoint = new PointF(point.X + denominatorOffset, point.Y + numeratorInfo.Height);
			denominator.Paint(g, dPoint);

			g.DrawLine(point.X, point.Y + numeratorInfo.Height, point.X + total.Width, point.Y + numeratorInfo.Height);

		}

		public override BoundaryInfo GetBoundary(IGraphics g)
		{

			Element numerator = Arguments[0];
			Element denominator = Arguments[1];

			BoundaryInfo numeratorInfo = numerator.GetBoundary(g);
			BoundaryInfo denominatorInfo = denominator.GetBoundary(g);

			SizeF sigleCharSize = g.MeasureString("+");

			float width = Math.Max(numeratorInfo.Width, denominatorInfo.Width);
			float height = numeratorInfo.Height + denominatorInfo.Height;
			float baseLine = numeratorInfo.Height + sigleCharSize.Height * 0.5f;

			return new BoundaryInfo
			{
				Size = new System.Drawing.SizeF(width, height),
				BaseLine = baseLine
			};



		}
	}
}
