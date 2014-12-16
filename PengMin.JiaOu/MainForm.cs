﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using PengMin.Infrastructure;
using PengMin.JiaOu.Dal;
using PengMin.JiaOu.SysConfig;

namespace PengMin.JiaOu
{
	public partial class MainForm : Form
	{
		private AccountInfo _from, _to;

		public MainForm()
		{
			InitializeComponent();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			new AccountManager().ShowDialog();
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() == DialogResult.OK)
			{
				_from = sl.CheckedInfo;
				var data = new DataAccess(new SqlHelper(_from.GetConnectionString())).GetPurchaseOrder();
				dataGridView1.Columns.Clear();
				foreach (DataColumn cln in data.Columns)
				{
					dataGridView1.Columns.Add(cln.ColumnName, cln.ColumnName);
				}
				dataGridView1.Columns[0].Visible = false;
				foreach (DataRow row in data.Rows)
				{
					dataGridView1.Rows.Add(
						row["id"],
						row["单据日期"],
						row["单据编号"],
						row["供应商"],
						row["业务员"],
						row["预计到货日期"],
						row["付款方式"],
						row["订金金额"],
						row["预付款百分比"]
					);
				}
			}
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() != DialogResult.OK) return;

			var fromSqlHelper = new SqlHelper(_from.GetConnectionString());
			var toSqlHelper = new SqlHelper(_to.GetConnectionString());
			var purchaseOrder = new DataAccess(fromSqlHelper)
				.ImportPurchaseOrder((from DataGridViewRow row in dataGridView1.Rows select (Guid)row.Cells[0].Value)
					.ToArray());
			if (purchaseOrder != null && purchaseOrder.Rows.Count > 0)
			{
				var saleOrder = DataAccess.PurchaseOrderToSaleOrder(purchaseOrder, _from.Name);
				var result = new DataAccess(toSqlHelper).ExportSaleOrder(saleOrder);
				MessageBox.Show(string.Join("\r\n", result));
			}
			else
			{
				MessageBox.Show("没有可导入的采购订单");
			}
		}

		private void 安装采购订单预付款程序ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() != DialogResult.OK) return;

			var sql = @"IF ( OBJECT_ID('yufukuan_insert', 'tr') IS NOT NULL )
    DROP TRIGGER yufukuan_insert
GO
CREATE TRIGGER yufukuan_insert ON dbo.PU_PurchaseOrder
    FOR INSERT
AS
    DECLARE @percent FLOAT ,
        @money DECIMAL ,
        @id UNIQUEIDENTIFIER
    SELECT  @percent = CONVERT(FLOAT, REPLACE(ISNULL(pubuserdefnvc1, '0%'),
                                              '%', '')) ,
            @money = totalAmount ,
            @id = id
    FROM    Inserted
    UPDATE  dbo.PU_PurchaseOrder
    SET     origEarnestMoney = @money * @percent / 100
    WHERE   id = @id
GO
IF ( OBJECT_ID('yufukuan_update', 'tr') IS NOT NULL )
    DROP TRIGGER yufukuan_update
GO
CREATE TRIGGER yufukuan_update ON dbo.PU_PurchaseOrder
    FOR UPDATE
AS
    DECLARE @percent FLOAT ,
        @money DECIMAL ,
        @id UNIQUEIDENTIFIER
    SELECT  @percent = CONVERT(FLOAT, REPLACE(ISNULL(pubuserdefnvc1, '0%'),
                                              '%', '')) ,
            @money = totalAmount ,
            @id = id
    FROM    Inserted
    UPDATE  dbo.PU_PurchaseOrder
    SET     origEarnestMoney = @money * @percent / 100
    WHERE   id = @id
GO";
			var sqlHelper = new SqlHelper(sl.CheckedInfo.GetConnectionString());
			sqlHelper.Open();
			sqlHelper.Execute(sql);
			sqlHelper.Close();
		}

		private void 卸载采购订单预付款程序ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() != DialogResult.OK) return;

			var sql = @"IF ( OBJECT_ID('yufukuan_insert', 'tr') IS NOT NULL )
    DROP TRIGGER yufukuan_insert
GO";
			var sqlHelper = new SqlHelper(sl.CheckedInfo.GetConnectionString());
			sqlHelper.Open();
			sqlHelper.Execute(sql);
			sqlHelper.Close();
		}
	}
}
