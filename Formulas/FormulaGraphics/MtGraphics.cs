using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class MtGraphics : IGraphics
	{

		Font _f = new Font("MS Gothic", 10);
		Font _fg = new Font("Symbol", 10);
		Brush _b = new SolidBrush(Color.Black);
		Pen _p = new Pen(Color.Black);
		Pen _bp = new Pen(Color.Blue);
		Pen _bbaseline = new Pen(Color.Green);
		Graphics _g;

		public bool IsBoundary { get; set; }

		public void SetGraphics(Graphics g)
		{
			_g = g;
		}

		public void DrawLine(float x1, float y1, float x2, float y2)
		{
			_g.DrawLine(_p, x1, y1, x2, y2);
		}

		public void DrawLine(PointF p1, PointF p2)
		{
			_g.DrawLine(_p, p1, p2);
		}

		public void DrawString(string str, PointF point)
		{
			if (isGreek(str)) {

				string symbol = GetGreekSymbol(str);
				_g.DrawString(symbol, _fg, _b, point);
				return;
			}
			
			_g.DrawString(str, _f, _b, point);
		}

		public SizeF MeasureString(string str)
		{
			if (isGreek(str)) {

				string symbol = GetGreekSymbol(str);
				return _g.MeasureString(symbol, _fg);
			}

			return _g.MeasureString(str, _f);
		}

		

		public void DrawCurve(PointF[] pointsF)
		{
			Point[] points = new Point[pointsF.Length];
			for (int i = 0 ; i < pointsF.Length ; ++i) {
				points[i] = new Point((int)pointsF[i].X, (int)pointsF[i].Y);
			}

			_g.DrawCurve(_p, points);
			
		}



		public void PaintBoundary(BoundaryInfo boundary, PointF point)
		{
			if (!IsBoundary)
				return;

			Rectangle rect = new Rectangle((int)point.X, (int)point.Y, (int)boundary.Size.Width, (int)boundary.Size.Height);			
			PointF p1 = new PointF(point.X, point.Y + boundary.BaseLine);
			PointF p2 = new PointF(point.X + boundary.Width, point.Y + boundary.BaseLine);
			
			_g.DrawRectangle(_bp, rect);
			_g.DrawLine(_bbaseline, p1, p2);
		}

	

		bool isGreek(string str)
		{

			return (_symbols.Contains(str));

			

		}

		string GetGreekSymbol(string str)
		{
			string symbol = str.Substring(0, 1);
			return symbol;
		}


		string[] _symbols = {

		"alpha",
"beta",
"gamma",
"delta",
"epsilon",
"zeta",
"eta",
"theta",
"iota",
"kappa",
"lambda",
"mu",
"nu",
"xi",
"omicron",
"pi",
"rho",
"sigma",
"tau",
"upsilon",
"phi",
"chi",
"psi",
"omega"};

	}

}
