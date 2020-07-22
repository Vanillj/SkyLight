using Client.Managers;
using GameServer.Managers;
using GameServer.Managers.Networking;
using GameServer.Scenes;
using GameServer.Types;
using GameServer.Types.Components;
using GameServer.Types.Map;
using Microsoft.Xna.Framework;
using Nez;
using Server.Managers;
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
        private static MainScene Scene;

        public static void LoadMaps(MainScene scene)
        {
            MapList = FileManager.GetMapInformation("Data/MapData.json");
            int totalWidth = 0;
            Scene = scene;

            foreach (var map in MapList)
            {
                Entity SceneEntity = scene.CreateEntity(map.MapName);
                SceneEntity.SetPosition(totalWidth, 0);
                SceneEntity.AddComponent(map);
                TiledMapRenderer tmr = SceneEntity.AddComponent(new TiledMapRenderer(map.GetTmxMap(), "Collision", true));
                tmr.SetLayersToRender(new string[] { });

                totalWidth += map.GetTmxMap().WorldWidth + 100;

            }
        }
        
        public static Map GetMapByName(string Name)
        {
            return MapList.FirstOrDefault(m => m.MapName.Equals(Name));
        }

        //for logging in
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
                foundMap = GetMapByName(ConstantValues.DefaultMap);
            }
            foundMap.AssignToLayer(scene, login);
        }

        internal static void MoveLoginToMap(LoginManagerServer login, Map newMap)
        {
            PlayerComponent pc = login.GetEntity().GetComponent<PlayerComponent>();
            pc.CurrentLayer.RemoveLoginFromLayer(login);
            newMap.AssignToLayer(Scene, login, pc);
            login.GetEntity().SetPosition(newMap.GetSpawnpoint() + newMap.Entity.Position);
            //MessageTemplate temp = new MessageTemplate(, MessageType.MapChange);

            MessageManager.SendStringToUniqueID(newMap.MapName, login.GetUniqueID(), MessageType.MapChange);
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
