using System.Collections.Generic;

namespace DiscordBot.model
{
	public class EncounterMonster
	{
		public string name;
		public int? qty;
	}

	public class Encounter
	{
		public List<EncounterMonster> monsters;
		public int? encounterValue;
		public int? awardValue;
	}
}
