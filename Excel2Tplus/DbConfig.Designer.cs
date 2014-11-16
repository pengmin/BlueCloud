namespace Excel2Tplus
{
	partial class DbConfig
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.server = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.user = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.password = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.database = new System.Windows.Forms.TextBox();
			this.ok = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(55, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "服务器：";
			// 
			// server
			// 
			this.server.Location = new System.Drawing.Point(66, 10);
			this.server.Name = "server";
			this.server.Size = new System.Drawing.Size(214, 20);
			this.server.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "账户名：";
			// 
			// user
			// 
			this.user.Location = new System.Drawing.Point(66, 40);
			this.user.Name = "user";
			this.user.Size = new System.Drawing.Size(214, 20);
			this.user.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "密码：";
			// 
			// password
			// 
			this.password.Location = new System.Drawing.Point(66, 69);
			this.password.Name = "password";
			this.password.Size = new System.Drawing.Size(214, 20);
			this.password.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(5, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "数据库：";
			// 
			// database
			// 
			this.database.Location = new System.Drawing.Point(66, 101);
			this.database.Name = "database";
			this.database.Size = new System.Drawing.Size(214, 20);
			this.database.TabIndex = 7;
			// 
			// ok
			// 
			this.ok.Location = new System.Drawing.Point(205, 132);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(75, 23);
			this.ok.TabIndex = 8;
			this.ok.Text = "确定";
			this.ok.UseVisualStyleBackColor = true;
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// DbConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 161);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.database);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.password);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.user);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.server);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "DbConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "数据库设置";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox server;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox user;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox password;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox database;
		private System.Windows.Forms.Button ok;
	}
}