using Client.Managers;
using GameClient.Managers;
using Nez;
using Nez.BitmapFonts;
using Nez.Tiled;
using Server.Types;
using System.Collections.Generic;

namespace GameClient.Types.Components.SceneComponents
{
    class TileSceneComponent : SceneComponent
    {
        public HashSet<Map.Map> MapList = new HashSet<Map.Map>();
        Entity entity;
        public override void OnEnabled()
        {
            SetupTiles();
            base.OnEnabled();
        }
        public void SetupTiles()
        {
            if (Core.Scene == null)
                return;
            MapList = FileManager.GetMapInformation("Data/MapData.json");
            Map.Map map = null;
            CharacterPlayer c = LoginManagerClient.GetCharacter();
            foreach (var maps in MapList)
            {
                if (maps.MapName == c.LastMultiLocation)
                {
                    map = maps;
                }
            }
            if (map != null)
            {
                entity = Scene.CreateEntity(map.MapName);
                TiledMapRenderer tmr = entity.AddComponent(new TiledMapRenderer(map.TmxMap));
                tmr.SetRenderLayer(1);
                tmr.SetLayersToRender(new string[] { "Tile", "Collision", "Decoration", "CustomCollision" });
            }
        }

        public void ChangeMap(Map.Map newMap)
        {
            entity.RemoveComponent<TiledMapRenderer>();
            TiledMapRenderer tmr = entity.AddComponent(new TiledMapRenderer(newMap.TmxMap));
            tmr.SetRenderLayer(1);
            tmr.SetLayersToRender(new string[] { "Tile", "Collision", "Decoration", "CustomCollision" });

        }
    }
}
