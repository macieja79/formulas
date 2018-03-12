using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class SumElement : MultiArgsElement
	{


		public SumElement() : base()
		{
			Signs = new List<string>();
		}

		public List<string> Signs { get; set; }

		public override void Paint(IGraphics g, PointF point)
		{

			PaintArguments(g, point, Signs);

		}



		public override BoundaryInfo GetBoundary(IGraphics g)
		{

			return GetArgumentsBoundary(g, Signs);

		}
	}

}

