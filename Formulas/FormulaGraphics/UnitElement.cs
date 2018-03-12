using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class UnitElement : Element
	{

		public string Unit { get; set; }

		public override void Paint(IGraphics g, System.Drawing.PointF point)
		{
			g.DrawString(Unit, point);
		}

		public override BoundaryInfo GetBoundary(IGraphics g)
		{

			SizeF size = g.MeasureString(Unit);
			return new BoundaryInfo
			{
				Size = size,
				BaseLine = size.Height
			};


		}
	}
}
