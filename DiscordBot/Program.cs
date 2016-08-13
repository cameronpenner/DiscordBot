using Discord;
using Discord.Commands;
using System;
using System.IO;

namespace DiscordBot
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			new Program().Start();
		}

		private DiscordClient _client;

		public void Start()
		{
			_client = new DiscordClient(s =>
			{
				s.AppName = "D&D";
				s.AppUrl = "";
				s.LogLevel = LogSeverity.Info;
				s.LogHandler = Log;
			});

			_client.UsingCommands(x =>
			{
				x.PrefixChar = '!';
				x.AllowMentionPrefix = true;
				x.HelpMode = HelpMode.Public;
			});

			string token = "";

			try
			{   // Open the text file using a stream reader.
				StreamReader sr = new StreamReader("token.txt");

				// Read the stream to a string, and write the string to the console.
				token = sr.ReadToEnd();

				Console.WriteLine($"Got token: {token}");
			}
			catch(Exception e)
			{
				Console.WriteLine("Token could not be found, make sure there is a token.txt");
				Console.WriteLine(e.Message);
			}

			CreateCommands();

			_client.ExecuteAndWait(async () =>
			{
				await _client.Connect(token);
			});
		}

		public void CreateCommands()
		{
			CommandService cService = _client.GetService<CommandService>();

			var commands = new Commands();
			commands.AddCommands(cService);
		}

		public void Log(object sender, LogMessageEventArgs e)
		{
			Console.WriteLine($"[{e.Severity}] [{e.Source}]{e.Message}");
		}
	}
}
