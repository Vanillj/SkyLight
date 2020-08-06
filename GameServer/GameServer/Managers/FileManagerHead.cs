using GameServer.General;
using GameServer.Types.Abilities;
using GameServer.Types.Abilities.SharedAbilities;
using GameServer.Types.Map;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class FileManagerHead
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

    }
}
