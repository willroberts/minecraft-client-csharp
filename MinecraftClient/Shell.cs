using System;
using System.Net.Sockets;
using System.Text;

namespace MinecraftClient
{
	class Shell
	{
		private const string Host = "127.0.0.1";
		private const int Port = 25575;
		private const string Password = "minecraft";
		private const int HeaderLength = 10; // Does not include 4-byte message length.
		private const int MaxMessageSize = 4110; // 4096 + 14 bytes of header data.

		static void Main(string[] args)
		{
			// Connect.
			Console.WriteLine("establishing connection");
			TcpClient client = new TcpClient(Host, Port);
			NetworkStream conn = client.GetStream();

			// Authenticate.
			Console.WriteLine("authenticating");
			int msgLen = Password.Length + HeaderLength;
			Message authMsg = new Message(msgLen, 1, MessageType.Authenticate, Encoding.ASCII.GetBytes(Password));
			Message authResp = Client.SendMessage(conn, authMsg);
			if (authResp.ID != 1)
				Console.WriteLine("authentication failure");

			// Get world seed.
			Console.WriteLine("getting world seed");
			string seedCommand = "seed";
			msgLen = seedCommand.Length + HeaderLength;
			Message seedMsg = new Message(msgLen, 2, MessageType.Command, Encoding.ASCII.GetBytes(seedCommand));
			Message seedResp = Client.SendMessage(conn, seedMsg);
			Console.WriteLine(Encoding.ASCII.GetString(seedResp.Body));

			// Disconnect.
			Console.WriteLine("disconnecting");
			conn.Close();
			client.Close();
		}
	}
}
