using Discord.Commands;
using System.Linq;

namespace DiscordBot
{
	internal class Commands
	{
		public void AddCommands(CommandService cService)
		{
			//Rol dice command begin
			cService.CreateCommand("roll")
				.Description("Rolls dice. Usage: !roll 6 - rolls six sided die and returns the result.")
				.Parameter("parameters", ParameterType.Unparsed)
				.Do(async (e) =>
				{
					var roll = new RollCommand();
					string message = roll.Roll(e.GetArg("parameters"));

					await e.Channel.SendMessage(message);
				});

			cService.CreateCommand("spell")
				.Description("Searches all spells with !spell query")
				.Parameter("parameters", ParameterType.Unparsed)
				.Do(async (e) =>
				{
					var spell = SpellSearch.Instance;
					string message = spell.Search(e.GetArg("parameters"));

					await e.Channel.SendMessage(message);
				});

			cService.CreateCommand("monster")
				.Description("Searches all monsters with !monster query")
				.Parameter("parameters", ParameterType.Unparsed)
				.Do(async (e) =>
				{
					var monster = MonsterSearch.Instance;
					string message = monster.Search(e.GetArg("parameters"));

					await e.Channel.SendMessage(message);
				});

			cService.CreateCommand("follow")
				.Description("Gives the user a discord role")
				.Parameter("edition", ParameterType.Unparsed)
				.Do(async (e) =>
				{
					string[] openRoles = new string[] { "5e", "pathfinder", "3.5", "player", "dungeonMaster" };
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
	}
}
