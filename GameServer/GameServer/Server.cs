using GameServer.General;
using GameServer.Managers;
using GameServer.Scenes;
using GameServer.Types.Abilities;
using Microsoft.Xna.Framework;
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

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 144f);

            ItemContainer.GenerateItems();
            AbilityContainer.LoadAbilities();

            //setup constant values
            //Getting credentials from file
            CredentialInfo credentialInfo = FileManager.GetCredentialInformation("Credentials.json");
            ConstantValues.ConnectionID = credentialInfo.ID;
            ConstantValues.ConnectionCredential = credentialInfo.ConnectionCredential;
            ConstantValues.ServerString = credentialInfo.ServerString;
            ConstantValues.Port = credentialInfo.Port;
        }

        protected override void Initialize()
        {
            base.Initialize();

            SQLManager.SetUpSQL();
            Scene = new MainScene();
            //Scene.Camera.ZoomOut(10f);
            Scene.Camera.SetPosition(new Vector2(17*64, 12*64));
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            SQLManager.CloseSQL();
            base.OnExiting(sender, args);
        }
    }
}
