using DiscordBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DiscordBotTests
{
	[TestClass]
	public class RollCommandTests
	{
		private string ending = "**";

		[TestMethod]
		public void single_number()
		{
			var roll = new RollCommand();

			string message = roll.Roll("5");
			Console.WriteLine(message);

			Assert.IsTrue(message.EndsWith("5" + ending));
		}

		[TestMethod]
		public void negative_number()
		{
			var roll = new RollCommand();

			string message = roll.Roll("-5");
			Console.WriteLine(message);

			Assert.IsTrue(message.EndsWith("-5" + ending));
		}

		[TestMethod]
		public void single_die()
		{
			var roll = new RollCommand();

			string message = roll.Roll("4d1");
			Console.WriteLine(message);

			Assert.IsTrue(message.EndsWith("4" + ending));
		}

		[TestMethod]
		public void negative_die()
		{
			var roll = new RollCommand();

			string message = roll.Roll("-3d1");
			Console.WriteLine(message);

			Assert.IsTrue(message.EndsWith("-3" + ending));
		}
	}
}
