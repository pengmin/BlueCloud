using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public enum CertificateCategory
	{
		/// <summary>
		/// 盘点单凭证
		/// </summary>
		MiChkInv,
		/// <summary>
		/// 其他入库单凭证
		/// </summary>
		MiCommOver,
		/// <summary>
		/// 其他出库单凭证
		/// </summary>
		MiCommLost,
		/// <summary>
		/// 移库单凭证
		/// </summary>
		MiStockMove,
		/// <summary>
		/// 无订单入库凭证
		/// </summary>
		MINPI,
		/// <summary>
		/// 订单入库凭证
		/// </summary>
		MiPI,
		/// <summary>
		/// 应付对账单凭证
		/// </summary>
		MIAccountCfm
	}
}
