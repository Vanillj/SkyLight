using Client.Managers;
using GameClient.Managers;
using GameClient.Scenes;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;
using Server.Managers;
using Server.Types;
using System.Collections.Generic;

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
                SendMovementRequest(new KeyboardState(KeyboardChange(newState)));

            oldState = newState;

            previousMouseWheelValue = currentMouseWheelValue;
            currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;
            ScrollChange();
        }

        public Keys[] KeyboardChange(KeyboardState newState)
        {
            List<Keys> keys = new List<Keys>();
            float speed = 100;
            var dir = Vector2.Zero;
            //might be usable later for abilities and more.
            if (newState.IsKeyDown(Keys.T) && !oldState.IsKeyDown(Keys.T))
            {
                keys.Add(Keys.T);
            }

            //Opens Inventory
            if (newState.IsKeyDown(Keys.I) && oldState.IsKeyUp(Keys.I))
            {

                bool found = false;
                Table t = null;
                foreach (var item in ((MainScene)Scene).Table.GetChildren())
                {
                    if (item is Table && !found)
                    {
                        t = item as Table;
                        foreach (var cell in t.GetChildren())
                        {
                            if (cell is Label)
                            {
                                Label l = cell as Label;
                                if (l.GetText().Equals("Inventory"))
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (!found)
                {
                    Table table = new Table();
                    table.SetBounds(Core.GraphicsDevice.Viewport.Width - 100, Core.GraphicsDevice.Viewport.Height - 100, 100, 100);
                    table.Add(new Label("Inventory").SetFontColor(Color.WhiteSmoke).SetFontScale(1.5f));
                    table.Row().SetPadTop(10);
                    foreach (var i in LoginManagerClient.GetCharacter().Inventory)
                    {
                        if (i != null)
                        {
                            table.Add(new Label(i.Name).SetFontColor(Color.WhiteSmoke));
                            table.Row().SetPadTop(10);
                        }
                    }
                    ((MainScene)Scene).Table.Add(table);
                }
                else
                {
                    ((MainScene)Scene).Table.RemoveElement(t);
                }
                
            }

            if (newState.IsKeyDown(Keys.S) && !oldState.IsKeyDown(Keys.S))
            {

            }
            else if (newState.IsKeyDown(Keys.S) && oldState.IsKeyDown(Keys.S))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.Y += 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.Y += 1f;
                keys.Add(Keys.S);
                dir.Y = 1f;
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
                //LoginManagerClient.GetCharacter()._pos.Y -= 1f;
                dir.Y = -1f;
                keys.Add(Keys.W);
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
                //LoginManagerClient.GetCharacter()._pos.X -= 1f;
                keys.Add(Keys.A);
                dir.X = -1f;
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
                //LoginManagerClient.GetCharacter()._pos.X += 1f;
                keys.Add(Keys.D);
                dir.X = 1f;
            }
            else if (!newState.IsKeyDown(Keys.D) && oldState.IsKeyDown(Keys.D))
            {
                // the player was holding the key down, but has just let it go

            }
            var movement = dir * speed * Time.DeltaTime * 4;
            if (movement != Vector2.Zero)
                LoginManagerClient.GetCharacter()._pos += movement;
            return keys.ToArray();
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
