using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;

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
			var cService = _client.GetService<CommandService>();

			cService.CreateCommand("roll")
				.Description("Rolls dice.")
				.Parameter("roll", ParameterType.Unparsed)
				.Do(async (e) =>
				{
					string message = $"We're still working on this feature...";
					await e.Channel.SendMessage(message);
				});

			cService.CreateCommand("follow")
				.Description("Gives the user a discord role")
				.Parameter("edition", ParameterType.Unparsed)
				.Do(async (e) =>
				{
					string[] openRoles = new string[] { "5e", "pathfinder", "3.5" };
					string edition = e.GetArg("edition");

					bool found = false;

					foreach(var role in openRoles)
					{
						if(edition.Equals(role))
						{
							found = true;
						}
					}

					if(found)
					{
						await e.User.AddRoles(e.Server.Roles.Where(x => x.Name == edition).First());
						string message = $"@{e.User.Name}, you are now following {edition}!";
						await e.Channel.SendMessage(message);
					}
					else
					{
						string message = "That role doesn't exist, try one of: ";

						foreach(var role in openRoles)
						{
							message += "[" + role + "] ";
						}

						await e.Channel.SendMessage(message);
					}
				});
		}

		public void Log(object sender, LogMessageEventArgs e)
		{
			Console.WriteLine($"[{e.Severity}] [{e.Source}]{e.Message}");
		}
	}
}
