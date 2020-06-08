using Microsoft.Xna.Framework.Input;
using Server.Types;

namespace Server.Managers
{
    class InputManager
    {
        ServerNetworkManager networkManager;
        public InputManager(ServerNetworkManager _networkManager)
        {
            networkManager = _networkManager;
        }

        public static void CalculateMovement(Character character, Keys[] keys)
        {
            foreach (var key in keys)
            {
                switch (key)
                {
                    case Keys.W:
                        character._pos.X += 5;
                        break;

                    default:
                        break;
                }
            }
            
        }

    }
}
