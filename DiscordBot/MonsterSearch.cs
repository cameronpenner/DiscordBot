using DiscordBot.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscordBot
{
	public class MonsterSearch : Singleton<MonsterSearch>
	{
		private List<Monster> monsters;

		public MonsterSearch()
		{
			string json = File.ReadAllText(@"resources/monsters.json");
			monsters = JsonConvert.DeserializeObject<List<Monster>>(json);
		}

		public string Search(string query)
		{
			Monster result = monsters
							.Where(m => m.name.ToLower().Contains(query.ToLower()))
							.OrderBy(m => LevenshteinDistance.Compute(m.name, query))
							.First();

			if(result == null)
			{
				return "I am sorry to inform you that no monster could be found, please check for typos";
			}
			var actions = result.actions.Select(action => action.ToString() + "\t");
			StringBuilder MyStringBuilder = new StringBuilder();
			MyStringBuilder.AppendLine("**Name:** *" + result.name + "*");
			MyStringBuilder.AppendLine("**Size:** " + result.size);
			MyStringBuilder.AppendLine("**Type:** " + result.type);
			MyStringBuilder.AppendLine("**Alignment:** " + result.alignment);
			MyStringBuilder.Append("**AC:** " + result.armor_class);
			MyStringBuilder.Append("  **HP:** " + result.hit_points);
			MyStringBuilder.AppendLine("  **Hit Dice:** " + result.hit_dice);
			MyStringBuilder.AppendLine("**Speed:** " + result.speed);

			MyStringBuilder.AppendLine("\n__**Attacks:**__ ");
			foreach(String action in actions)
			{
				MyStringBuilder.Append(action);
			}

			MyStringBuilder.Append("\n**Strength:** " + result.strength);
			MyStringBuilder.Append("\t**Dexterity:** " + result.dexterity);
			MyStringBuilder.Append("\t**Constitution:** " + result.constitution);
			MyStringBuilder.Append("\t**Intelligence:** " + result.intelligence);
			MyStringBuilder.Append("\t**Wisdom:** " + result.wisdom);
			MyStringBuilder.AppendLine("\t**Charisma:** " + result.charisma);

			return MyStringBuilder.ToString();
		}
	}
}
