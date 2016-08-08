using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;

namespace DiscordBot
{C:\Users\User\Documents\GitHub\DiscordBot\DiscordBot\Program.cs
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

            /*
			cService.CreateCommand("roll")
				.Description("Rolls dice.")
				.Parameter("roll", ParameterType.Unparsed)
				.Do(async (e) =>
				{
					string message = $"We're still working on this feature...";
					await e.Channel.SendMessage(message);
				});
            */

              //Rol dice command begin
                        cService.CreateCommand("roll")
            .Description("Rolls dice. Usage: !roll 6 - rolls six sided die and returns the result.")
            .Parameter("multiplier")
            .Parameter("side")
            .Parameter("sign")
            .Parameter("modifier")
            .Do(async (e) =>
            {
                int value = 1;
                int mult = 1;
                int mod = 0;
 
                if (Int32.TryParse(e.GetArg("modifier"), out value))
                {
                    mod = Int32.Parse(e.GetArg("modifier"));
                }
 
                if (Int32.TryParse(e.GetArg("multiplier"), out value))
                {
                    //We're fine.
                    mult = Int32.Parse(e.GetArg("multiplier"));
                }
                else
                {
                    await e.Channel.SendMessage("I can't do that. Please ensure the multiplier is a whole number.");
                    await e.Channel.SendMessage("Use `!help roll` for further information.");
                    await e.Channel.SendMessage("I will use the multiplier `1` instead.");
                }
 
                if (Int32.TryParse(e.GetArg("side"), out value))
                {
                    Random rnd = new Random();
                    await e.Channel.SendMessage("Okay. Rolling " + mult + " " + Math.Abs(Int32.Parse(e.GetArg("side"))) + " sided die.");
 
                    int result = 0;
                    int[] rolls = new int[mult];
                    int temp;
                    int count = 0;
 
                    while (mult != 0)
                    {
                        temp = rnd.Next(1, Math.Abs(Int32.Parse(e.GetArg("side"))) + 1);
                        rolls[count] = temp;
                        count += 1;
                        result += temp;
                        mult--;
                    }
 
                    var results = string.Join(" + ", rolls.Select(x => x.ToString()).ToArray());
                   
                   
                    await e.Channel.SendMessage("Total: " + results + " = " + result);
                    int total;
                    if (e.GetArg("sign") == "-")
                    {
                        total = result - mod;
                        await e.Channel.SendMessage(result + " - " + mod + " = " + total);
                    }
                    else
                    {
                        total = result + mod;
                        await e.Channel.SendMessage(result + " + " + mod + " = " + total);
                    }
                }
                else
                {
                    await e.Channel.SendMessage("I can't do that. Please ensure the number of sides is a whole number.");
                    await e.Channel.SendMessage("Use `!help roll` for further information.");
                }
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
