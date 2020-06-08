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
        KeyboardState oldState;

        public InputManager()
        {

        }

        public void CheckForInput()
        {
            if (ClientNetworkManager.client.ConnectionStatus != NetConnectionStatus.Connected || Core.Scene is LoginScene)
                return;

            var newState = Keyboard.GetState();

            if (newState.GetPressedKeys().Length > 0)
                SendMovementRequest(newState);


            //might be usable later for abilities and more.
            if (newState.IsKeyDown(Keys.S) && !oldState.IsKeyDown(Keys.S))
            {
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

            }
            else if (newState.IsKeyDown(Keys.D) && oldState.IsKeyDown(Keys.D))
            {
                // the player is holding the key down

            }
            else if (!newState.IsKeyDown(Keys.D) && oldState.IsKeyDown(Keys.D))
            {
                // the player was holding the key down, but has just let it go

            }

            /*if (changeInPosition != Vector2.Zero && ClientNetworkManager.client.ConnectionStatus == NetConnectionStatus.Connected && !(Core.Scene is LoginScene))
            {
                string changeS = Newtonsoft.Json.JsonConvert.SerializeObject(changeInPosition);
                MessageManager.AddToQueue(new MessageTemplate(changeS, MessageType.Movement));
                changeInPosition = Vector2.Zero;
            }*/
            oldState = newState;
        }

        private void SendMovementRequest(KeyboardState keyboardState)
        {
            string messageS = Newtonsoft.Json.JsonConvert.SerializeObject(keyboardState.GetPressedKeys());
            MessageManager.AddToQueue(new MessageTemplate(messageS, MessageType.Movement));
        }
    }
}
