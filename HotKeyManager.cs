namespace Iksokodo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WK.Libraries.HotkeyListenerNS;

internal static class HotKeyManager
{
	static HotkeyListener _listener;

    public static void RegisterHotKey(Action<object, EventArgs> action)
	{
		_listener = new();

		Hotkey toggleKey = new(Keys.Alt, Keys.E);

		_listener.Add(toggleKey);

		_listener.HotkeyPressed += (_, e) =>
		{
			if (e.Hotkey == toggleKey)
			{
				action(null, null);
			}
		};
	}
}
