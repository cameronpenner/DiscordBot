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

        public string Search(string query)
        {
            Dictionary <string, Spell> spells = map();
            KeyValuePair<string, Spell> result = spells.FirstOrDefault(kvp => kvp.Key.ToLower().Contains(query.ToLower()));
            StringBuilder MyStringBuilder = new StringBuilder();
            MyStringBuilder.AppendLine("> **Spell:** *" + result.Key + "*");
            MyStringBuilder.AppendLine("> **Description:** " + result.Value.description);
            MyStringBuilder.AppendLine("> **Casting Time:** " + result.Value.casting_time);
            MyStringBuilder.AppendLine("> **Range:** " + result.Value.range);
            MyStringBuilder.AppendLine("> **Level:** " + result.Value.level);
            MyStringBuilder.AppendLine("> **School:** " + result.Value.school);
            return MyStringBuilder.ToString();
            

        }

        private Dictionary<string, Spell> map()
        {
            JsonSerializer serializer = new JsonSerializer();
            using(var client = new WebClient()){
                var json = client.DownloadString("https://raw.githubusercontent.com/tadzik/5e-spells/master/spells.json");
                return JsonConvert.DeserializeObject<Dictionary<string, Spell>>(json);
            }
            
        }
    }
}
