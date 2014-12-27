using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Excel2Tplus
{
	public partial class HistoryDg : Form
	{
		public HistoryDg()
		{
			InitializeComponent();
		}

		public void InitList(IEnumerable<string> items)
		{
			listBox1.Items.Clear();
			foreach (var item in items)
			{
				listBox1.Items.Add(item);
			}
		}

		public string GetSelected()
		{
			return listBox1.SelectedItem.ToString();
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			if (listBox1.SelectedItem != null)
			{
				DialogResult = DialogResult.OK;
			}
			Close();
		}
	}
}
