using System;
using System.Net.Sockets;
using System.Threading;

namespace MinecraftClient
{
	class RequestIDMismatchException : Exception { public RequestIDMismatchException() { } }

	class MinecraftClient
	{
		private const int MaxMessageSize = 4110; // 4096 + 14 bytes of header data.

		private TcpClient client;
		private NetworkStream conn;
		private int lastID = 0;

		public MinecraftClient(string host, int port)
		{
			client = new TcpClient(host, port);
			conn = client.GetStream();
		}

		public void Close()
		{
			conn.Close();
			client.Close();
		}

		public Message Authenticate(string password)
		{
			return sendMessage(new Message(
				password.Length + Encoder.HeaderLength,
				Interlocked.Increment(ref lastID),
				MessageType.Authenticate,
				password
			));
		}

		public Message SendCommand(string command)
		{
			return sendMessage(new Message(
				command.Length + Encoder.HeaderLength,
				Interlocked.Increment(ref lastID),
				MessageType.Command,
				command
			));
		}

		private Message sendMessage(Message req)
		{
			// Send the message.
			byte[] encoded = Encoder.EncodeMessage(req);
			conn.Write(encoded, 0, encoded.Length);

			// Receive the response.
			byte[] respBytes = new byte[MaxMessageSize];
			int bytesRead = conn.Read(respBytes, 0, respBytes.Length);
			Array.Resize(ref respBytes, bytesRead);

			// Decode the response and check for errors before returning.
			Message resp = Encoder.DecodeMessage(respBytes);
			if (req.ID != resp.ID)
			{
				throw new RequestIDMismatchException();
			}
			return resp;
		}
	}
}
