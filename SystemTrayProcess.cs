namespace Iksokodo;
using System;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

internal class SystemTrayProcess : ApplicationContext
{
	private static readonly Icon _normal = new("iksokodo.ico"), _suspended = new("iksokodo_suspend.ico");

	private readonly NotifyIcon _trayIcon;
	private readonly ToolStripMenuItem _toggleButton, _changeHotkeyButton, _exitButton;

	private readonly Converter _converter;
	private readonly Timer _timer;

	private const string PAUSE_MESSAGE = "Pause", RESUME_MESSAGE = "Resume", CHANGE_HOTKEY_MESSAGE = "Change Hotkey", EXIT_MESSAGE = "Exit";

	internal SystemTrayProcess()
	{
		ContextMenuStrip menuStrip = new();

		_toggleButton = new()
		{
			Name = PAUSE_MESSAGE,
			Text = PAUSE_MESSAGE,
		};

		_changeHotkeyButton = new()
		{
			Name = CHANGE_HOTKEY_MESSAGE,
			Text = CHANGE_HOTKEY_MESSAGE,
		};

		_exitButton = new()
		{
			Name = EXIT_MESSAGE,
			Text = EXIT_MESSAGE,
		};

		_toggleButton.Click += new EventHandler(ToggleLoop);
		_changeHotkeyButton.Click += new EventHandler(ChangeHotkey);
		_exitButton.Click += new EventHandler(Exit);

		menuStrip.Items.Add(_toggleButton);
		menuStrip.Items.Add(_changeHotkeyButton);
		menuStrip.Items.Add(_exitButton);

		_trayIcon = new()
		{
			Visible = true,
			Icon = _normal,
			Text = "Iksokodo",
			ContextMenuStrip = menuStrip,
		};

		_trayIcon.MouseClick += TrayRightClick;

		_converter = new();

		HotkeyManager.RegisterHotkey(ToggleLoop);

		_timer = new() { Interval = 10 };
		_timer.Elapsed += (object _, ElapsedEventArgs _) => _converter.Convert();
		_timer.Start();
	}

	private void TrayRightClick(object sender, MouseEventArgs e)
	{
		switch (e.Button)
		{
			case MouseButtons.Left:
			{
				ToggleLoop(null, null);
				break;
			}
			case MouseButtons.Right:
			{
				_trayIcon.ContextMenuStrip.Show(Cursor.Position);
				break;
			}
		}
	}

	private void ToggleLoop(object sender, EventArgs e)
	{
		bool wasPaused = _toggleButton.Name == RESUME_MESSAGE;

		_toggleButton.Name = wasPaused ? PAUSE_MESSAGE : RESUME_MESSAGE;
		_toggleButton.Text = _toggleButton.Name;

		_trayIcon.Icon = wasPaused ? _normal : _suspended;

		_converter.Paused = !wasPaused;
	}

	private void ChangeHotkey(object sender, EventArgs e)
	{
		EnterHotkeyForm form = new();

		HotkeyManager.EnableSelector(form.GetHotkeyTextbox());

		form.Show();
	}

	internal void Exit(object sender, EventArgs e)
	{
		_trayIcon.Visible = false;
		_timer.Stop();

		Application.Exit();
	}
}