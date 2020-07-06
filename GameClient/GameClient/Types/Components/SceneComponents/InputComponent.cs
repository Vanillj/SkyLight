using Client.Managers;
using GameClient.Managers;
using GameClient.Scenes;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.BitmapFonts;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.Components.SceneComponents
{
    class InputComponent : SceneComponent
    {
        public Camera Camera { get; set; }

        private KeyboardState oldState;
        private float currentMouseWheelValue, previousMouseWheelValue;
        private Entity Entity;

        public InputComponent(Entity entity, Camera camera)
        {
            Entity = entity;
            Camera = camera;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
        }
        public override void Update()
        {
            CheckForInput();
            base.Update();
        }
        public void CheckForInput()
        {
            if (ClientNetworkManager.client.ConnectionStatus != NetConnectionStatus.Connected || Core.Scene is LoginScene)
                return;

            var newState = Keyboard.GetState();

            if (newState.GetPressedKeys().Length > 0)
                SendMovementRequest(newState);

            KeyboardChange(newState);

            previousMouseWheelValue = currentMouseWheelValue;
            currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;
            ScrollChange();
        }
        public void KeyboardChange(KeyboardState newState)
        {
            //might be usable later for abilities and more.
            if (newState.IsKeyDown(Keys.S) && !oldState.IsKeyDown(Keys.S))
            {
            }
            else if (newState.IsKeyDown(Keys.S) && oldState.IsKeyDown(Keys.S))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.Y += 3*60f * Time.DeltaTime;
                LoginManagerClient.GetCharacter()._pos.Y += 4.5f;
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
                //LoginManagerClient.GetCharacter()._pos.Y -= 3*60f * Time.DeltaTime;
                LoginManagerClient.GetCharacter()._pos.Y -= 4.5f;
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
                //LoginManagerClient.GetCharacter()._pos.X -= 3*60f * Time.DeltaTime;
                LoginManagerClient.GetCharacter()._pos.X -= 4.5f;
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
                //LoginManagerClient.GetCharacter()._pos.X += 3*60f * Time.DeltaTime;
                LoginManagerClient.GetCharacter()._pos.X += 4.5f;
            }
            else if (!newState.IsKeyDown(Keys.D) && oldState.IsKeyDown(Keys.D))
            {
                // the player was holding the key down, but has just let it go

            }
            oldState = newState;
        }

        public void ScrollChange()
        {
            if (currentMouseWheelValue > previousMouseWheelValue)
            {
                Camera.ZoomIn(.05f);
            }

            if (currentMouseWheelValue < previousMouseWheelValue)
            {
                Camera.ZoomOut(.05f);
            }

            if (Entity != null)
            {
                FollowCamera fc = Entity.GetComponent<FollowCamera>();
                if (fc != null && fc.Camera != null)
                {
                    //So the camera is centered after zooming in our out
                    fc.Follow(Entity, FollowCamera.CameraStyle.LockOn);

                }
            }
            
        }
        private void SendMovementRequest(KeyboardState keyboardState)
        {
            string messageS = Newtonsoft.Json.JsonConvert.SerializeObject(keyboardState.GetPressedKeys());
            MessageManager.AddToQueue(new MessageTemplate(messageS, MessageType.Movement));
        }

        
    }
}
