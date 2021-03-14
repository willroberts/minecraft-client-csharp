using MinecraftClient;
using NUnit.Framework;
using System.Text;

namespace MinecraftClient.Tests.Encoder
{
	[TestFixture]
	public class EncoderTests
	{
		[Test]
		public void EncodeMessageTest()
		{
			MinecraftClient.Message msg = new MinecraftClient.Message(
				14, // Message length.
				1, // Message ID.
				(MinecraftClient.MessageType)2, // Message type.
				"seed" // Message body.
			);
			byte[] bytes = MinecraftClient.Encoder.EncodeMessage(msg);
			byte[] expected = new byte[] {
				14, 0, 0, 0, // Message length.
				1, 0, 0, 0, // Message ID.
				2, 0, 0, 0, // Message type.
				115, 101, 101, 100, // Message body.
				0, 0, // Terminator.
			};
			Assert.AreEqual(bytes, expected);
		}

		[Test]
		public void DecodeMessageTest()
		{
			byte[] bytes = new byte[] {
				38, 0, 0, 0, //  Message length.
				2, 0, 0, 0, // Message ID.
				0, 0, 0, 0, // Message type.
				83, 101, 101, 100, 58, 32, 91, 45, 50, 52, 55, 52, 49, 50, 53, 53, 55, 52, 56, 57, 48, 54, 57, 50, 51, 48, 56, 93, // Message body.
				0, 0 // Terminator.
			};
			MinecraftClient.Message msg = MinecraftClient.Encoder.DecodeMessage(bytes);
			MinecraftClient.Message expected = new MinecraftClient.Message(
				38, // Message length.
				2, // Message ID.
				0, // Message type.
				"Seed: [-2474125574890692308]" // Message body.
			);
			Assert.AreEqual(msg.Length, expected.Length);
			Assert.AreEqual(msg.ID, expected.ID);
			Assert.AreEqual(msg.Type, expected.Type);
			Assert.AreEqual(msg.Body, expected.Body);
		}
	}
}