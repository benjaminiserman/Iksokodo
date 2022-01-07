namespace Iksokodo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

internal class TaskbarProcess : ApplicationContext
{
	private NotifyIcon _trayIcon;
    private Thread _loopThread;
    private ToolStripMenuItem _toggleButton, _exitButton;

    private static readonly Icon _normal = new("iksokodo.ico"), _suspended = new("iksokodo_suspend.ico");

    private const string PAUSE_MESSAGE = "Pause", RESUME_MESSAGE = "Resume";

    internal TaskbarProcess(Action loop)
    {
        ContextMenuStrip menuStrip = new();

        _toggleButton = new()
        {
            Name = PAUSE_MESSAGE,
            Text = PAUSE_MESSAGE,
        };

        _exitButton = new()
        {
            Name = "Exit",
            Text = "Exit",
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

        _loopThread = new Thread(() => loop());
        _loopThread.Start();
    }
    
    private void ToggleLoop(object sender, EventArgs e)
	{
        bool wasPaused = _toggleButton.Name == RESUME_MESSAGE;

        _toggleButton.Name = wasPaused ? PAUSE_MESSAGE : RESUME_MESSAGE;
        _toggleButton.Text = _toggleButton.Name;

        Program.sleep = !wasPaused;
        _trayIcon.Icon = wasPaused ? _normal : _suspended;

        if (!Program.sleep) _loopThread.Interrupt();
	}

    private void TrayRightClick(object sender, MouseEventArgs e)
	{
        if (e.Button == MouseButtons.Right)
        {
            _trayIcon.ContextMenuStrip.Show(Cursor.Position);
        }
    }

    internal void Exit(object sender, EventArgs e)
    {
        _trayIcon.Visible = false;
        Program.sleep = true;
        Thread.Sleep(10);
        Program.abort = true;
        _loopThread.Interrupt();
        Application.Exit();
    }
}
