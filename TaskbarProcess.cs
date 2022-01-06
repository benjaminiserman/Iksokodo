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

    internal TaskbarProcess(Action loop)
    {
        Icon foundIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

        ContextMenuStrip menuStrip = new()
        {
            Visible = true,
        };

        ToolStripMenuItem testButton = new()
        {
            Name = "name testing",
            Text = "text testing",
            Visible = true,
        };

        testButton.Click += new EventHandler(Exit);

        menuStrip.Items.Add(testButton);

        _trayIcon = new()
        {
            Visible = true,
            Icon = foundIcon,
            Text = "woah!?",
            BalloonTipIcon = ToolTipIcon.Info,
            BalloonTipTitle = "TestTitle",
            BalloonTipText = "woah woah woah",
            ContextMenuStrip = menuStrip,
        };

        _trayIcon.ShowBalloonTip(1);

        _trayIcon.ContextMenuStrip.Items.Add("testin2", null, (object _, EventArgs _) => { });
        _trayIcon.MouseClick += TrayRightClick;

        new Thread(() => loop()).Start();
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
        Application.Exit();
    }
}
