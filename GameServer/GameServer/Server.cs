using GameServer.Scenes;
using Microsoft.Xna.Framework;
using Nez;
using Server.Managers;
using System;

namespace GameServer
{
    public class Server : Core
    {

        ServerNetworkManager networkManager;
        MessageManager MessageManager;
        public Server() : base()
        {
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
            Core.PauseOnFocusLost = false;
            networkManager = new ServerNetworkManager();
            MessageManager = new MessageManager();
            Scene mainScene = new MainScene() { MessageManager = MessageManager };
            Scene = mainScene;
        }
    }
}
