using System;
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
			if (sl.ShowDialog() == DialogResult.OK && sl.CheckedInfo != null)
			{
				_from = sl.CheckedInfo;
				var partner = textBox1.Text;
				var sd = dateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:00");
				var ed = dateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59");
				var data = new DataAccess(new SqlHelper(_from.GetConnectionString())).GetPurchaseOrder(partner, sd, ed);
				dataGridView1.Columns.Clear();
				dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { ReadOnly = false, Width = 30 });
				foreach (DataColumn cln in data.Columns)
				{
					dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = cln.ColumnName, ReadOnly = true });
				}
				dataGridView1.Columns[1].Visible = false;
				foreach (DataRow row in data.Rows)
				{
					dataGridView1.Rows.Add(
						false,
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
				textBox1.Focus();
			}
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			if (_from == null)
			{
				MessageBox.Show("请先选择导入采购订单");
				return;
			}
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() != DialogResult.OK) return;

			var fromSqlHelper = new SqlHelper(_from.GetConnectionString());
			var toSqlHelper = new SqlHelper(sl.CheckedInfo.GetConnectionString());
			var da = new DataAccess(fromSqlHelper);
			var result = new List<string>();
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				if (!(bool)row.Cells[0].Value) continue;
				var purchaseOrder = da.ImportPurchaseOrder((Guid)row.Cells[1].Value);
				if (purchaseOrder != null && purchaseOrder.Rows.Count > 0)
				{
					var saleOrder = DataAccess.PurchaseOrderToSaleOrder(purchaseOrder, _from.Name);
					var rt = new DataAccess(toSqlHelper).ExportSaleOrder(saleOrder);
					result.AddRange(rt);
				}
			}
			if (result.Count > 0)
			{
				result.Insert(0, "导出成功！\r\n");
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

			var sqls = new[]
			{
				@"IF ( OBJECT_ID('yvfukuan_insert', 'tr') IS NOT NULL ) 
    DROP TRIGGER yvfukuan_insert",
//------------------------
				@"CREATE TRIGGER yvfukuan_insert ON dbo.PU_PurchaseOrder
    FOR INSERT
AS
    --定义变量
    DECLARE @percent FLOAT ,--预付款百分比
        @money DECIMAL ,--含税金额
        @id UNIQUEIDENTIFIER ,--单据id
        @earnestMoney DECIMAL--预付款
    SELECT  @percent = CONVERT(FLOAT, REPLACE(ISNULL(pubuserdefnvc1, '0%'),
                                              '%', '')) ,
            @money = totalTaxAmount ,
            @id = id
    FROM    INSERTED
--没有设置预付款百分比则不执行任何操作  
    IF ( @percent = 0 ) 
        BEGIN
            RETURN
        END  
--计算并设置预付款
    SET @earnestMoney = @money * @percent / 100 
    UPDATE  PU_PurchaseOrder
    SET     earnestMoney = @earnestMoney ,
            origEarnestMoney = @earnestMoney
    WHERE   id = @id
		--添加预付款项
    INSERT  INTO [PU_PurchaseOrder_ArnestMoney]
            ( id ,
              idPurchaseOrderDTO ,
              idbankaccount ,
              updated ,
              idsettlestyle ,
              updatedBy ,
              amount ,
              sequencenumber ,
              origAmount ,
              code
            )
    VALUES  ( NEWID() ,
              @id ,
              '4adbf11c-9eca-4a3f-999b-a8e00b657e19' ,
              GETDATE() ,
              'c14bf775-089e-4e58-96c5-9b482f5a42b9' ,
              'demo' ,
              @earnestMoney ,
              0 ,
              @earnestMoney ,
              '0000'
            );",
//------------------------
				@"IF ( OBJECT_ID('yvfukuan_update', 'tr') IS NOT NULL ) 
    DROP TRIGGER yvfukuan_update",
//------------------------
				@"CREATE TRIGGER yvfukuan_update ON dbo.PU_PurchaseOrder
    FOR UPDATE
AS
    --定义变量      
    DECLARE @percent FLOAT ,--当前预付款百分比
        @money DECIMAL ,--含税金额
        @id UNIQUEIDENTIFIER ,--单据id
        @oldPercent FLOAT ,--原预付款百分比
        @earnestMoney DECIMAL--预付款
--获取当前预付款百分比
    SELECT  @percent = CONVERT(FLOAT, REPLACE(ISNULL(pubuserdefnvc1, '0%'),
                                              '%', '')) ,
            @money = totalTaxAmount ,
            @id = id
    FROM    INSERTED
--获取原预付款百分比  
    SELECT  @oldPercent = CONVERT(FLOAT, REPLACE(ISNULL(pubuserdefnvc1, '0%'),
                                                 '%', ''))
    FROM    deleted
--如果预付款百分比未改变，则不执行任何操作
    IF ( @percent = @oldPercent ) 
        BEGIN
            RETURN
        END
--计算并设置预付款
    SET @earnestMoney = @money * @percent / 100
    UPDATE  PU_PurchaseOrder
    SET     earnestMoney = @earnestMoney ,
            origEarnestMoney = @earnestMoney
    WHERE   id = @id
--若存在预付款项，则删除，因为可能有多条预付款项，重新计算预付款时没法分配
    IF ( EXISTS ( SELECT    1
                  FROM      PU_PurchaseOrder_ArnestMoney
                  WHERE     idPurchaseOrderDTO = @id ) ) 
        BEGIN
            UPDATE  PU_PurchaseOrder_ArnestMoney
            SET     amount = @earnestMoney ,
                    origAmount = @earnestMoney
        END
    ELSE 
        BEGIN      
            INSERT  INTO [PU_PurchaseOrder_ArnestMoney]
                    ( id ,
                      idPurchaseOrderDTO ,
                      idbankaccount ,
                      updated ,
                      idsettlestyle ,
                      updatedBy ,
                      amount ,
                      sequencenumber ,
                      origAmount ,
                      code
                    
                    )
            VALUES  ( NEWID() ,
                      @id ,
                      '4adbf11c-9eca-4a3f-999b-a8e00b657e19' ,
                      GETDATE() ,
                      'c14bf775-089e-4e58-96c5-9b482f5a42b9' ,
                      'demo' ,
                      @earnestMoney ,
                      0 ,
                      @earnestMoney ,
                      '0000'
                    );
        END"
			};
			var sqlHelper = new SqlHelper(sl.CheckedInfo.GetConnectionString());
			sqlHelper.Open();
			foreach (var sql in sqls)
			{
				sqlHelper.Execute(sql);
			}
			sqlHelper.Close();
			MessageBox.Show("安装成功");
		}

		private void 卸载采购订单预付款程序ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var sl = new AccountSelectForm(new SysConfigManager().Get().Accounts);
			if (sl.ShowDialog() != DialogResult.OK) return;

			var sql = @"IF ( OBJECT_ID('yvfukuan_insert', 'tr') IS NOT NULL )
    DROP TRIGGER yvfukuan_insert;
IF ( OBJECT_ID('yvfukuan_update', 'tr') IS NOT NULL ) 
    DROP TRIGGER yvfukuan_update";
			var sqlHelper = new SqlHelper(sl.CheckedInfo.GetConnectionString());
			sqlHelper.Open();
			sqlHelper.Execute(sql);
			sqlHelper.Close();
			MessageBox.Show("卸载成功");
		}

		private void toolStripButton5_Click(object sender, EventArgs e)
		{
			textBox1.Focus();
			if (dataGridView1.Rows.Count > 0)
			{
				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					row.Cells[0].Value = true;
				}
			}
		}
	}
}
