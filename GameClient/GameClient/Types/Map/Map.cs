using GameServer.Types.Map;
using Nez;
using Nez.Tiled;

namespace GameClient.Types.Map
{
    class Map
    {
        public string MapName { get; set; }
        public TmxMap TmxMap { get; set; }
        public MapType MapType { get; set; }

        public Map(string MapName, MapType MapType)
        {
            this.MapType = MapType;
            this.MapName = MapName;
            TmxMap = Core.Scene.Content.LoadTiledMap("Assets/" + MapName + ".tmx");
        }
    }
}
