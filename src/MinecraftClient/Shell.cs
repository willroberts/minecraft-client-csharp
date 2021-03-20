using System;
using System.Collections.Generic;

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
						host = args[i + 1];
						break;
					case "--port":
						port = int.Parse(args[i + 1]);
						break;
					case "--password":
						password = args[i + 1];
						break;
					default:
						break;
				}
			}

			// Connect and authenticate.
			MinecraftClient client = new MinecraftClient(host, port);
			if (!client.Authenticate(password))
			{
				Console.WriteLine("authentication failure");
				client.Close();
				return;
			}

			// Start RCON shell.
			List<String> quitCommands = new List<String> { "exit", "quit" };
			Console.WriteLine("Starting RCON shell. Use 'exit', 'quit', or Ctrl-C to exit.");
			while (true)
			{
				Console.Write("> ");
				String command = Console.ReadLine();
				if (quitCommands.Contains(command)) { break; }

				Message resp;
				if (client.SendCommand(command, out resp))
				{
					Console.WriteLine(resp.Body);
				}
				else
				{
					Console.WriteLine("Error sending command.");
					break;
				}
			}
			client.Close();
		}
	}
}
