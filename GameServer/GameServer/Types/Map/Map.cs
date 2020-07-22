using Client.Managers;
using FarseerPhysics.Dynamics;
using GameServer.General;
using GameServer.Types.Components;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Farseer;
using Nez.Tiled;
using System.Collections.Generic;

namespace GameServer.Types.Map
{
    class Map : Component, IUpdatable
    {
        public MapType MapType { get; set; }
        public string MapName { get; set; }
        private TmxMap TileMap;
        private List<MapLayer> MapLayers = new List<MapLayer>();
        private long LayerID = 0;
        private Vector2 SpawnPoint;

        public Map(string MapName, MapType MapType)
        {
            this.MapType = MapType;
            this.MapName = MapName;
            TileMap = Core.Scene.Content.LoadTiledMap("Assets/" + MapName + ".tmx");
        }
        public void Update()
        {
            List<MapLayer> layersToDestroy = new List<MapLayer>();
            foreach (var layer in MapLayers)
            {
                layer.Update();
                if (layer.ToDestroy)
                    layersToDestroy.Add(layer);
            }

            if (layersToDestroy.Count > 0)
                foreach (var layer in layersToDestroy)
                    MapLayers.Remove(layer);

        }

        public void RemoveFromLayer(LoginManagerServer login)
        {
            foreach (var layer in MapLayers)
            {
                if (layer.LayerLogins.Contains(login))
                {
                    layer.RemoveLoginFromLayer(login);
                }
            }
        }

        public void AssignToLayer(Scene scene, LoginManagerServer login, PlayerComponent pc = null)
        {
            MapLayer assignedLayer = null;

            if (MapType == MapType.Multi)
            {
                //find first that isn't full
                assignedLayer = MapLayers.Find(l => 
                { 
                    if (l != null) 
                        return l.LayerLogins.Count < ConstantValues.MaxConnectionsToLayer; 
                    return false; 
                });
                //if none found then make new layer
                if (assignedLayer == null)
                    assignedLayer = CreateNewLayer(-1);
                login.GetCharacter().LastMultiLocation = MapName;
            }
            else if (MapType == MapType.Single)
            {
                assignedLayer = CreateNewLayer(login.GetUniqueID());
            }

            assignedLayer.AddLoginToLayer(login);

            Entity e = Core.Scene.FindEntity(login.GetCharacter()._name);
            if (e == null)
            {
                FSRigidBody fbody = new FSRigidBody().SetBodyType(BodyType.Dynamic).SetIgnoreGravity(true).SetLinearDamping(15f);

                e = scene.CreateEntity(login.GetCharacter()._name).SetPosition(login.GetCharacter().physicalPosition);
                    e.AddComponent(fbody)
                    .AddComponent(new FSCollisionCircle(25))
                    .AddComponent(new PlayerComponent(login) { CurrentLayer = assignedLayer })
                    .AddComponent(new Mover())
                    .AddComponent(new CircleCollider(25));
                fbody.Body.FixedRotation = true;
                login.SetEntity(e);

            }
            if (pc != null)
            {
                pc.CurrentLayer = assignedLayer;
            }
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            SetSpawnPoint();
            SetupCustomCollision();
        }

        private MapLayer CreateNewLayer(long ID)
        {
            MapLayer newLayer;
            if (ID != -1)
                newLayer = new MapLayer(MapName, LayerID, ID);
            else
                newLayer = new MapLayer(MapName, LayerID);

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
                                Entity.Scene.CreateEntity(obj.Name, new Vector2(Entity.Position.X + tile.Position.X * tile.Tileset.TileWidth + obj.Width / 2, Entity.Position.Y + tile.Position.Y * tile.Tileset.TileHeight + obj.Height / 2))
                                    .AddComponent(new FSCollisionEllipse(obj.Width / 2, obj.Height / 2))
                                    .AddComponent(new CircleCollider((obj.Width + obj.Height) / 4)); // have to get an average of sides, hence / 4
                            }
                            else if (type == TmxObjectType.Polygon)
                            {
                                Vector2[] points = obj.Points;

                                Entity.Scene.CreateEntity(obj.Name, new Vector2(Entity.Position.X + tile.Tileset.TileWidth * tile.Position.X + obj.X, Entity.Position.Y + tile.Tileset.TileHeight * tile.Position.Y + obj.Y))
                                    .AddComponent(new FSCollisionPolygon(points))
                                    .AddComponent(new PolygonCollider(points));
                            }
                            //basic is rectangle
                            else if (type == TmxObjectType.Basic)
                            {
                                Entity.Scene.CreateEntity(obj.Name, new Vector2(Entity.Position.X + tile.Position.X * tile.Tileset.TileWidth + obj.Width / 2, Entity.Position.Y + tile.Position.Y * tile.Tileset.TileHeight + obj.Height / 2))
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

        public TmxMap GetTmxMap()
        {
            return TileMap;
        }

    }
}
