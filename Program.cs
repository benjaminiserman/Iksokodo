namespace Iksokodo;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

internal static class Program
{
	[DllImport("User32.dll")]
	private static extern short GetAsyncKeyState(Keys ArrowKeys);

	private static readonly Keys[] _possibleKeys = Enum.GetValues<Keys>();

	private static readonly InputSimulator _inputSimulator = new();

	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize();
		
		TaskbarProcess taskBarProcess = new(Loop);

		AppDomain.CurrentDomain.ProcessExit += new EventHandler(taskBarProcess.Exit);
		
		Application.Run(taskBarProcess);
	}

	static void Loop()
	{
		(KeyReceived key, bool shift)? current = null;

		while (true)
		{
			(KeyReceived key, bool shift) received = GetKey();

			if (received is (not KeyReceived.Other and not KeyReceived.X and not KeyReceived.Apostrophe, _) or (KeyReceived.Apostrophe, false))
			{
				current = received;
			}
			else
			{
				if (current is (KeyReceived, bool) valid && received is (KeyReceived.X, false))
				{
					if (valid.key is KeyReceived.Apostrophe)
					{
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);
					}
					else
					{
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
						_inputSimulator.Keyboard.TextEntry(ConvertAndCapitalize(valid));
					}
				}

				current = null;
			}
		}
	}

	private static (KeyReceived key, bool shift) GetKey()
	{
		bool shift = GetAsyncKeyState(Keys.Shift) != 0;

		while (true)
		{
			foreach (Keys key in _possibleKeys)
			{
				if (GetAsyncKeyState(key) == -32767)
				{
					return (Convert(key), shift);
				}
			}
		}
	} 

	private static KeyReceived Convert(Keys key) => key switch
	{
		Keys.C => KeyReceived.C,
		Keys.G => KeyReceived.G,
		Keys.H => KeyReceived.H,
		Keys.J => KeyReceived.J,
		Keys.S => KeyReceived.S,
		Keys.U => KeyReceived.U,
		Keys.X => KeyReceived.X,
		Keys.Oem7 => KeyReceived.Apostrophe,
		_ => KeyReceived.Other,
	};
	
	private static char ConvertAndCapitalize((KeyReceived key, bool shift) received)
	{
		char c = received.key switch
		{
			KeyReceived.C => 'ĉ',
			KeyReceived.G => 'ĝ',
			KeyReceived.H => 'ĥ',
			KeyReceived.J => 'ĵ',
			KeyReceived.S => 'ŝ',
			KeyReceived.U => 'ŭ',
			_ => throw new InvalidOperationException("Cannot convert KeyReceived.X or KeyReceived.Other into a char.")
		};

		if (received.shift) c = char.ToUpper(c);

		return c;
	}

	private enum KeyReceived { C, G, H, J, S, U, X, Apostrophe, Other }
}