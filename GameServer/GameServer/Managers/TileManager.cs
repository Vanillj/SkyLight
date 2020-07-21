using Microsoft.Xna.Framework;
using Nez;
using Nez.Farseer;
using Nez.Tiled;

namespace GameServer.Types.Components.SceneComponents
{
    class TileManager
    {
        bool enabled = false;

        public void SetupTiles()
        {
            if (Core.Scene == null || enabled)
                return;
            //enabled = true;
            TmxMap map =  Core.Scene.Content.LoadTiledMap("Assets/map.tmx");
            Entity entity = Core.Scene.CreateEntity("tiled-map");

            TiledMapRenderer tmr = entity.AddComponent(new TiledMapRenderer(map, "Collision", true));
            TmxLayer CustomCollisionLayer = (TmxLayer) map.GetLayer("CustomCollision");

            foreach (TmxLayerTile tile in CustomCollisionLayer.Tiles)
            {
                if (tile != null && tile.TilesetTile != null)
                {
                    TmxList<TmxObjectGroup> objgl = tile.TilesetTile.ObjectGroups;
                    
                    if (objgl != null && objgl.Count > 0) {
                        TmxObjectGroup objg = objgl[0];
                        if (objg.Objects != null && objg.Objects.Count > 0)
                        {
                            TmxObject obj = objg.Objects[0];
                            TmxObjectType type = obj.ObjectType;

                            if (type == TmxObjectType.Ellipse)
                            {
                                //Draw Ellipse as collision
                                Core.Scene.CreateEntity(obj.Name, new Vector2(tile.Position.X * tile.Tileset.TileWidth + obj.Width / 2, tile.Position.Y * tile.Tileset.TileHeight + obj.Height / 2))
                                    .AddComponent(new FSCollisionEllipse(obj.Width / 2, obj.Height / 2))
                                    .AddComponent(new CircleCollider((obj.Width + obj.Height) / 4)); // have to get an average of sides, hence / 4
                            }
                            else if (type == TmxObjectType.Polygon)
                            {
                                Vector2[] points = obj.Points;

                                Core.Scene.CreateEntity(obj.Name,new Vector2(tile.Tileset.TileWidth * tile.Position.X + obj.X, tile.Tileset.TileHeight * tile.Position.Y + obj.Y))
                                    .AddComponent(new FSCollisionPolygon(points))
                                    .AddComponent(new PolygonCollider(points));
                            }
                            //basic is rectangle
                            else if (type == TmxObjectType.Basic)
                            {
                                Core.Scene.CreateEntity(obj.Name, new Vector2(tile.Position.X * tile.Tileset.TileWidth + obj.Width / 2, tile.Position.Y * tile.Tileset.TileHeight + obj.Height / 2))
                                    .AddComponent(new FSCollisionBox(obj.Width, obj.Height))
                                    .AddComponent(new BoxCollider(obj.Width, obj.Height));
                            }
                        }
                    }
                }
            }
            tmr.SetLayersToRender(new string[] { });
        }

        private void Setup()
        {

        }
    }
}
