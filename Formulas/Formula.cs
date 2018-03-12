using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{

    #region <base classes>

    public interface IFormulaItemVisitor
    {
        void Visit(FormulaItem item);
    }

    public abstract class FormulaItem
    {
        public string Pattern { get; set; }

        // czy bedzie potrzebne?
        public FormulaItem Parent { get; set; }

        public abstract double GetValue();

        public int Level
        {
            get
            {

                int level = 1;
                FormulaItem parent = Parent;
                while (parent != null)
                {
                    level++;
                    parent = parent.Parent;
                }

                return level;

            }
        }
    
    }



    public abstract class FunctionItem : FormulaItem
    {
        
        public List<FormulaItem> Arguments { get; set; }
    }


    public class ValueItem : FormulaItem
    {

        public double Value { get; set; }

        public string Symbol { get; set; }

        public override double GetValue()
        {
            return Value;
        }

    }
    #endregion

    public class FunctionAbs : FunctionItem
    {
        public override double GetValue()
        {
            FormulaItem value = Arguments.First();
            double arg = value.GetValue();
            double result = Math.Abs(arg);
            return result;
        }
    }

    public class FunctionMin : FunctionItem
    {

        public override double GetValue()
        {

            List<double> values = Arguments.Select(a => a.GetValue()).ToList();
            double result = values.Min();
            return result;
        }
    }

    public class FunctionMax : FunctionItem
    {

        public override double GetValue()
        {
            List<double> values = Arguments.Select(a => a.GetValue()).ToList();
            double result = values.Max();
            return result;
        }
    }

    public class FunctionSum : FunctionItem
    {
        public override double GetValue()
        {
            List<double> values = Arguments.Select(a => a.GetValue()).ToList();
            double result = values.Sum();
            return result;
        }
    }

    public class FunctionMultiply : FunctionItem
    {

        public override double GetValue()
        {
            List<double> values = Arguments.Select(a => a.GetValue()).ToList();
            double result = values.Aggregate((i, i1) => i * i1);
            return result;
        }
 
    }






}
