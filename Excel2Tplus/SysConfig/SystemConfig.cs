using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.SysConfig
{
	/// <summary>
	/// 系统配置对象
	/// </summary>
	public class SystemConfig
	{
		/// <summary>
		/// 导出历史记录
		/// </summary>
		public string Excel2TplusHistorySql
		{
			get { return _excel2TplusHistorySql; }
			set { _excel2TplusHistorySql = value; }
		}
		private string _excel2TplusHistorySql =
@"CREATE TABLE [dbo].[Excel2TplusHistory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[datetime] [datetime] NOT NULL,
	[type] [varchar](50) NOT NULL,
	[xml] [xml] NOT NULL,
	[Name] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Excel2TplusHistory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
		/// <summary>
		/// 数据库配置
		/// </summary>
		public DatabaseConfig DbConfig { get; set; }
		/// <summary>
		/// 是否配置了数据库
		/// </summary>
		public bool HasDbConfig
		{
			get { return DbConfig != null && !string.IsNullOrWhiteSpace(DbConfig.Server); }
		}

		public SystemConfig()
		{
			DbConfig = new DatabaseConfig();
		}
		/// <summary>
		/// 数据库配置
		/// </summary>
		public class DatabaseConfig
		{
			private const string ConnStr = @"Data Source={0};Initial Catalog={1};User ID={2};Password={3}";

			/// <summary>
			/// 服务器
			/// </summary>
			public string Server { get; set; }
			/// <summary>
			/// 账户名称
			/// </summary>
			public string UserName { get; set; }
			/// <summary>
			/// 密码
			/// </summary>
			public string Password { get; set; }
			/// <summary>
			/// 数据库名称
			/// </summary>
			public string Database { get; set; }

			/// <summary>
			/// 获取数据库连接字符串
			/// </summary>
			/// <returns></returns>
			public string GetConnectionString()
			{
				return string.Format(ConnStr, Server, Database, UserName, Password);
			}
		}
	}
}
