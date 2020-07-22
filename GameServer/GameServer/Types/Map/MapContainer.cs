using Client.Managers;
using GameServer.Managers;
using GameServer.Scenes;
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

        public static void LoadMaps(MainScene scene)
        {
            MapList = FileManager.GetMapInformation("Data/MapData.json");
            int totalWidth = 0;
            foreach (var map in MapList)
            {
                Entity SceneEntity = scene.CreateEntity(map.MapName);
                SceneEntity.SetPosition(totalWidth, 0);
                TiledMapRenderer tmr = SceneEntity.AddComponent(new TiledMapRenderer(map.GetTmxMap(), "Collision", true));
                tmr.SetLayersToRender(new string[] { });

                totalWidth += map.GetTmxMap().Width + 100;

            }
        }
        
        public static Map GetMapByName(string Name)
        {
            return MapList.FirstOrDefault(m => m.MapName.Equals(Name));
        }

        public static void AssignLogin(Scene scene, LoginManagerServer login)
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
            foundMap.AssignToLayer(scene, login);
        }

        public static CharacterPlayer FindCharacterByID(long uniqueID)
        {
            LoginManagerServer login = GetLoginByID(uniqueID);
            if (login != null)
            {
                return login.GetCharacter();
            }
            return null;
        }

        public static LoginManagerServer GetLoginByID(long uniqueID)
        {
            foreach (var map in MapList)
            {
                foreach (var layer in map.GetMapLayers())
                {
                    foreach (LoginManagerServer login in layer.LayerLogins)
                    {
                        if (login.GetUniqueID().Equals(uniqueID))
                        {
                            return login;
                        }
                    }
                }
            }
            return null;
        }

        public static void RemoveLoginByID(long uniqueID)
        {
            foreach (var map in MapList)
            {
                foreach (var layer in map.GetMapLayers())
                {
                    layer.RemoveLoginFromLayerID(uniqueID);
                }
            }
        }

    }

}
