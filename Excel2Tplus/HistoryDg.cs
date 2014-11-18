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

		public void InitList(IEnumerable<DateTime> items)
		{
			listBox1.Items.Clear();
			foreach (var item in items)
			{
				listBox1.Items.Add(item.ToString("yyyy-MM-dd HH:mm:ss.fff"));
			}
		}

		public DateTime GetSelected()
		{
			return DateTime.Parse(listBox1.SelectedItem as string);
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
