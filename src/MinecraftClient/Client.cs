using System;
using System.Net.Sockets;

namespace MinecraftClient
{
	class Client
	{
		private const string Host = "127.0.0.1";
		private const int Port = 25575;
		private const string Password = "minecraft";
		private const int HeaderLength = 10; // Does not include 4-byte message length.
		private const int MaxMessageSize = 4110; // 4096 + 14 bytes of header data.

		public static Message SendMessage(NetworkStream conn, Message msg)
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
