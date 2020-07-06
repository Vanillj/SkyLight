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
            //Setup game settings
            Window.AllowUserResizing = true;
            DebugRenderEnabled = true;
            PauseOnFocusLost = false;

            //setup constant values
            //Getting credentials from file
            CredentialInfo credentialInfo = FileManager.GetFileFromString("Credentials.json");
            StaticConstantValues.ConnectionID = credentialInfo.ID;
            StaticConstantValues.ConnectionCredential = credentialInfo.ConnectionCredential;
            StaticConstantValues.ServerString = credentialInfo.ServerString;
            StaticConstantValues.Port = credentialInfo.Port;
        }

        protected override void Initialize()
        {
            base.Initialize();

            SQLManager.SetUpSQL();
            Scene = new MainScene();
            //Scene.Camera.ZoomOut(0.1f);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            SQLManager.CloseSQL();
            base.OnExiting(sender, args);
        }
    }
}
