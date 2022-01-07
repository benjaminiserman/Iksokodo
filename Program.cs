namespace Iksokodo;
internal static class Program
{
	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize();

		TaskbarProcess taskBarProcess = new();

		AppDomain.CurrentDomain.ProcessExit += new EventHandler(taskBarProcess.Exit);

		Application.Run(taskBarProcess);
	}
}