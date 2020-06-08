using Client.Managers;
using GameClient.Managers;
using GameClient.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Server.Scenes;

namespace GameClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameClient : Core
    {
        public GameClient()
        {
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            ClientNetworkManager.SetupClient();

            BaseScene.messageManager = new MessageManager();
            MessageManager.inputManager = new InputManager();

            LoginScene mainScene = new LoginScene() {  };
            SceneManager.CurrentScene = mainScene;
            Scene = mainScene;
        }

    }
}
