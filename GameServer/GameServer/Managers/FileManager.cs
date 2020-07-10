using GameServer.General;
using GameServer.Types.Item;
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

        public static HashSet<ItemBase> GetItemInformation(string FileName)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<HashSet <ItemBase>>(GetFileFromString(FileName));
        }

    }
}
