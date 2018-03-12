using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formulas
{
	public class TreeViewFactory
	{




        
        #region <singleton>

		TreeViewFactory() { }

		static TreeViewFactory _instance;

		public static TreeViewFactory Instance
		{
			get
			{
				if (null == _instance) _instance = new TreeViewFactory();
				
				return _instance;
			}

		}
        
		#endregion

	





		public void CreateFormulaTree(TreeView view, FormulaItem formula)
		{
			TreeNode node = new TreeNode();
			node.Text = formula.Pattern;
			view.Nodes.Add(node);

			if (formula is FunctionItem) {
				FunctionItem function = (FunctionItem)formula;

				foreach (FormulaItem arg in function.Arguments) {
					addItem(node, arg);
				}
			}


		}

		void addItem(TreeNode parent, FormulaItem item)
		{

			TreeNode node = new TreeNode();
			node.Text = item.Pattern;
			parent.Nodes.Add(node);

			if (item is FunctionItem) {
				FunctionItem function = (FunctionItem)item;

				foreach (FormulaItem arg in function.Arguments) {
					addItem(node, arg);
				}
			}
		}



	
	
	}
}
