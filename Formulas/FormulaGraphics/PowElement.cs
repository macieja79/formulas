using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Formulas
{
	public class PowElement : Element
	{






		public override void Paint(IGraphics g, PointF point)
		{
	
			Element powBase = Arguments[0];
			Element exponent = Arguments[1];

			BoundaryInfo allInfo = GetBoundary(g);
			g.PaintBoundary(allInfo, point);

			BoundaryInfo baseInfo = powBase.GetBoundary(g);
			BoundaryInfo exponentInfo = exponent.GetBoundary(g);

			float height = allInfo.Height - baseInfo.Height;
			powBase.Paint(g, new PointF(point.X, point.Y + height));

			exponent.Paint(g, new PointF(point.X + baseInfo.Width, point.Y));

		}


		public override BoundaryInfo GetBoundary(IGraphics g)
		{
			Element powBase = Arguments[0];
			Element exponent = Arguments[1];

			BoundaryInfo baseInfo = powBase.GetBoundary(g);
			BoundaryInfo exponentInfo = exponent.GetBoundary(g);
			
			float width = baseInfo.Width + exponentInfo.Width;
			float height = (baseInfo.Height * 0.5f + exponentInfo.Height);

			SizeF rectangle = new SizeF(width, height);
			float baseLine = height;

			return new BoundaryInfo
			{
				Size = rectangle,
				BaseLine = baseLine
			};
		}
	}
}
