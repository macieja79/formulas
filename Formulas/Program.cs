using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formulas
{
    class Program
    {
        static void Main(string[] args)
        {


			Application.EnableVisualStyles ();

			FormulaTester tester = new FormulaTester();
			tester.ShowDialog();

			//FormulaBuilder builder = new FormulaBuilder();

			//FormulaItem item = builder.Create("a.1 + b.2");

			

        }
    }
}
