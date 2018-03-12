using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public abstract class MultiArgsElement : Element
	{
		public abstract override void Paint(IGraphics g, System.Drawing.PointF point);

		public abstract override BoundaryInfo GetBoundary(IGraphics g);


		protected BoundaryInfo GetArgumentsBoundary(IGraphics g, List<string> Signs)
		{

			List<BoundaryInfo> boundaries = Arguments.Select(a => a.GetBoundary(g)).ToList();
			
			float aboveBaseline = boundaries.Select(b => b.BaseLine).Max();
			float underBaseline = boundaries.Select(b => b.UnderBase).Max();

			float width = boundaries.Select(b => b.Width).Sum();
			float height = aboveBaseline + underBaseline;

			float baseLine = aboveBaseline;


			for (int i = 1 ; i < Signs.Count; ++i) {
				string sign = Signs[i];

				if (string.IsNullOrEmpty(sign))
					continue;

				SizeF size = g.MeasureString(sign);
				width += size.Width;
			}

			return new BoundaryInfo
			{
				Size = new SizeF(width, height),
				BaseLine = baseLine
			};
		}

		protected void PaintArguments(IGraphics g, PointF point, List<string> Signs)
		{
			BoundaryInfo boundary = GetArgumentsBoundary(g, Signs);
			g.PaintBoundary(boundary, point);

			float ix = point.X;

			for (int i = 0 ; i < Arguments.Count ; ++i) {

				Element argument = Arguments[i];
				BoundaryInfo iBoundary = argument.GetBoundary(g);

				float y0 = point.Y + (boundary.BaseLine - iBoundary.BaseLine);
				PointF iPoint = new PointF(ix, y0);

				argument.Paint(g, iPoint);

				ix += iBoundary.Width;

				if (i < Arguments.Count - 1) {

					string sign = Signs[i + 1];
					SizeF size = g.MeasureString(sign);
					PointF sPoint = new PointF(ix, point.Y + boundary.BaseLine - size.Height);

					g.DrawString(sign, sPoint);

					ix += size.Width;
				}


			}
		}


		
	}
}
