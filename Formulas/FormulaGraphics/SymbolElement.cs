using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Formulas
{
	public class SymbolElement : Element
	{



		public string Symbol { get; set; }
		public string Suffix { get; set; }
	
		

		public override void Paint(IGraphics g, PointF p)
		{

			BoundaryInfo boundary = GetBoundary(g);
			g.PaintBoundary(boundary, p);

			SizeF symbolSize = g.MeasureString(Symbol);
			SizeF suffixSize = g.MeasureString(Suffix);
		

			g.DrawString(Symbol, p);

			PointF indexPoint = new PointF(
				p.X + symbolSize.Width, 
				p.Y + symbolSize.Height - suffixSize.Height * 0.5f);

			g.DrawString(Suffix, indexPoint);

			
		

		}
		

		public override BoundaryInfo GetBoundary(IGraphics g)
		{

			SizeF symbolSize = g.MeasureString(Symbol);
			SizeF suffixSize = g.MeasureString(Suffix);


			SizeF rectangle = new SizeF(symbolSize.Width + suffixSize.Width, symbolSize.Height + suffixSize.Height * 0.5f);
			float baseLine = symbolSize.Height;
			

			return new BoundaryInfo
			{
				Size = rectangle,
				BaseLine = baseLine
			};
	
		}
	}
}
