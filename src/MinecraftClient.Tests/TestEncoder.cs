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
				Encoding.ASCII.GetBytes("seed") // Message body.
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
	}
}