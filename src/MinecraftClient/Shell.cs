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
			Console.WriteLine("Starting RCON shell. Use 'exit', 'quit', or Ctrl-C to exit.");
			TcpClient client = new TcpClient(DefaultHost, DefaultPort);
			NetworkStream conn = client.GetStream();

			// Authenticate.
			int msgLen = DefaultPassword.Length + Encoder.HeaderLength;
			Message authMsg = new Message(msgLen, 1, MessageType.Authenticate, Encoding.ASCII.GetBytes(DefaultPassword));
			Message authResp = Client.SendMessage(conn, authMsg);
			if (authResp.ID != 1)
				Console.WriteLine("authentication failure");

			// Get world seed.
			string seedCommand = "seed";
			msgLen = seedCommand.Length + Encoder.HeaderLength;
			Message seedMsg = new Message(msgLen, 2, MessageType.Command, Encoding.ASCII.GetBytes(seedCommand));
			Message seedResp = Client.SendMessage(conn, seedMsg);
			Console.WriteLine(Encoding.ASCII.GetString(seedResp.Body));

			// Disconnect.
			conn.Close();
			client.Close();
		}
	}
}
