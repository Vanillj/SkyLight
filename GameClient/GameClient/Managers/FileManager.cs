using GameClient.Types.Abilities;
using GameClient.Types.KeyBinding;
using GameClient.Types.Map;
using GameServer.General;
using GameServer.Managers;
using GameServer.Types.Abilities.SharedAbilities;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameClient.Managers
{
    class FileManager : FileManagerHead
    {
        private static string GetFileFromString(string FileName)
        {
            using (var file = File.OpenText(FileName))
            {
                return file.ReadToEnd();
            }
        }

        public static HashSet<Map> GetMapInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet<Map>>(GetFileFromString(FileName), new StringEnumConverter());
        }

        public static List<KeyBind> GetKeyBinds(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyBind>>(GetFileFromString(FileName), new StringEnumConverter());
        }

        public static AbilityformaterClient GetAbilityInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AbilityformaterClient>(GetFileFromString(FileName), new StringEnumConverter());
        }

    }
}
