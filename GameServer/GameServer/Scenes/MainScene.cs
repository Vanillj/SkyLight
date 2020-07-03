using Client.Managers;
using GameServer.Types;
using GameServer.Types.Components.SceneComponents;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;
using Nez.UI;
using Server.Managers;
using Server.Scenes;
using Server.Types;
using System;
using System.Collections.Generic;
using CharacterPlayer = Server.Types.CharacterPlayer;


namespace GameServer.Scenes
{
    class MainScene : BaseScene
    {
        public override Table Table { get; set; }

        public static Label ConnectedCount;
        public override void Initialize()
        {
            base.Initialize();
            ConnectedCount = new Label("Current connections: 0").SetFontScale(5).SetFontColor(Color.Red);
            Table.Add(ConnectedCount);
            AddSceneComponent<MessageSceneComponent>();
            AddSceneComponent<GameSceneComponent>();

        }

        float timeSpan = 0;
        public override void Update()
        {
            /*timeSpan += Time.DeltaTime;
            if (timeSpan > 0.05)
            {
                timeSpan = 0;
            }*/
            
            base.Update();
        }
    }
}
