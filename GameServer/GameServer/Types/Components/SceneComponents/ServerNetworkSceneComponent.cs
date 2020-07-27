using GameServer.General;
using Lidgren.Network;
using Nez;

namespace Server.Managers
{
    class ServerNetworkSceneComponent : SceneComponent
    {
        private static NetServer server;

        public static NetServer GetNetServer()
        {
            return server;
        }
        public override void OnEnabled()
        {
            NetPeerConfiguration Configuration = new NetPeerConfiguration(ConstantValues.ServerString) { Port = (int)ConstantValues.Port };
            Configuration.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            Configuration.EnableMessageType(NetIncomingMessageType.StatusChanged);
            server = new NetServer(Configuration);
            server.Start();
            
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            server.Shutdown("Shutting down server!");

            base.OnDisabled();
        }
    }
}
