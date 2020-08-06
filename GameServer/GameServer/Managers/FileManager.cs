using GameClient.Types.Item;
using GameServer.General;
using GameServer.Types.Abilities;
using GameServer.Types.Abilities.SharedAbilities;
using GameServer.Types.Item;
using GameServer.Types.Map;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;

namespace GameServer.Managers
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

        public static HashSet<WeaponItem> GetItemInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet<WeaponItem>>(GetFileFromString(FileName));
        }

        public static HashSet<Map> GetMapInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet<Map>>(GetFileFromString(FileName), new StringEnumConverter());
        }
        public static AbilityFormater GetAbilityInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AbilityFormater>(GetFileFromString(FileName));
        }

    }
}
