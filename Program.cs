namespace Iksokodo;
using System.Text.Json;

internal static class Program
{
	public static Config Config { get; private set; }
	public const string CONFIG_PATH = @"config.json";

	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize();

		if (!File.Exists(CONFIG_PATH))
		{
			Config = new Config();
			File.WriteAllText(CONFIG_PATH, JsonSerializer.Serialize(Config));
		}
		else Config = JsonSerializer.Deserialize<Config>(File.ReadAllText(CONFIG_PATH));

		SystemTrayProcess taskBarProcess = new();

		AppDomain.CurrentDomain.ProcessExit += new EventHandler(taskBarProcess.Exit);

		Application.Run(taskBarProcess);
	}
}