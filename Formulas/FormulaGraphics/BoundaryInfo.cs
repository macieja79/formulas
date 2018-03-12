using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class BoundaryInfo
	{


		#region <static>
		static BoundaryInfo _empty;

		public static BoundaryInfo Empty
		{
			get
			{
				if (null == _empty)
				{
					_empty = new BoundaryInfo()
					{
						BaseLine = 0,
						Size = new SizeF()
					};
				}

				return _empty;

			}

		}

		public static BoundaryInfo GetBoundaryInfo(IGraphics g, Element element)
		{
			if (null == element)
				return BoundaryInfo.Empty;
			return element.GetBoundary(g);
		}

		public static float GetWidthSummed(params BoundaryInfo[] boundaries)
		{
			return boundaries.Sum(b => b.Width);
		}

		public static float GetHeightSummed(params BoundaryInfo[] boundaries)
		{
			return boundaries.Sum(b => b.Height);
		}

		
		public static float GetWidthMax(params BoundaryInfo[] boundaries)
		{
			return boundaries.Max(b => b.Width);
		}


		public static BoundaryInfo GetElementsBoundary(IGraphics g, List<Element> elements)
		{
			List<BoundaryInfo> boundaries = elements.Select(a => a.GetBoundary(g)).ToList();
			
			float aboveBaseline = boundaries.Select(b => b.BaseLine).Max();
			float underBaseline = boundaries.Select(b => b.UnderBase).Max();

			float width = boundaries.Select(b => b.Width).Sum();
			float height = aboveBaseline + underBaseline;

			float baseLine = aboveBaseline;

			return new BoundaryInfo
			{
				Size = new SizeF(width, height),
				BaseLine = baseLine
			};
		}

		public static void PaintElements(IGraphics g, PointF point, List<Element> elements)
		{
			BoundaryInfo boundary = GetElementsBoundary(g, elements);
			g.PaintBoundary(boundary, point);

			float ix = point.X;
			for (int i = 0 ; i < elements.Count ; ++i) {

				Element iElement = elements[i];
				BoundaryInfo iBoundary = iElement.GetBoundary(g);

				float y0 = point.Y + (boundary.BaseLine - iBoundary.BaseLine);
				PointF iPoint = new PointF(ix, y0);

				iElement.Paint(g, iPoint);
				ix += iBoundary.Width;
			}
		}
		

		#endregion

		#region <props>
		public SizeF Size { get; set; }
		public float BaseLine { get; set; }

		public float Height
		{
			get
			{
				return Size.Height;
			}
		}

		public float Width
		{
			get
			{
				return Size.Width;
			}
		}


		public float UnderBase
		{
			get
			{
				return Size.Height - BaseLine;
			}
		}
		#endregion



	}

}
