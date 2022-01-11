namespace Iksokodo;
using System;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

internal class Converter
{
	[DllImport("User32.dll")]
	private static extern short GetAsyncKeyState(Keys ArrowKeys);

	private static readonly Keys[] _possibleKeys = Enum.GetValues<Keys>();

	private readonly InputSimulator _inputSimulator = new();

	private Key? _cached = null;

	public bool Paused { get; set; }

	public ManualResetEvent Handle { get; private set; } = new(false);

	public void Convert()
	{
		if (Paused) return;

		Key? received = GetKey();

		if (received is Key receivedKey)
		{
			if (receivedKey.IsStart || receivedKey.IsApostrophe)
			{
				_cached = received;
			}
			else
			{
				if (_cached is Key cachedKey && receivedKey.IsX)
				{
					if (cachedKey.IsApostrophe) // c'x => cx
					{
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);
					}
					else // cx => ĉ
					{
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
						_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
						_inputSimulator.Keyboard.TextEntry((char)cachedKey);
					}
				}

				_cached = null;
			}
		}
	}
	
	private static Key? GetKey()
	{
		bool shift = GetAsyncKeyState(Keys.ShiftKey) != 0;

		foreach (Keys key in _possibleKeys)
		{
			if (GetAsyncKeyState(key) == -32767) // -32767 == 0b10000000_00000001 => key is down and was pressed since last query
			{
				return new Key(key, shift);
			}
		}

		return null;
	}
}