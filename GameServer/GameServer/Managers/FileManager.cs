using GameServer.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class FileManager
    {
        internal static CredentialInfo GetFileFromString(string FileName)
        {
            using (var file = File.OpenText(FileName))
            {
                string s = file.ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<CredentialInfo>(s);
            }
        }
    }
}
