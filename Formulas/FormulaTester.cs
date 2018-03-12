using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Formulas
{
	public partial class FormulaTester : Form
	{

		FormulaBuilder _builder = new FormulaBuilder();
		Converter _converter = new Converter();
		Element _element = null;
		PointF _point = new PointF(10, 10);
		MtGraphics _mtGraphics = new MtGraphics();
		CalculationBuilder _calcBuilder = new CalculationBuilder();

		Element _multiElement = null;

		public FormulaTester()
		{
			InitializeComponent();
			this.Focus();
			textBox1.Select();

			tabControl1.SelectedIndex = 1;

			//_mtGraphics.IsBoundary = true;
		}

		private void onFormulaChanged(object sender, EventArgs e)
		{

			if (!checkBox1.Checked)
				return;

			try {

				string pattern = textBox1.Text;
				FormulaItem formula = _builder.Create(pattern);
				double value = formula.GetValue();
				textBox2.Text = value.ToString();

				_element = _converter.Convert(formula);
				updateTree(formula);
				panel1.Invalidate();
				
	
			} catch (Exception exc) {

				textBox2.Text = "[ERROR]";
				_element = null;
			}

		}

		private void button1_Click(object sender, EventArgs e)
		{
			string pattern = textBox1.Text;
			FormulaItem formula = _builder.Create(pattern);
			//double value = formula.GetValue();
			//textBox2.Text = value.ToString();
			_element = _converter.Convert(formula);
			updateTree(formula);
			panel1.Invalidate();
		}

		private void button2_Click(object sender, EventArgs e)
		{

			try {

				List<string> lines = textBox3.Lines.ToList();
				List<string> cleared = _calcBuilder.getCleaned(lines);

				cleared.Insert(0, "{");
				cleared.Add("}");

				CalculationItem item = _calcBuilder.Create(cleared);
				CalculationItemStack stack = new CalculationItemStack();
				item.Calculate(stack);

				EquationBlock block = new EquationBlock();
				foreach (CalculationItem stackItem in stack.Items) {

					if (stackItem is Equation) {
						block.Equations.Add(stackItem);

					}



				}

				_multiElement = _converter.Convert(block);
				panel2.Invalidate();
			} catch (Exception exc) {
				MessageBox.Show(exc.ToString(), "Bład", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}


		private void onPaint(object sender, PaintEventArgs e)
		{
			if (_element == null)
				return;

			_mtGraphics.SetGraphics(e.Graphics);

			_element.Paint(_mtGraphics, _point);

		}

		private void onMultipanelPaint(object sender, PaintEventArgs e)
		{

			if (_multiElement == null)
				return;

			_mtGraphics.SetGraphics(e.Graphics);

			_multiElement.Paint(_mtGraphics, _point);
		}


		private void textBox1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) {
				button1_Click(null, null);
			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			_mtGraphics.IsBoundary = checkBox3.Checked;
			panel1.Invalidate();
		}

		void updateTree(FormulaItem item)
		{
			treeView1.Nodes.Clear();
			TreeViewFactory.Instance.CreateFormulaTree(treeView1, item);
			treeView1.ExpandAll();
		}

		

		

	}
}
