using Client.Managers;
using GameServer.General;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Farseer;
using Nez.Tiled;
using Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Map
{
    class Map : Component, IUpdatable
    {
        public MapType MapType { get; set; }
        public string MapName { get; set; }
        private TmxMap TileMap;
        private List<MapLayer> MapLayers = new List<MapLayer>();
        private int LayerID = 0;
        private Vector2 SpawnPoint;

        public Map(string MapName, MapType MapType)
        {
            this.MapType = MapType;
            this.MapName = MapName;
            TileMap = Core.Scene.Content.LoadTiledMap("Assets/" + MapName + ".tmx");
        }
        public void Update()
        {
            foreach (var layer in MapLayers)
            {
                layer.Update();
            }
        }

        public MapLayer AssignToLayer(LoginManagerServer login)
        {
            MapLayer assignedLayer = null;

            if (MapType == MapType.Multi)
            {
                assignedLayer = MapLayers.Find(l => 
                { 
                    if (l != null) 
                        return l.LayerLogins.Count < ConstatValues.MaxConnectionsToLayer; 
                    return false; 
                });

                if (assignedLayer == null)
                    assignedLayer = CreateNewLayer();
                login.GetCharacter().LastMultiLocation = MapName;
            }
            else if (MapType == MapType.Single)
            {
                assignedLayer = CreateNewLayer();
            }

            assignedLayer.AddLoginToLayer(login);
            return assignedLayer;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            SetSpawnPoint();
            SetupCustomCollision();
        }

        private MapLayer CreateNewLayer()
        {
            MapLayer newLayer = new MapLayer(LayerID);
            MapLayers.Add(newLayer);
            LayerID++;
            return newLayer;
        }

        private void SetupCustomCollision()
        {

            TmxLayer CustomCollisionLayer = (TmxLayer)TileMap.GetLayer("CustomCollision");

            foreach (TmxLayerTile tile in CustomCollisionLayer.Tiles)
            {
                if (tile != null && tile.TilesetTile != null)
                {
                    TmxList<TmxObjectGroup> objgl = tile.TilesetTile.ObjectGroups;

                    if (objgl != null && objgl.Count > 0)
                    {
                        TmxObjectGroup objg = objgl[0];
                        if (objg.Objects != null && objg.Objects.Count > 0)
                        {
                            TmxObject obj = objg.Objects[0];
                            TmxObjectType type = obj.ObjectType;

                            if (type == TmxObjectType.Ellipse)
                            {
                                //Draw Ellipse as collision
                                Core.Scene.CreateEntity(obj.Name, new Vector2(Entity.Position.X + tile.Position.X * tile.Tileset.TileWidth + obj.Width / 2, Entity.Position.Y + tile.Position.Y * tile.Tileset.TileHeight + obj.Height / 2))
                                    .AddComponent(new FSCollisionEllipse(obj.Width / 2, obj.Height / 2))
                                    .AddComponent(new CircleCollider((obj.Width + obj.Height) / 4)); // have to get an average of sides, hence / 4
                            }
                            else if (type == TmxObjectType.Polygon)
                            {
                                Vector2[] points = obj.Points;

                                Core.Scene.CreateEntity(obj.Name, new Vector2(Entity.Position.X + tile.Tileset.TileWidth * tile.Position.X + obj.X, Entity.Position.Y + tile.Tileset.TileHeight * tile.Position.Y + obj.Y))
                                    .AddComponent(new FSCollisionPolygon(points))
                                    .AddComponent(new PolygonCollider(points));
                            }
                            //basic is rectangle
                            else if (type == TmxObjectType.Basic)
                            {
                                Core.Scene.CreateEntity(obj.Name, new Vector2(Entity.Position.X + tile.Position.X * tile.Tileset.TileWidth + obj.Width / 2, Entity.Position.Y + tile.Position.Y * tile.Tileset.TileHeight + obj.Height / 2))
                                    .AddComponent(new FSCollisionBox(obj.Width, obj.Height))
                                    .AddComponent(new BoxCollider(obj.Width, obj.Height));
                                Entity.AddComponent(new FSCollisionBox(obj.Width, obj.Height)).AddComponent(new BoxCollider(obj.Width, obj.Height));

                            }
                        }
                    }
                }
            }
        }

        private void SetSpawnPoint()
        {
            TmxObjectGroup objectLayer = TileMap.GetLayer<TmxObjectGroup>("Objects");
            foreach (TmxObject obj in objectLayer.Objects)
            {
                if (obj.ObjectType == TmxObjectType.Point && obj.Name.Equals("Spawn"))
                {
                    SpawnPoint = new Vector2(obj.X, obj.Y);
                }
            }
        }

        public Vector2 GetSpawnpoint()
        {
            return SpawnPoint;
        }

        public List<MapLayer> GetMapLayers()
        {
            return MapLayers;
        }

    }
}
