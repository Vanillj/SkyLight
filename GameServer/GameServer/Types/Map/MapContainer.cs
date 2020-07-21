using Client.Managers;
using GameServer.Managers;
using GameServer.Types;
using GameServer.Types.Map;
using Microsoft.Xna.Framework;
using Nez;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.General
{
    class MapContainer
    {
        public static HashSet<Map> MapList = new HashSet<Map>();

        public static void LoadMaps()
        {
            MapList = FileManager.GetMapInformation("Data/MapData.json");
        }
        
        public static Map GetMapByName(string Name)
        {
            return MapList.FirstOrDefault(m => m.MapName.Equals(Name));
        }

        public static MapLayer AssignPlayer(LoginManagerServer login)
        {
            string map = login.GetCharacter().LastMultiLocation;
            Map foundMap;
            if (map != null)
            {
                foundMap = GetMapByName(map);

                login.GetCharacter().MoveToPos(foundMap.GetSpawnpoint());
            }
            else
            {
                foundMap = GetMapByName(ConstatValues.DefaultMap);
            }
            return foundMap.AssignToLayer(login);
        }

        public static CharacterPlayer FindCharacterByID(long uniqueID)
        {
            foreach (var map in MapList)
            {
                foreach (var layer in map.GetMapLayers())
                {
                    foreach (LoginManagerServer login in layer.LayerLogins)
                    {
                        if (login.GetUniqueID().Equals(uniqueID))
                        {
                            return login.GetCharacter();
                        }
                    }
                }
            }
            return null;
        }
        
    }
}
