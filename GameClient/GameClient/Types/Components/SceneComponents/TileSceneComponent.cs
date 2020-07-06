using Nez;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.Components.SceneComponents
{
    class TileSceneComponent : SceneComponent
    {
        public override void OnEnabled()
        {
            SetupTiles();
            base.OnEnabled();
        }
        public void SetupTiles()
        {
            if (Core.Scene == null)
                return;

            TmxMap map = Core.Scene.Content.LoadTiledMap("Assets/map.tmx");
            Entity entity = Core.Scene.CreateEntity("tiled-map");

            TiledMapRenderer tmr = entity.AddComponent(new TiledMapRenderer(map));
            tmr.SetRenderLayer(1);

            //tmr.SetLayersToRender(new string[]{ "Tile", "Collision" });
        }
    }
}
