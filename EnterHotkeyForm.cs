namespace Iksokodo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class EnterHotkeyForm : Form
{
	public EnterHotkeyForm()
	{
		InitializeComponent();
	}

	private void SubmitButton_Click(object sender, EventArgs e)
	{
		HotkeyManager.UpdateHotkey(HotkeyTextBox.Text);
		Close();
	}

	public Control GetHotkeyTextbox() => HotkeyTextBox;
}
