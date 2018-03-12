using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas
{
	public class Condition : CalculationItem
	{


		public ConditionType TypeOfCondition { get; set; }

		public Equation Left { get; set; }
		public Equation Right { get; set; }


		public bool? IsTrue { get; set; }


		public override void Calculate(CalculationItemStack calculationStack, bool isToAdd = true)
		{
			Left.Calculate(calculationStack, false);
			Right.Calculate(calculationStack, false);

			bool isTrue = false;

			double leftValue = Left.Value.GetValue();
			double rightValue = Right.Value.GetValue();

			if (TypeOfCondition == ConditionType.Equals)
				isTrue = leftValue == rightValue;
			else if (TypeOfCondition == ConditionType.Greater)
				isTrue = leftValue > rightValue;
			else if (TypeOfCondition == ConditionType.GreaterOrEqual)
				isTrue = leftValue >= rightValue;
			else if (TypeOfCondition == ConditionType.Less)
				isTrue = leftValue < rightValue;
			else if (TypeOfCondition == ConditionType.LessOrEqual)
				isTrue = leftValue <= rightValue;

			IsTrue = isTrue;
		}
	}
}
