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
			string host = DefaultHost;
			int port = DefaultPort;
			string password = DefaultPassword;

			// Parse arguments.
			for (int i = 0; i < args.Length; i++)
			{
				switch (args[i])
				{
					case "--host":
						host = args[i+1];
						break;
					case "--port":
						port = int.Parse(args[i+1]);
						break;
					case "--password":
						password = args[i+1];
						break;
					default:
						break;
				}
			}

			// Connect and authenticate.
			MinecraftClient client = new MinecraftClient(host, port);
			try
			{
				Message authResp = client.Authenticate(password);
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
