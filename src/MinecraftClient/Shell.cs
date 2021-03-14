using System;

namespace MinecraftClient
{
	class Shell
	{
		private const string DefaultHost = "127.0.0.1";
		private const int DefaultPort = 25575;
		private const string DefaultPassword = "minecraft";

		static void Main(string[] args)
		{
			// Connect.
			MinecraftClient client = new MinecraftClient(DefaultHost, DefaultPort);

			// Authenticate.
			try
			{
				Message authResp = client.Authenticate(DefaultPassword);
			}
			catch (RequestIDMismatchException)
			{
				Console.WriteLine("authentication failure");
				return;
			}

			// Start RCON shell.
			Console.WriteLine("Starting RCON shell. Use 'exit', 'quit', or Ctrl-C to exit.");

			// Get world seed.
			Message seedResp = client.SendCommand("seed");
			Console.WriteLine(seedResp.Body);
		}
	}
}
