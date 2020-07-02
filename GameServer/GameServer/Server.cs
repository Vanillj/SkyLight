using dotenv.net;
using dotenv.net.Utilities;
using GameServer.Scenes;
using Microsoft.Xna.Framework;
using Nez;
using Server.Managers;
using System;
using System.IO;

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
            Core.DebugRenderEnabled = true;
            Core.PauseOnFocusLost = false;
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine(dir);
            DotEnv.Config(true, "GameServer/.env");
            var envReader = new EnvReader();
            var value = envReader.GetStringValue("PASSWORD");

            networkManager = new ServerNetworkManager();
            SQLManager.SetUpSQL();
            MessageManager = new MessageManager();
            Scene mainScene = new MainScene() { MessageManager = MessageManager };
            Scene = mainScene;
        }
    }
}
