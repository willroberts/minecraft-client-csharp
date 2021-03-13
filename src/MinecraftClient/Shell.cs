using System;
using System.Net.Sockets;
using System.Text;

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
			Message authResp = client.Authenticate(DefaultPassword);
			if (authResp.ID != 1)
				Console.WriteLine("authentication failure");

			// Start RCON shell.
			Console.WriteLine("Starting RCON shell. Use 'exit', 'quit', or Ctrl-C to exit.");

			// Get world seed.
			Message seedResp = client.SendCommand("seed");
			Console.WriteLine(Encoding.ASCII.GetString(seedResp.Body));
		}
	}
}
