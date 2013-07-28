using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public class Factory
	{
		public static ICertificateOutRepository GetRepository(CertificateCategory category, AdoProxy ado)
		{
			switch (category)
			{
				default:
					return null;
			}
		}
	}
}
