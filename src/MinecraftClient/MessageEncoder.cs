using System;
using System.Collections.Generic;
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
		public readonly String Body;

		public Message(int length, int id, MessageType type, String body)
		{
			Length = length;
			ID = id;
			Type = type;
			Body = body;
		}
	}

	public class Encoder
	{
		public const int HeaderLength = 10; // Does not include 4-byte message length.

		public static byte[] EncodeMessage(Message msg)
		{
			List<byte> bytes = new List<byte>();

			bytes.AddRange(BitConverter.GetBytes(msg.Length));
			bytes.AddRange(BitConverter.GetBytes(msg.ID));
			bytes.AddRange(BitConverter.GetBytes((int)msg.Type));
			bytes.AddRange(Encoding.ASCII.GetBytes(msg.Body));
			bytes.AddRange(new byte[] { 0, 0 });

			return bytes.ToArray();
		}

		public static Message DecodeMessage(byte[] bytes)
		{
			int len = BitConverter.ToInt32(bytes, 0);
			int id = BitConverter.ToInt32(bytes, 4);
			int type = BitConverter.ToInt32(bytes, 8);

			int bodyLen = bytes.Length - (HeaderLength + 4);
			if (bodyLen > 0)
			{
				byte[] bodyBytes = new byte[bodyLen];
				Array.Copy(bytes, 12, bodyBytes, 0, bodyLen);
				Array.Resize(ref bodyBytes, bodyLen);
				return new Message(len, id, (MessageType)type, Encoding.ASCII.GetString(bodyBytes));
			}
			else { return new Message(len, id, (MessageType)type, ""); }
		}
	}
}
