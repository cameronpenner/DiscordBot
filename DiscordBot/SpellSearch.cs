using DiscordBot.utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscordBot
{
	public class SpellSearch : Singleton<SpellSearch>
	{
		private Dictionary<string, Spell> spells;

		public SpellSearch()
		{
			string json = File.ReadAllText(@"resources/spells.json");
			spells = JsonConvert.DeserializeObject<Dictionary<string, Spell>>(json);
		}

		public string Search(string query)
		{
			KeyValuePair<string, Spell> result = spells
							.Where(m => m.Key.ToLower().Contains(query.ToLower()))
							.OrderBy(m => LevenshteinDistance.Compute(m.Key, query))
							.First();
			if(result.Key == null)
			{
				return "I am sorry to inform you that no spell could be found, please check for typos";
			}
			StringBuilder MyStringBuilder = new StringBuilder();
			MyStringBuilder.AppendLine("**Spell:** *" + result.Key + "*");
			MyStringBuilder.AppendLine("**Description:** " + result.Value.description);
			MyStringBuilder.AppendLine("**Casting Time:** " + result.Value.casting_time);
			MyStringBuilder.AppendLine("**Range:** " + result.Value.range);
			MyStringBuilder.AppendLine("**Level:** " + result.Value.level);
			MyStringBuilder.AppendLine("**School:** " + result.Value.school);
			return MyStringBuilder.ToString();
		}
	}
}
