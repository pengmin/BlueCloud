using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class CertificateOutFactory
	{
		public static ICertificateOutRepository GetRepository(CertificateCategory category, AdoProxy ado)
		{
			switch (category)
			{
				case CertificateCategory.MiChkInv:
					return new MiChkInvOutRepository(ado);
				case CertificateCategory.MiCommOver:
					return new MiCommOverOutRepository(ado);
				case CertificateCategory.MiCommLost:
					return new MiCommLostOutRepository(ado);
				case CertificateCategory.MiStockMove:
					return new MiStockMoveOutRepository(ado);
				case CertificateCategory.MINPI:
					return new MINPIOutRepository(ado);
				case CertificateCategory.MiPI:
					return new MiPIOutRepository(ado);
				case CertificateCategory.MIAccountCfm:
					return new MIAccountCfmOutRepository(ado);
				default:
					return null;
			}
		}
	}
}
