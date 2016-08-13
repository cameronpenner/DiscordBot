using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DiscordBot
{
    public class SpellSearch
    {
        private static SpellSearch instance;
        Dictionary<string, Spell> spells;

        private SpellSearch()
        {
        }

        public static SpellSearch Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpellSearch();
                    string json = File.ReadAllText(@"resources/spells.json");
                    instance.spells = JsonConvert.DeserializeObject<Dictionary<string, Spell>>(json);
                }
                return instance;
            }
        }

        public string Search(string query)
        {
            KeyValuePair<string, Spell> result = spells.FirstOrDefault(kvp => kvp.Key.ToLower().Contains(query.ToLower()));
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
