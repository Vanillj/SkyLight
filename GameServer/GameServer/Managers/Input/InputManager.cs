using Microsoft.Xna.Framework.Input;
using Server.Types;
using System;
using System.Diagnostics;

namespace Server.Managers
{
    class InputManager
    {
        public InputManager()
        {
        }

        public static void CalculateMovement(CharacterPlayer character, Keys[] keys)
        {
            if (character == null)
                return;

            try
            {
                foreach (var key in keys)
                {
                    //add proper movement with deltaTime later
                    switch (key)
                    {
                        case Keys.W:
                            character._pos.Y -= 3f;
                            break;
                        case Keys.A:
                            character._pos.X -= 3f;
                            break;
                        case Keys.S:
                            character._pos.Y += 3f;
                            break;
                        case Keys.D:
                            character._pos.X += 3f;
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            
            
        }

    }

}
