using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CertificateGenerator
{
	public partial class MainForm : Form
	{
		private string _user = null;
		private Controller _ctrller = null;
		private string _receiptFlag = null;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var af = new AccountForm();
			af.ShowDialog(this);
			if (af.Exited)
			{
				this.Close();
				return;
			}
			this._user = af.User;
			this._ctrller = new Controller(this._user);
			this._ctrller.BindReceipts(this.treeView1);
			var crt = DateTime.Now;
			this.dateStart.Value = new DateTime(crt.Year, crt.Month, 1);
			this.dateEnd.Value = this.dateStart.Value.AddMonths(1).AddDays(-1);
		}

		private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Tag == null)
			{
				return;
			}
			this._receiptFlag = e.Node.Tag.ToString();
			try
			{
				this._ctrller.BindReceiptInfo(this.dataGridView1, this._receiptFlag);
			}
			catch
			{
				MessageBox.Show("无该单据信息");
			}
		}

		private void build_Click(object sender, EventArgs e)
		{
			var err = "";
			var ids = new List<string>();
			foreach (DataGridViewRow row in this.dataGridView1.Rows)
			{
				if (row.Cells["凭证号"].Value is string && !string.IsNullOrEmpty(row.Cells["凭证号"].Value.ToString()))
				{
					err += row.Cells["单据号"].Value.ToString() + "\r\n";
					continue;
				}
				if (Convert.ToBoolean(row.Cells["select"].Value))
				{
					ids.Add(row.Cells["key"].Value.ToString());
				}
			}
			if (ids.Count == 0)
			{
				MessageBox.Show(err + "请选择单据");
				return;
			}
			if (err.Length > 0)
			{
				MessageBox.Show(err + "已生成过凭证，不能再次生成！");
				return;
			}
			try
			{
				this._ctrller.BuildReceiptToCertificate(this._receiptFlag, ids.ToArray());
				MessageBox.Show("单据生成成功");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !Convert.ToBoolean(this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
			}
		}

		private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.tabControl2.SelectedIndex == 0)
			{
				this.InitDbForm();
			}
			else
			{
				this._ctrller.BindReceipts(this.treeView2);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this._ctrller.SetOutDbInfo(this.outServer.Text, this.outDatabase.Text, this.outName.Text, this.outPswd.Text);
			this._ctrller.SetInDbInfo(this.inServer.Text, this.inDatabase.Text, this.inName.Text, this.inPswd.Text);
			MessageBox.Show("保存成功");
		}

		private void treeView2_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Node.Tag != null)
			{
				string name, infoSql, outSql;
				this._ctrller.GetReceiptInfo(e.Node.Tag.ToString(), out name, out infoSql, out outSql);
				this.recName.Text = name;
				this.recFlag.Text = e.Node.Tag.ToString();
				this.recInfoSql.Text = infoSql;
				this.recOutSql.Text = outSql;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (this.recFlag.ReadOnly)
			{
				this.recFlag.ReadOnly = false;
				this.recName.Text = this.recFlag.Text = this.recInfoSql.Text = this.recOutSql.Text = string.Empty;
				this.button4.Enabled = false;
				this.button2.Text = "取消";
			}
			else
			{
				this.recFlag.ReadOnly = true;
				this.button4.Enabled = true;
				this.button2.Text = "新增";
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.recFlag.Text))
			{
				if (this.recFlag.ReadOnly)
				{
					this._ctrller.SaveReceiptInfo(this.recFlag.Text, this.recName.Text, this.recInfoSql.Text, this.recOutSql.Text);
					this._ctrller.BindReceipts(this.treeView2);
					MessageBox.Show("保存成功");
				}
				else
				{
					this._ctrller.AddReceiptInfo(this.recFlag.Text, this.recName.Text, this.recInfoSql.Text, this.recOutSql.Text);
					this.button2_Click(this.button2, new EventArgs());
					this._ctrller.BindReceipts(this.treeView2);
					MessageBox.Show("新增成功");
				}
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.recFlag.Text) && this.recFlag.ReadOnly)
			{
				if (MessageBox.Show("是否要删除？", "删除", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
				{
					this._ctrller.RemoveReceiptInfo(this.recFlag.Text);
					this._ctrller.BindReceipts(this.treeView2);
					this.WaitReceiptInfoForm();
					MessageBox.Show("删除成功");
				}
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.tabControl1.SelectedIndex == 0)
			{
				this._ctrller.BindReceipts(this.treeView1);
			}
			else
			{
				this.InitDbForm();
			}
		}

		private void InitDbForm()
		{
			string server, database, name, pswd;
			this._ctrller.GetOutDbInfo(out server, out database, out name, out pswd);
			this.outServer.Text = server;
			this.outDatabase.Text = database;
			this.outName.Text = name;
			this.outPswd.Text = pswd;
			//
			this._ctrller.GetInDbInfo(out server, out database, out name, out pswd);
			this.inServer.Text = server;
			this.inDatabase.Text = database;
			this.inName.Text = name;
			this.inPswd.Text = pswd;
		}

		private void WaitReceiptInfoForm()
		{
			this.recName.Text = this.recFlag.Text = this.recInfoSql.Text = this.recOutSql.Text = string.Empty;
			this.recFlag.ReadOnly = true;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show(@"Sql说明：
查询结果必须为：select 0 as [check], xxx as [key], ...
[check]：必须为0
[key]：单据唯一编码，用于生成时查找对应的单据");
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			MessageBox.Show(@"outSql说明：
查询结果必须为：GETDATE() AS created ,NULL AS audited ,'' AS createUser ,
	borrowRemark ,borrowSubject ,borrowMoney,borrowDept,borrowSup
	lendRemark ,lendSubject ,lendMoney,lendDept,lendSup
其中,borrowDept,borrowSup和,lendDept,lendSup可选");
		}

		private void InstallHistory_Click(object sender, EventArgs e)
		{
			this._ctrller.InstallHistoryTable();
			MessageBox.Show("安装成功");
		}
	}
}
