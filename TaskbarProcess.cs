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
	private readonly NotifyIcon _trayIcon;

    internal TaskbarProcess()
    {
        Icon foundIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

        _trayIcon = new()
        {
            Visible = true,
            Icon = foundIcon,
            Text = "woah?",
            BalloonTipIcon = ToolTipIcon.Info,
            BalloonTipTitle = "TestTitle",
            BalloonTipText = "woah woah woah",
        };
    }

    internal void Exit(object sender, EventArgs e)
    {
        _trayIcon.Visible = false;
        Application.Exit();
    }
}
