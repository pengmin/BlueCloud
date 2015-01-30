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
        @earnestMoney DECIMAL ,--预付款
        @accountId UNIQUEIDENTIFIER ,--账户id
        @styleId UNIQUEIDENTIFIER--结算方式id
--获取预付款百分比
    SELECT  @percent = CONVERT(FLOAT, REPLACE(ISNULL(pubuserdefnvc1, '0%'),
                                              '%', '')) ,
            @money = totalTaxAmount ,
            @id = id
    FROM    INSERTED
	IF(@percent=0)BEGIN
		RETURN
	END
--获取账户id
    DECLARE @accName NVARCHAR(50)
    SELECT  @accName = pubuserdefnvc2
    FROM    INSERTED
    IF ( @accName IS NULL
         OR @accName = ''
       ) 
        BEGIN
            SET @accName = '现金'
        END
    SELECT  @accountId = id
    FROM    dbo.AA_BankAccount
    WHERE   name = @accName  
--获取结算方式id  
    DECLARE @styName NVARCHAR(50)
    SELECT  @styName = pubuserdefnvc3
    FROM    INSERTED
    IF ( @styName IS NULL
         OR @styName = ''
       ) BEGIN
            SET @styName = '现金'
        END
    SELECT  @styleId = id
    FROM    dbo.AA_SettleStyle
    WHERE   name = @styName
--计算预付款
    SET @earnestMoney = @money * @percent / 100 
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
              @accountId ,
              GETDATE() ,
              @styleId ,
              'demo' ,
              @earnestMoney ,
              0 ,
              @earnestMoney ,
              '0000'
            );
--更新主记录预付款
    UPDATE  PU_PurchaseOrder
    SET     earnestMoney = @earnestMoney ,
            origEarnestMoney = @earnestMoney
    WHERE   id = @id",
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
        @earnestMoney DECIMAL ,--预付款
        @accountId UNIQUEIDENTIFIER ,--账户id
        @styleId UNIQUEIDENTIFIER--结算方式id
--如果更新的数据与之前的数据一致，则不用执行更新
    IF ( ( SELECT   COUNT(0)
           FROM     inserted AS a
                    JOIN deleted AS b ON a.id = b.id
                                         AND a.pubuserdefnvc2 = b.pubuserdefnvc2
                                         AND a.pubuserdefnvc3 = b.pubuserdefnvc3
                                         AND a.pubuserdefnvc1 = b.pubuserdefnvc1
										 AND a.totalTaxAmount = b.totalTaxAmount
         ) > 0 ) BEGIN
            RETURN
        END
--获取当前预付款百分比
    SELECT  @percent = CONVERT(FLOAT, REPLACE(ISNULL(pubuserdefnvc1, '0%'),
                                              '%', '')) ,
            @money = totalTaxAmount ,
            @id = id
    FROM    INSERTED
	IF(@percent=0) BEGIN
		DELETE PU_PurchaseOrder_ArnestMoney WHERE idPurchaseOrderDTO=@id
		RETURN
	END
--获取账户id
    DECLARE @accName NVARCHAR(50)
    SELECT  @accName = pubuserdefnvc2
    FROM    INSERTED
    IF ( @accName IS NULL
         OR @accName = ''
       ) 
        BEGIN
            SET @accName = '现金'
        END
    SELECT  @accountId = id
    FROM    dbo.AA_BankAccount
    WHERE   name = @accName  
--获取结算方式id  
    DECLARE @styName NVARCHAR(50)
    SELECT  @styName = pubuserdefnvc3
    FROM    INSERTED
    IF ( @styName IS NULL
         OR @styName = ''
       ) BEGIN
            SET @styName = '现金'
        END
    SELECT  @styleId = id
    FROM    dbo.AA_SettleStyle
    WHERE   name = @styName
--计算预付款
    SET @earnestMoney = @money * @percent / 100
--更新预付款记录
    IF ( ( SELECT   COUNT(0)
           FROM     dbo.PU_PurchaseOrder_ArnestMoney
           WHERE    idPurchaseOrderDTO = @id
         ) > 0 ) 
        BEGIN  
            UPDATE  PU_PurchaseOrder_ArnestMoney
            SET     amount = @earnestMoney ,
                    idbankaccount = @accountId ,
                    idsettlestyle = @styleId ,
                    origAmount = @earnestMoney
            WHERE   idPurchaseOrderDTO = @id
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
                      @accountId ,
                      GETDATE() ,
                      @styleId ,
                      'demo' ,
                      @earnestMoney ,
                      0 ,
                      @earnestMoney ,
                      '0000'
                    );  
        END
--更新主记录预付款
    UPDATE  PU_PurchaseOrder
    SET     earnestMoney = @earnestMoney ,
            origEarnestMoney = @earnestMoney
    WHERE   id = @id"
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

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				var val = (bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
				dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !val;
			}
		}
	}
}
