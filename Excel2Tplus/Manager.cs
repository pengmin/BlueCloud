using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel2Tplus.Common;
using Excel2Tplus.DatabaseExport;
using Excel2Tplus.Entities;
using Excel2Tplus.ExcelImport;
using Excel2Tplus.History;
using Excel2Tplus.PriceHandle;
using Excel2Tplus.SysConfig;

namespace Excel2Tplus
{
	public partial class Manager : Form
	{
		private string _billType = string.Empty;
		private IEnumerable<Entity> _list;
		private SystemConfig _sysCfg;
		private bool _bookPriceAllChecked;//是否是价格本价格全选
		private bool _billPriceAllChecked;//是否是单据价格全选

		public Manager()
		{
			InitializeComponent();
			openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			if (!new SysConfigManager().Get().HasDbConfig)
			{
				数据库配置ToolStripMenuItem_Click(数据库配置ToolStripMenuItem, new EventArgs());
			}
		}

		private void toolStripDropDownButton1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			var item = sender as ToolStripDropDownButton;
			if (item != null) _billType = e.ClickedItem.Text;
			var result = openFileDialog1.ShowDialog();
			if (result == DialogResult.Cancel) return;

			var excelPath = openFileDialog1.FileName;
			var excelImportManager = new ExcelImportManager();
			//try
			//{
			switch (_billType)
			{
				case "请购单":
					_list = excelImportManager.Import<PurchaseRequisition>(excelPath);
					break;
				case "采购订单":
					_list = excelImportManager.Import<PurchaseOrder>(excelPath);
					break;
				case "进货单":
					_list = excelImportManager.Import<PurchaseArrival>(excelPath);
					break;
				case "采购入库单":
					_list = excelImportManager.Import<InputWarehouse>(excelPath);
					break;
				case "报价单":
					_list = excelImportManager.Import<SaleQuotation>(excelPath);
					break;
				case "销售订单":
					_list = excelImportManager.Import<SaleOrder>(excelPath);
					break;
				case "销售出库单":
					_list = excelImportManager.Import<OutputWarehouse>(excelPath);
					break;
				case "销货单":
					_list = excelImportManager.Import<SaleDelivery>(excelPath);
					break;
			}
			//}
			//catch
			//{
			//	MessageBox.Show("Excel文件已被其他程序打开");
			//	return;
			//}
			try
			{
				new PriceHandler().Handler(_list);
			}
			catch (Exception ex)
			{
				_list = null;
				MessageBox.Show("客户不是客户性质或客户没有设置价格等级");
				return;
			}

			ShowToView(_list);
		}

		private void ShowToView(IEnumerable<Entity> list)
		{
			dataGridView1.Rows.Clear();
			foreach (var entity in list)
			{
				dataGridView1.Rows.Add(
					entity.InventoryCode,
					entity.InventoryName,
					entity.BookPrice,
					entity.含税单价,
					entity.Differential,
					entity.UseBookPrice,
					!entity.UseBookPrice
				);
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 5)
			{
				dataGridView1.Rows[e.RowIndex].Cells[5].Value = !(bool)dataGridView1.Rows[e.RowIndex].Cells[5].Value;
				dataGridView1.Rows[e.RowIndex].Cells[6].Value = !(bool)dataGridView1.Rows[e.RowIndex].Cells[5].Value;
			}
			else if (e.ColumnIndex == 6)
			{
				dataGridView1.Rows[e.RowIndex].Cells[6].Value = !(bool)dataGridView1.Rows[e.RowIndex].Cells[6].Value;
				dataGridView1.Rows[e.RowIndex].Cells[5].Value = !(bool)dataGridView1.Rows[e.RowIndex].Cells[6].Value;
			}
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (_list == null || !_list.Any())
			{
				MessageBox.Show("请先导入单据");
				return;
			}

			for (var i = 0; i < _list.Count(); i++)
			{
				_list.ElementAt(i).UseBookPrice = (bool)dataGridView1.Rows[i].Cells[5].FormattedValue;
				//(bool)dataGridView1.Rows[i].Cells[5].Value;
			}

			var msgList = new DatabaseExportManager().Export(_list);
			var msgStr = string.Join("\r\n", msgList.ToArray());
			if (msgList.Last() != "-1")
			{
				new HistoryManager().Set(_list, _billType + "[" + msgList.ElementAt(msgList.Count() - 2) + "]");
				MessageBox.Show("导入完成\r\n" + msgStr);
			}
			else
			{
				MessageBox.Show("导入失败\r\n" + msgStr);
			}
		}

		private void 数据库配置ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_sysCfg = new SysConfigManager().Get();
			var form = new DbConfig();
			form.InitConfig(_sysCfg.DbConfig);
			var result = form.ShowDialog();

			if (result != DialogResult.OK) return;

			form.SetConfig(_sysCfg.DbConfig);
			var sh = new SqlHelper(_sysCfg.DbConfig.GetConnectionString());
			try
			{
				sh.Open();
				new SysConfigManager().Set(_sysCfg);
				try
				{
					sh.Execute(_sysCfg.Excel2TplusHistorySql);
				}
				catch { }
			}
			catch
			{
				MessageBox.Show("数据库无法连接");
			}
			finally
			{
				sh.Close();
			}
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			if (_bookPriceAllChecked)
			{
				_bookPriceAllChecked = false;
				toolStripButton3.Text = "价格本确认全选";
			}
			else
			{
				_bookPriceAllChecked = true;
				toolStripButton3.Text = "价格本确认全消";
			}
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				row.Cells[5].Value = _bookPriceAllChecked;
				row.Cells[6].Value = !_bookPriceAllChecked;
			}
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			if (_billPriceAllChecked)
			{
				_billPriceAllChecked = false;
				toolStripButton4.Text = "单据价格确认全选";
			}
			else
			{
				_billPriceAllChecked = true;
				toolStripButton4.Text = "单据价格确认全消";
			}
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				row.Cells[6].Value = _billPriceAllChecked;
				row.Cells[5].Value = !_billPriceAllChecked;
			}
		}

		private void toolStripButton5_Click(object sender, EventArgs e)
		{
			var hm = new HistoryManager();
			var hdg = new HistoryDg();
			hdg.InitList(hm.GetHead());

			var dr = hdg.ShowDialog();
			if (dr == DialogResult.OK)
			{
				_list = hm.Get<Entity>(hdg.GetSelected());
				ShowToView(_list);
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			var dr = saveFileDialog1.ShowDialog();
			if (dr != DialogResult.OK)
			{
				return;
			}
			var path = saveFileDialog1.FileName;
			if (!File.Exists(path))
			{
				File.Create(path).Close();
			}
			var dt = new DataTable();
			var clnCount = dataGridView1.ColumnCount;
			for (var i = 0; i < clnCount; i++)
			{
				dt.Columns.Add(new DataColumn(dataGridView1.Columns[i].HeaderText));
			}
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				var i = 0;
				var r = dt.NewRow();
				foreach (DataGridViewCell cell in row.Cells)
				{
					r[i++] = cell.Value;
				}
				dt.Rows.Add(r);
			}
			var eh = new ExcelHelper(path, true);
			eh.Write(dt);
		}
	}
}
