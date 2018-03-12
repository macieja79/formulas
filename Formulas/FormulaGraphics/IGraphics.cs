using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public interface IGraphics
	{
		SizeF MeasureString(string str);

	
		void DrawString(string str, PointF point);
		void DrawLine(float x1, float y1, float x2, float y2);
		void DrawLine(PointF p1, PointF p2);
		void PaintBoundary(BoundaryInfo boundary, PointF point);
		void DrawCurve(PointF[] points);

	}
}
