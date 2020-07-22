using GameClient.Types.Map;
using GameServer.General;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameClient.Managers
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

        public static HashSet<Map> GetMapInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet<Map>>(GetFileFromString(FileName), new StringEnumConverter());
        }

    }
}
