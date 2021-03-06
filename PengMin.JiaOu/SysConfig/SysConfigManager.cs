﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PengMin.Infrastructure;

namespace PengMin.JiaOu.SysConfig
{
	/// <summary>
	/// 系统配置管理器
	/// </summary>
	class SysConfigManager
	{
		private string ConfigPath
		{
			get { return Environment.CurrentDirectory + "\\SysConfig.xml"; }
		}

		/// <summary>
		/// 设置系统配置
		/// </summary>
		/// <param name="config">系统配置对象</param>
		public void Set(SystemConfig config)
		{
			var sw = CommonFunction.XmlSerializer(config);
			using (var file = File.OpenWrite(ConfigPath))
			{
				var buf = Encoding.UTF8.GetBytes(sw.ToString());
				file.Write(buf, 0, buf.Length);
				file.SetLength(buf.Length);
				file.Close();
			}
		}

		/// <summary>
		/// 获取系统配置
		/// </summary>
		/// <returns></returns>
		public SystemConfig Get()
		{
			if (!File.Exists(ConfigPath))
			{
				File.Create(ConfigPath).Close();
			}
			using (var file = File.OpenText(ConfigPath))
			{
				var str = file.ReadToEnd();
				file.Close();
				if (string.IsNullOrWhiteSpace(str))
				{
					return new SystemConfig();
				}
				var sr = new StringReader(str);
				var sc = CommonFunction.XmlDeserialize<SystemConfig>(sr);
				return sc;
			}
		}
	}
}
