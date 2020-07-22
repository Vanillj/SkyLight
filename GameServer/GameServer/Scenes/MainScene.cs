using GameServer.General;
using GameServer.Types;
using GameServer.Types.Components.SceneComponents;
using Microsoft.Xna.Framework;
using Nez.UI;
using Server.Managers;
using Server.Scenes;


namespace GameServer.Scenes
{
    class MainScene : BaseScene
    {
        public override Table Table { get; set; }

        public static Label ConnectedCount;

        TileManager tileManager;
        public override void Initialize()
        {
            base.Initialize();
            ConnectedCount = new Label("Current connections: 0").SetFontScale(5).SetFontColor(Color.Red);
            Table.Add(ConnectedCount);
            //setup server for networking
            AddSceneComponent<ServerNetworkSceneComponent>();
            //setup recieving component
            AddSceneComponent<MessageSceneComponent>();
            //Load tiles and environment
            tileManager = new TileManager();

            //probably dont need this one
            //Game update scene
            AddSceneComponent<GameSceneComponent>();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnStart()
        {
            MapContainer.LoadMaps(this);
            //tileManager.SetupTiles();
            base.OnStart();
        }
    }
}
