using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace starH45.net.mp3
{
	public static class Utilities
	{
		public static bool GetValue(string keyName, bool defaultValue)
		{
			return Convert.ToBoolean(GetValue(keyName, defaultValue.ToString()));
		}

		public static int GetValue(string keyName, int defaultValue)
		{
			return Convert.ToInt32(GetValue(keyName, defaultValue.ToString()));
		}

		public static object GetValue(string keyName, object defaultValue)
		{
			object o = Registry.GetValue(@"HKEY_CURRENT_USER\Software\" + Application.CompanyName + @"\" + Application.ProductName, keyName, defaultValue);
			if (o == null)
			{
				SetValue(keyName, defaultValue);
				return defaultValue;
			}
			else
			{
				return o.ToString();
			}
		}

		public static void SetValue(string keyName, object value)
		{
			Registry.SetValue(@"HKEY_CURRENT_USER\Software\" + Application.CompanyName + @"\" + Application.ProductName, keyName, value);
		}
	}
}