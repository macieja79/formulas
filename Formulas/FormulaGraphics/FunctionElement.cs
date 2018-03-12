using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Formulas
{
	public class FunctionElement : MultiArgsElement
	{


		public FunctionElement()
		{
			TypeOfBracket = BracketShapeType.Round;
		}


		public BracketShapeType TypeOfBracket { get; set; }

		public string FunctionName { get; set; }
	

		int BRACKET_WIDTH = 3;

		

		public override void Paint(IGraphics g, PointF point)
		{
		

			SizeF functionSize = g.MeasureString(FunctionName);

			List<string> signs = new List<string>();
			foreach (var Arg in Arguments)
				signs.Add(";");
			
			BoundaryInfo argsSize = GetArgumentsBoundary(g, signs);

			g.DrawString(FunctionName, point);

			PointF argPoint = new PointF(point.X + functionSize.Width + BRACKET_WIDTH, point.Y);
			PaintArguments(g, argPoint, signs);

			float x1 = point.X + functionSize.Width;
			float x2 = point.X + functionSize.Width + BRACKET_WIDTH;
			float x3 = point.X + functionSize.Width + BRACKET_WIDTH + argsSize.Width;
			float x4 = point.X + functionSize.Width + BRACKET_WIDTH + argsSize.Width + BRACKET_WIDTH;

			float y1 = point.Y;
			float y12 = point.Y + argsSize.Height * 0.5f;
			float y2 = point.Y + argsSize.Height;


			if (TypeOfBracket == BracketShapeType.Squared) {
				g.DrawLine(x2, y2, x1, y2);
				g.DrawLine(x1, y2, x1, y1);
				g.DrawLine(x1, y1, x2, y1);

				g.DrawLine(x3, y1, x4, y1);
				g.DrawLine(x4, y1, x4, y2);
				g.DrawLine(x4, y2, x3, y2);
			}
			
			if (TypeOfBracket == BracketShapeType.Round) {
				PointF p1 = new PointF(x2, y1);
				PointF p2 = new PointF(x1, y12);
				PointF p3 = new PointF(x2, y2);

				PointF p4 = new PointF(x3, y1);
				PointF p5 = new PointF(x4, y12);
				PointF p6 = new PointF(x3, y2);

							PointF[] pointsLeft = new PointF[] { p1, p2, p3 };
			g.DrawCurve(pointsLeft);
			
			PointF[] pointsRight = new PointF[] { p4, p5, p6 };
			g.DrawCurve(pointsRight);

			}

		}




		public override BoundaryInfo GetBoundary(IGraphics g)
		{

			SizeF functionSize = g.MeasureString(FunctionName);

			List<string> signs = new List<string>();
			foreach (var Arg in Arguments)
				signs.Add(";");
			
			BoundaryInfo argsSize = GetArgumentsBoundary(g, signs);



			float width = functionSize.Width + BRACKET_WIDTH + argsSize.Width + BRACKET_WIDTH;
			float height = BRACKET_WIDTH + argsSize.Height + BRACKET_WIDTH;

			float baseLine = argsSize.BaseLine;

			return new BoundaryInfo
			{
				Size = new SizeF(width, height),
				BaseLine = baseLine
			};

		}
	}
}
