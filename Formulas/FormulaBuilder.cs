using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{




    public abstract class FormulaBuilderMethod
    {

        #region <to override>
        
        public abstract bool IsMe(string pattern);
        
        public abstract FormulaItem Create(string pattern);

        public FormulaBuilder Parent { get; private set; }
        
        #endregion




    }

   


    public class FormulaBuilder
    {
    
    
    }
}
