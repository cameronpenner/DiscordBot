using DiscordBot.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace DiscordBot
{
	public class EncounterCommand : Singleton<EncounterCommand>
	{
		public string GenerateEncounter(string parameters)
		{
			string errorMessage = "error, use the format: !encounter 1 2 2 4";

			string difficulty = "Medium";
			string environment = "All";

			try
			{
				string URL = "http://tools.goblinist.com/g/encounter?";

				string[] tokens = parameters.Split(' ');

				foreach(var token in tokens)
				{
					int level = int.Parse(token);

					URL += $"players[]={level}&";
				}

				URL += $"difficulty={difficulty}&gType=mixed&environment={environment}&basicRulesOnly=false&min=1&max=100";

				WebClient client = new WebClient();
				string downloadString = client.DownloadString(URL);

				dynamic encounter = JObject.Parse(downloadString);

				string message = $"**Encounter, {encounter.awardValue}xp**\n\n";

				foreach(var monster in encounter.monsters)
				{
					message += monster + "\n\n";
				}

				return message;
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return errorMessage;
			}
		}
	}
}
