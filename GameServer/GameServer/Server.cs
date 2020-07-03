using GameServer.General;
using GameServer.Managers;
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
        MessageManager MessageManager;
        public Server() : base()
        {
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
            DebugRenderEnabled = true;
            PauseOnFocusLost = false;

            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine(dir);
            CredentialInfo credentialInfo = FileManager.GetFileFromString();
            

            new ServerNetworkManager(credentialInfo.ServerString, credentialInfo.Port);
            SQLManager.SetUpSQL(credentialInfo.ID, credentialInfo.PSW);
            MessageManager = new MessageManager();
            Scene mainScene = new MainScene() { MessageManager = MessageManager };
            Scene = mainScene;
        }
    }
}
