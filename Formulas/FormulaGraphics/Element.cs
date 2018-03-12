using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public abstract class Element
	{


		#region <flyweight>
		class SignElement : Element
		{


			string _sign;

			public SignElement(string sign)
			{
				_sign = sign;
			}

			public override void Paint(IGraphics g, PointF point)
			{
				g.DrawString(_sign, point);
			}

			public override BoundaryInfo GetBoundary(IGraphics g)
			{

				SizeF size = g.MeasureString(_sign);
				
				return new BoundaryInfo()
				{
					Size = size,
					BaseLine = size.Height
				};
				
			}
		}

		static SignElement _plus = new SignElement("+");
		static SignElement _minus = new SignElement("-");
		static SignElement _equal = new SignElement("=");

		public static Element Plus
		{
			get
			{
				return _plus;
			}
		}

		public static Element Minus
		{
			get
			{
				return _minus;
			}
		}

		public static Element Eq
		{
			get
			{
				return _equal;
			}
		}
		#endregion



		public Element()
		{
			Arguments = new List<Element>();
		}

		public List<Element> Arguments { get; set; }

		public bool HasArguments
		{
			get
			{
				return Arguments.Count > 0;
			}

		}


		public abstract void Paint(IGraphics g, PointF point);
		public abstract BoundaryInfo GetBoundary(IGraphics g);

		

	}
}
