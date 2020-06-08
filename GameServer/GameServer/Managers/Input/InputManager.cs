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
                        character._pos.Y -= 1f;
                        break;
                    case Keys.A:
                        character._pos.X -= 1f;
                        break;
                    case Keys.S:
                        character._pos.Y += 1f;
                        break;
                    case Keys.D:
                        character._pos.X += 1f;
                        break;

                    default:
                        break;
                }
            }
            
        }

    }
}
