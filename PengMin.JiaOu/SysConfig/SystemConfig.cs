using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PengMin.JiaOu.SysConfig
{
	public class SystemConfig
	{
		public AccountInfo[] Accounts
		{
			get
			{
				return _accounts.ToArray();
			}
			set
			{
				_accounts.Clear();
				_accounts.AddRange(value);
			}
		}

		private readonly List<AccountInfo> _accounts;

		public SystemConfig()
		{
			_accounts = new List<AccountInfo>();
		}

		public void AddAccountInfo(AccountInfo account)
		{
			_accounts.Add(account);
		}

		public void RemoveAccountInfo(AccountInfo account)
		{
			_accounts.Remove(account);
		}
	}
}
