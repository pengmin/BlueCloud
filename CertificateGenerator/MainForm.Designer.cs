namespace CertificateGenerator
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("单据目录");
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("单据目录");
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.key = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.build = new System.Windows.Forms.ToolStripMenuItem();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabControl2 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.inPswd = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.inName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.inDatabase = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.inServer = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.outPswd = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.outName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.outDatabase = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.outServer = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.recOutSql = new System.Windows.Forms.TextBox();
			this.recInfoSql = new System.Windows.Forms.TextBox();
			this.recFlag = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.recName = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.treeView2 = new System.Windows.Forms.TreeView();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.InstallHistory = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabControl2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(738, 524);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.splitContainer1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(730, 498);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "凭证生成";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
			this.splitContainer1.Panel2.Controls.Add(this.menuStrip1);
			this.splitContainer1.Size = new System.Drawing.Size(724, 492);
			this.splitContainer1.SplitterDistance = 241;
			this.splitContainer1.TabIndex = 0;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			treeNode3.Name = "Receipts";
			treeNode3.Text = "单据目录";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
			this.treeView1.Size = new System.Drawing.Size(241, 492);
			this.treeView1.TabIndex = 0;
			this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.select,
            this.key});
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 25);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.Size = new System.Drawing.Size(479, 467);
			this.dataGridView1.TabIndex = 1;
			this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
			// 
			// select
			// 
			this.select.DataPropertyName = "check";
			this.select.FalseValue = "0";
			this.select.HeaderText = "选择";
			this.select.IndeterminateValue = "0";
			this.select.Name = "select";
			this.select.ReadOnly = true;
			this.select.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.select.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.select.TrueValue = "1";
			this.select.Width = 40;
			// 
			// key
			// 
			this.key.DataPropertyName = "key";
			this.key.HeaderText = "单据号";
			this.key.Name = "key";
			this.key.ReadOnly = true;
			this.key.Visible = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.build,
            this.InstallHistory});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(479, 25);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// build
			// 
			this.build.Name = "build";
			this.build.Size = new System.Drawing.Size(44, 21);
			this.build.Text = "生成";
			this.build.Click += new System.EventHandler(this.build_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.tabControl2);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(730, 498);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "系统配置";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabControl2
			// 
			this.tabControl2.Controls.Add(this.tabPage3);
			this.tabControl2.Controls.Add(this.tabPage4);
			this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl2.Location = new System.Drawing.Point(3, 3);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(724, 492);
			this.tabControl2.TabIndex = 0;
			this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.button1);
			this.tabPage3.Controls.Add(this.groupBox2);
			this.tabPage3.Controls.Add(this.groupBox1);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(716, 466);
			this.tabPage3.TabIndex = 0;
			this.tabPage3.Text = "数据库设置";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(6, 6);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "保存";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.inPswd);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.inName);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.inDatabase);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.inServer);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Location = new System.Drawing.Point(360, 35);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(348, 153);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "凭证录入数据库";
			// 
			// inPswd
			// 
			this.inPswd.Location = new System.Drawing.Point(65, 120);
			this.inPswd.Name = "inPswd";
			this.inPswd.Size = new System.Drawing.Size(275, 21);
			this.inPswd.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 123);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 12);
			this.label5.TabIndex = 6;
			this.label5.Text = "密码：";
			// 
			// inName
			// 
			this.inName.Location = new System.Drawing.Point(65, 84);
			this.inName.Name = "inName";
			this.inName.Size = new System.Drawing.Size(275, 21);
			this.inName.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 90);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 12);
			this.label6.TabIndex = 4;
			this.label6.Text = "账号：";
			// 
			// inDatabase
			// 
			this.inDatabase.Location = new System.Drawing.Point(65, 48);
			this.inDatabase.Name = "inDatabase";
			this.inDatabase.Size = new System.Drawing.Size(275, 21);
			this.inDatabase.TabIndex = 3;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 52);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 2;
			this.label7.Text = "数据库：";
			// 
			// inServer
			// 
			this.inServer.Location = new System.Drawing.Point(65, 14);
			this.inServer.Name = "inServer";
			this.inServer.Size = new System.Drawing.Size(275, 21);
			this.inServer.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 18);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(53, 12);
			this.label8.TabIndex = 0;
			this.label8.Text = "服务器：";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.outPswd);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.outName);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.outDatabase);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.outServer);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(6, 35);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(348, 153);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "单据来源数据库";
			// 
			// outPswd
			// 
			this.outPswd.Location = new System.Drawing.Point(65, 120);
			this.outPswd.Name = "outPswd";
			this.outPswd.Size = new System.Drawing.Size(275, 21);
			this.outPswd.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 123);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(41, 12);
			this.label4.TabIndex = 6;
			this.label4.Text = "密码：";
			// 
			// outName
			// 
			this.outName.Location = new System.Drawing.Point(65, 84);
			this.outName.Name = "outName";
			this.outName.Size = new System.Drawing.Size(275, 21);
			this.outName.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 90);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "账号：";
			// 
			// outDatabase
			// 
			this.outDatabase.Location = new System.Drawing.Point(65, 48);
			this.outDatabase.Name = "outDatabase";
			this.outDatabase.Size = new System.Drawing.Size(275, 21);
			this.outDatabase.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "数据库：";
			// 
			// outServer
			// 
			this.outServer.Location = new System.Drawing.Point(65, 14);
			this.outServer.Name = "outServer";
			this.outServer.Size = new System.Drawing.Size(275, 21);
			this.outServer.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "服务器：";
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.panel1);
			this.tabPage4.Controls.Add(this.button4);
			this.tabPage4.Controls.Add(this.button3);
			this.tabPage4.Controls.Add(this.button2);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(716, 466);
			this.tabPage4.TabIndex = 1;
			this.tabPage4.Text = "单据管理";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.linkLabel2);
			this.panel1.Controls.Add(this.linkLabel1);
			this.panel1.Controls.Add(this.recOutSql);
			this.panel1.Controls.Add(this.recInfoSql);
			this.panel1.Controls.Add(this.recFlag);
			this.panel1.Controls.Add(this.label10);
			this.panel1.Controls.Add(this.recName);
			this.panel1.Controls.Add(this.label9);
			this.panel1.Controls.Add(this.treeView2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 35);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(710, 428);
			this.panel1.TabIndex = 3;
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Location = new System.Drawing.Point(246, 227);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(95, 12);
			this.linkLabel2.TabIndex = 10;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "单据生成凭证Sql";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(246, 38);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(71, 12);
			this.linkLabel1.TabIndex = 9;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "单据明细Sql";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// recOutSql
			// 
			this.recOutSql.Location = new System.Drawing.Point(246, 243);
			this.recOutSql.Multiline = true;
			this.recOutSql.Name = "recOutSql";
			this.recOutSql.Size = new System.Drawing.Size(461, 182);
			this.recOutSql.TabIndex = 8;
			// 
			// recInfoSql
			// 
			this.recInfoSql.Location = new System.Drawing.Point(246, 55);
			this.recInfoSql.Multiline = true;
			this.recInfoSql.Name = "recInfoSql";
			this.recInfoSql.Size = new System.Drawing.Size(461, 165);
			this.recInfoSql.TabIndex = 6;
			// 
			// recFlag
			// 
			this.recFlag.Location = new System.Drawing.Point(514, 7);
			this.recFlag.Name = "recFlag";
			this.recFlag.ReadOnly = true;
			this.recFlag.Size = new System.Drawing.Size(193, 21);
			this.recFlag.TabIndex = 4;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(467, 10);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(41, 12);
			this.label10.TabIndex = 3;
			this.label10.Text = "标识：";
			// 
			// recName
			// 
			this.recName.Location = new System.Drawing.Point(289, 6);
			this.recName.Name = "recName";
			this.recName.Size = new System.Drawing.Size(172, 21);
			this.recName.TabIndex = 2;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(246, 10);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(41, 12);
			this.label9.TabIndex = 1;
			this.label9.Text = "名称：";
			// 
			// treeView2
			// 
			this.treeView2.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView2.Location = new System.Drawing.Point(0, 0);
			this.treeView2.Name = "treeView2";
			treeNode4.Name = "节点0";
			treeNode4.Text = "单据目录";
			this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
			this.treeView2.Size = new System.Drawing.Size(240, 428);
			this.treeView2.TabIndex = 0;
			this.treeView2.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView2_NodeMouseDoubleClick);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(168, 6);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 2;
			this.button4.Text = "删除";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(87, 6);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 1;
			this.button3.Text = "保存";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(6, 6);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 0;
			this.button2.Text = "新增";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// InstallHistory
			// 
			this.InstallHistory.Name = "InstallHistory";
			this.InstallHistory.Size = new System.Drawing.Size(80, 21);
			this.InstallHistory.Text = "安装历史库";
			this.InstallHistory.Click += new System.EventHandler(this.InstallHistory_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(738, 524);
			this.Controls.Add(this.tabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "凭证生成器";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabControl2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem build;
		private System.Windows.Forms.DataGridViewCheckBoxColumn select;
		private System.Windows.Forms.DataGridViewTextBoxColumn key;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TextBox outName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox outDatabase;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox outServer;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox outPswd;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox inPswd;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox inName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox inDatabase;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox inServer;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TreeView treeView2;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox recName;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox recFlag;
		private System.Windows.Forms.TextBox recInfoSql;
		private System.Windows.Forms.TextBox recOutSql;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.ToolStripMenuItem InstallHistory;

	}
}

