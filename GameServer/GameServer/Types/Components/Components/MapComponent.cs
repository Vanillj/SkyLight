using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types
{
    class MapComponent : Component, IUpdatable
    {
        public float TotalTime;
        private TmxMap TileMap { get; }
        public string MapName { get; }
        public long? CreatorID { get; }

        public MapComponent(TmxMap tmxmap, string name, long? ID = null)
        {
            TileMap = tmxmap;
            MapName = name;
            CreatorID = ID;
        }

        public void Update()
        {
            TotalTime += Time.DeltaTime;
        }

        public TmxMap GetTiledMap()
        {
            return TileMap;
        }
    }
}
