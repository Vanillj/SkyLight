using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
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
