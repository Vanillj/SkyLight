﻿using GameClient.Types.Item;
using GameServer.General;
using GameServer.Types.Item;
using GameServer.Types.Map;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;

namespace GameServer.Managers
{
    class FileManager
    {
        private static string GetFileFromString(string FileName)
        {
            using (var file = File.OpenText(FileName))
            {
                return file.ReadToEnd();
            }
        }
        
        public static CredentialInfo GetCredentialInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<CredentialInfo>(GetFileFromString(FileName));
        }

        public static HashSet<WeaponItem> GetItemInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet<WeaponItem>>(GetFileFromString(FileName));
        }

        public static HashSet<Map> GetMapInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet<Map>>(GetFileFromString(FileName), new StringEnumConverter());
        }

    }
}
