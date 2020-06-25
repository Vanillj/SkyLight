using Client.Managers;
using GameClient.Managers;
using GameClient.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Server.Scenes;
using System;

namespace GameClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameClient : Core
    {
        public GameClient() : base()
        {
            Content.RootDirectory = "Content";
            policy = Scene.SceneResolutionPolicy.None;
            Scene.SetDefaultDesignResolution(1920, 1080, policy);
            Window.AllowUserResizing = true;
        }
        Scene.SceneResolutionPolicy policy;
        protected override void Initialize()
        {
            base.Initialize();
            ClientNetworkManager.SetupClient();
            PauseOnFocusLost = false;
            IsFixedTimeStep = false;

            BaseScene.messageManager = new MessageManager();
            MessageManager.inputManager = new InputManager();
            
            LoginScene mainScene = new LoginScene() {  };
            //SceneManager.CurrentScene = mainScene;
            Scene = mainScene;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            MessageManager.SendExitMessage();
            base.OnExiting(sender, args);
        }

    }
}
