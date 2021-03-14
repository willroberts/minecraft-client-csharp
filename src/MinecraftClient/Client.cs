using System;
using System.Net.Sockets;
using System.Text;

namespace MinecraftClient
{
	class MinecraftClient
	{
		private const int MaxMessageSize = 4110; // 4096 + 14 bytes of header data.

		private TcpClient client;
		private NetworkStream conn;

		public MinecraftClient(string host, int port)
		{
			client = new TcpClient(host, port);
			conn = client.GetStream();
		}

		public Message Authenticate(string password)
		{
			return sendMessage(new Message(
				password.Length + Encoder.HeaderLength,
				1, // fix this with id generator
				MessageType.Authenticate,
				password
			));
		}

		public Message SendCommand(string command)
		{
			return sendMessage(new Message(
				command.Length + Encoder.HeaderLength,
				2, // fix this with id generator
				MessageType.Command,
				command
			));
		}

		private Message sendMessage(Message msg)
		{
			// Send the message.
			byte[] encoded = Encoder.EncodeMessage(msg);
			conn.Write(encoded, 0, encoded.Length);

			// Receive the response.
			byte[] respBytes = new byte[MaxMessageSize];
			int bytesRead = conn.Read(respBytes, 0, respBytes.Length);
			Array.Resize(ref respBytes, bytesRead);

			// Decode and return the response.
			return Encoder.DecodeMessage(respBytes);
		}
	}
}
