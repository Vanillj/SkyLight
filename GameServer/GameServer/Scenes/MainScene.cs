using Microsoft.Xna.Framework;
using Nez.UI;
using Server.Managers;
using Server.Scenes;

namespace GameServer.Scenes
{
    class MainScene : BaseScene
    {
        public override Table Table { get; set; }
        public override MessageManager MessageManager { get; set; }

        public static Label ConnectedCount;
        public override void Initialize()
        {
            base.Initialize();
            ConnectedCount = new Label("Current connections: 0").SetFontScale(5).SetFontColor(Color.Red);
            
            Table.Add(ConnectedCount);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
