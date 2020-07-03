using GameServer.General;
using GameServer.Managers;
using GameServer.Scenes;
using Nez;
using Server.Managers;
using System;

namespace GameServer
{
    public class Server : Core
    {
        public Server() : base()
        {
            Window.AllowUserResizing = true;
            DebugRenderEnabled = true;
            PauseOnFocusLost = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            //Getting credentials from file
            CredentialInfo credentialInfo = FileManager.GetFileFromString("Credentials.json");
            
            new ServerNetworkManager(credentialInfo.ServerString, credentialInfo.Port);
            SQLManager.SetUpSQL(credentialInfo.ID, credentialInfo.PSW);

            Scene = new MainScene();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            SQLManager.CloseSQL();
            base.OnExiting(sender, args);
        }
    }
}
