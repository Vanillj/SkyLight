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
    public class GameClient : Core
    {
        Scene.SceneResolutionPolicy policy;
        public GameClient() : base()
        {
            Content.RootDirectory = "Content";
            policy = Scene.SceneResolutionPolicy.None;
            Scene.SetDefaultDesignResolution(1920, 1080, policy);
            Window.AllowUserResizing = true;
        }
        protected override void Initialize()
        {
            base.Initialize();
            GraphicsDevice.SetRenderTarget(new RenderTarget2D(GraphicsDevice, 64, 48));
            ClientNetworkManager.SetupClient();
            PauseOnFocusLost = false;
            IsFixedTimeStep = false;

            Scene = new LoginScene();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            MessageManager.SendExitMessage();
            base.OnExiting(sender, args);
        }

    }
}
