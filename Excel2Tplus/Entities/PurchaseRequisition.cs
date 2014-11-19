using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excel2Tplus.Entities
{
	/// <summary>
	/// 请购单
	/// </summary>
	public class PurchaseRequisition : Entity
	{
		public string 规格型号 { get; set; }
		public string 采购单位 { get; set; }
	}
}
