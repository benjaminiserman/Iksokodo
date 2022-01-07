namespace Iksokodo;
using System;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

internal class TaskbarProcess : ApplicationContext
{
    private static readonly Icon _normal = new("iksokodo.ico"), _suspended = new("iksokodo_suspend.ico");

    private readonly NotifyIcon _trayIcon;
	private readonly ToolStripMenuItem _toggleButton, _exitButton;

    private readonly Converter _converter;
    private readonly Timer _timer;

    private const string PAUSE_MESSAGE = "Pause", RESUME_MESSAGE = "Resume", EXIT_MESSAGE = "Exit";

    internal TaskbarProcess()
    {
        ContextMenuStrip menuStrip = new();

        _toggleButton = new()
        {
            Name = PAUSE_MESSAGE,
            Text = PAUSE_MESSAGE,
        };

        _exitButton = new()
        {
            Name = EXIT_MESSAGE,
            Text = EXIT_MESSAGE,
        };

        _toggleButton.Click += new EventHandler(ToggleLoop);
        _exitButton.Click += new EventHandler(Exit);
       
        menuStrip.Items.Add(_toggleButton);
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

    internal void Exit(object sender, EventArgs e)
    {
        _trayIcon.Visible = false;
        _timer.Stop();
        
        Application.Exit();
    }
}