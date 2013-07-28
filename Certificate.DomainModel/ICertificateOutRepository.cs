using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Certificate.DomainModel
{
	public interface ICertificateOutRepository
	{
		Certificate GetCertificate(string id);
	}
}
