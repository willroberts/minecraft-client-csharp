using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MinecraftClient
{
	public enum MessageType : int
	{
		Response,
		_,
		Command,
		Authenticate
	}

	public readonly struct Message
	{
		public readonly int Length;
		public readonly int ID;
		public readonly MessageType Type;
		public readonly byte[] Body;

		public Message(int length, int id, MessageType type, byte[] body)
		{
			Length = length;
			ID = id;
			Type = type;
			Body = body;
		}
	}

	class Program
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
			Message authResp = SendMessage(conn, authMsg);
			if (authResp.ID != 1)
				Console.WriteLine("authentication failure");

			// Get world seed.
			Console.WriteLine("getting world seed");
			string seedCommand = "seed";
			msgLen = seedCommand.Length + HeaderLength;
			Message seedMsg = new Message(msgLen, 2, MessageType.Command, Encoding.ASCII.GetBytes(seedCommand));
			Message seedResp = SendMessage(conn, seedMsg);
			Console.WriteLine(Encoding.ASCII.GetString(seedResp.Body));

			// Disconnect.
			Console.WriteLine("disconnecting");
			conn.Close();
			client.Close();
		}

		static Message SendMessage(NetworkStream conn, Message msg)
		{
			// Send the message.
			byte[] encoded = EncodeMessage(msg);
			conn.Write(encoded, 0, encoded.Length);

			// Receive the response.
			byte[] respBytes = new byte[MaxMessageSize];
			int bytesRead = conn.Read(respBytes, 0, respBytes.Length);
			Array.Resize(ref respBytes, bytesRead);

			// Decode and return the response.
			return DecodeMessage(respBytes);
		}

		static byte[] EncodeMessage(Message msg)
		{
			List<byte> bytes = new List<byte>();

			bytes.AddRange(BitConverter.GetBytes(msg.Length));
			bytes.AddRange(BitConverter.GetBytes(msg.ID));
			bytes.AddRange(BitConverter.GetBytes((int)msg.Type));
			bytes.AddRange(msg.Body);
			bytes.AddRange(new byte[] { 0, 0 });

			return bytes.ToArray();
		}

		static Message DecodeMessage(byte[] bytes)
		{
			int len = BitConverter.ToInt32(bytes, 0);
			int id = BitConverter.ToInt32(bytes, 4);
			int type = BitConverter.ToInt32(bytes, 8);

			int bodyLen = bytes.Length - (HeaderLength + 4);
			if (bodyLen > 0)
			{
				byte[] bodyBytes = new byte[MaxMessageSize];
				Array.Copy(bytes, 12, bodyBytes, 0, bodyLen);
				Array.Resize(ref bodyBytes, bodyLen);
				return new Message(len, id, (MessageType)type, bodyBytes);
			}
			else
			{
				return new Message(len, id, (MessageType)type, new byte[]{});
			}
		}
	}
}
