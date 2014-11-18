using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
		}

		private void toolStripDropDownButton1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			var item = sender as ToolStripDropDownButton;
			if (item != null) _billType = e.ClickedItem.Text;
			var result = openFileDialog1.ShowDialog();
			if (result == DialogResult.Cancel) return;

			var excelPath = openFileDialog1.FileName;
			var excelImportManager = new ExcelImportManager();
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
			new PriceHandler().Handler(_list);
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
					entity.BillPrice,
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
				dataGridView1.Rows[e.RowIndex].Cells[6].Value = dataGridView1.Rows[e.RowIndex].Cells[5].Value;
			}
			else if (e.ColumnIndex == 6)
			{
				dataGridView1.Rows[e.RowIndex].Cells[5].Value = dataGridView1.Rows[e.RowIndex].Cells[6].Value;
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
				_list.ElementAt(i).UseBookPrice = (bool)dataGridView1.Rows[i].Cells[5].Value;
			}
			new HistoryManager().Set(_list);
			new DatabaseExportManager().Export(_list);
		}

		private void 数据库配置ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_sysCfg = new SysConfigManager().Get();
			var form = new DbConfig();
			form.InitConfig(_sysCfg.DbConfig);
			var result = form.ShowDialog();
			if (result == DialogResult.OK)
			{
				form.SetConfig(_sysCfg.DbConfig);
				new SysConfigManager().Set(_sysCfg);
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
	}
}
