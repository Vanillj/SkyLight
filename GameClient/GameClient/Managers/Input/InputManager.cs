using Client.Types;
using GameClient.Managers;
using GameClient.Scenes;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Managers
{
    class InputManager
    {
        static Vector2 changeInPosition = new Vector2();
        KeyboardState oldState;

        public InputManager()
        {

        }

        public void CheckForInput()
        {
            var newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.S) && !oldState.IsKeyDown(Keys.S))
            {
                changeInPosition.Y -= 5;
                
            }
            else if (newState.IsKeyDown(Keys.S) && oldState.IsKeyDown(Keys.S))
            {
                // the player is holding the key down

            }
            else if (!newState.IsKeyDown(Keys.S) && oldState.IsKeyDown(Keys.S))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.W) && !oldState.IsKeyDown(Keys.W))
            {
                changeInPosition.Y += 5;
                
            }
            else if (newState.IsKeyDown(Keys.W) && oldState.IsKeyDown(Keys.W))
            {
                // the player is holding the key down

            }
            else if (!newState.IsKeyDown(Keys.W) && oldState.IsKeyDown(Keys.W))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.A) && !oldState.IsKeyDown(Keys.A))
            {
                changeInPosition.X -= 5;

            }
            else if (newState.IsKeyDown(Keys.A) && oldState.IsKeyDown(Keys.A))
            {
                // the player is holding the key down

            }
            else if (!newState.IsKeyDown(Keys.A) && oldState.IsKeyDown(Keys.A))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.D) && !oldState.IsKeyDown(Keys.D))
            {
                changeInPosition.X += 5;

            }
            else if (newState.IsKeyDown(Keys.D) && oldState.IsKeyDown(Keys.D))
            {
                // the player is holding the key down

            }
            else if (!newState.IsKeyDown(Keys.D) && oldState.IsKeyDown(Keys.D))
            {
                // the player was holding the key down, but has just let it go

            }

            if (changeInPosition != Vector2.Zero && ClientNetworkManager.client.ConnectionStatus == NetConnectionStatus.Connected && !(Core.Scene is LoginScene))
            {
                MessageManager.AddToQueue(new MessageTemplate(MessageTemplate.ObjectToJson(changeInPosition), MessageType.Movement));
                changeInPosition = Vector2.Zero;
            }
            oldState = newState;
        }
    }
}
