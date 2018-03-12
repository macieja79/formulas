using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class ValueElement : Element
	{


		public string Value { get; set; }



		public override void Paint(IGraphics g, PointF point)
		{

					g.DrawString(Value, point);

			if (!HasArguments) {
		
				return;
			}

			SizeF valueSize = g.MeasureString(Value);

			UnitElement unit = (UnitElement)Arguments.First();
			BoundaryInfo unitBounds = unit.GetBoundary(g);

			PointF unitPoint = new PointF(point.X + valueSize.Width, point.Y);
			unit.Paint(g, unitPoint);


		}

		public override BoundaryInfo GetBoundary(IGraphics g)
		{
			SizeF valueSize = g.MeasureString(Value);
			if (!HasArguments) {
				return new BoundaryInfo
				{
					Size = valueSize,
					BaseLine = valueSize.Height
				};
			}

			// TODO: poprawic, po zrobione 

			UnitElement unit = (UnitElement)Arguments[0];

			BoundaryInfo unitBounds = unit.GetBoundary(g);

			float totalWidth = valueSize.Width + unitBounds.Width;
			float totalHeight = Math.Max(valueSize.Height, unitBounds.Height);

			return new BoundaryInfo
			{
				Size = new SizeF(totalWidth, totalHeight),
				BaseLine = valueSize.Height
			};
			




		}
	}
}
