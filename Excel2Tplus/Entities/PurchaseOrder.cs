﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 采购订单
	/// </summary>
	class PurchaseOrder : Entity
	{
		public string 仓库 { get; set; }
		public string 供应商 { get; set; }
		public string 供应商名称 { get; set; }
		public string 项目 { get; set; }
		public string 部门 { get; set; }
		public string 业务员 { get; set; }
		public string 规格型号 { get; set; }
		public string 采购单位 { get; set; }
	}
}
