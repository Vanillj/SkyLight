using GameServer.Scenes;
using Nez;
using Server.Managers;

namespace GameServer
{
    public class Server : Core
    {

        ServerNetworkManager networkManager;
        MessageManager MessageManager;
        public Server()
        {

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
