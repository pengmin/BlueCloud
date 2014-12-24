using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

		public DateTime GetSelected()
		{
			return DateTime.Parse(listBox1.SelectedItem.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Last());
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
