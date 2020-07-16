using Client.Managers;
using GameClient.Managers;
using GameClient.Managers.UI;
using GameClient.Scenes;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;

namespace GameClient.Types.Components.SceneComponents
{
    class InputComponent : SceneComponent
    {
        public Camera Camera { get; set; }

        private float currentMouseWheelValue, previousMouseWheelValue;
        private Entity Entity;
        Skin skin = Skin.CreateDefaultSkin();
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

            //Update Keyboard

            if (Input.CurrentKeyboardState.GetPressedKeys().Length > 0)
                SendKeyboardRequest(new KeyboardState(KeyboardChange()));

            //Update mouse
            //if (Input.LeftMouseButtonPressed || Input.RightMouseButtonPressed ||Input.MousePositionDelta != Point.Zero)
            //MouseChange();

            previousMouseWheelValue = currentMouseWheelValue;
            currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;
            ScrollChange();
        }

        //TODO: Change to customizable keybindings later
        private Keys[] KeyboardChange()
        {
            KeyboardState newState = Input.CurrentKeyboardState;
            KeyboardState OldKeyboardState = Input.PreviousKeyboardState;

            List<Keys> keys = new List<Keys>();
            float speed = 100;
            var dir = Vector2.Zero;

            //might be usable later for abilities and more.
            if (newState.IsKeyDown(Keys.T) && !OldKeyboardState.IsKeyDown(Keys.T))
            {
                keys.Add(Keys.T);
            }

            //Generated inventory
            if (newState.IsKeyDown(Keys.I) && OldKeyboardState.IsKeyUp(Keys.I))
            {
                if (UIManager.FindElementByString("Inventory", Scene))
                {
                    Window table = UIManager.GenerateInventoryWindow(skin, (Scene as MainScene).UICanvas.Stage);

                    table.SetPosition(Core.GraphicsDevice.Viewport.Width - table.GetWidth(), Core.GraphicsDevice.Viewport.Height - table.GetHeight());
                    table.DebugAll();

                    (Scene as MainScene).UICanvas.Stage.AddElement(table);
                }
            }

            if (newState.IsKeyDown(Keys.C) && OldKeyboardState.IsKeyUp(Keys.C))
            {
                if (UIManager.FindElementByString("Character Information", Scene))
                {
                    Window table = UIManager.GenerateCharacterWindow(skin, (Scene as MainScene).UICanvas.Stage);

                    table.SetPosition(Core.GraphicsDevice.Viewport.Width - table.GetWidth(), 50);
                    table.DebugAll();
                    List<Element> l = (Scene as MainScene).UICanvas.Stage.GetElements();
                    (Scene as MainScene).UICanvas.Stage.AddElement(table);
                }
            }

            if (newState.IsKeyDown(Keys.S) && !OldKeyboardState.IsKeyDown(Keys.S))
            {

            }
            else if (newState.IsKeyDown(Keys.S) && OldKeyboardState.IsKeyDown(Keys.S))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.Y += 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.Y += 1f;
                keys.Add(Keys.S);
                dir.Y = 1f;
            }
            else if (!newState.IsKeyDown(Keys.S) && OldKeyboardState.IsKeyDown(Keys.S))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.W) && !OldKeyboardState.IsKeyDown(Keys.W))
            {
            }
            else if (newState.IsKeyDown(Keys.W) && OldKeyboardState.IsKeyDown(Keys.W))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.Y -= 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.Y -= 1f;
                dir.Y = -1f;
                keys.Add(Keys.W);
            }
            else if (!newState.IsKeyDown(Keys.W) && OldKeyboardState.IsKeyDown(Keys.W))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.A) && !OldKeyboardState.IsKeyDown(Keys.A))
            {

            }
            else if (newState.IsKeyDown(Keys.A) && OldKeyboardState.IsKeyDown(Keys.A))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.X -= 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.X -= 1f;
                keys.Add(Keys.A);
                dir.X = -1f;
            }
            else if (!newState.IsKeyDown(Keys.A) && OldKeyboardState.IsKeyDown(Keys.A))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.D) && !OldKeyboardState.IsKeyDown(Keys.D))
            {

            }
            else if (newState.IsKeyDown(Keys.D) && OldKeyboardState.IsKeyDown(Keys.D))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.X += 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.X += 1f;
                keys.Add(Keys.D);
                dir.X = 1f;
            }
            else if (!newState.IsKeyDown(Keys.D) && OldKeyboardState.IsKeyDown(Keys.D))
            {
                // the player was holding the key down, but has just let it go

            }
            var movement = dir * speed * Time.DeltaTime * 4;
            if (movement != Vector2.Zero)
                LoginManagerClient.GetCharacter()._pos += movement;
            return keys.ToArray();
        }

        private void MouseChange()
        {
            Vector2 mousePosition = Input.MousePosition;
            //Console.WriteLine(mousePosition);
            if (Scene is MainScene)
            {
                MainScene scene = Scene as MainScene;

                if (scene.UICanvas.Stage != null)
                {
                    foreach (var item in scene.UICanvas.Stage.GetElements())
                    {

                        if (item is Table)
                        {
                            foreach (var child in (item as Table).GetChildren())
                            {
                                if (child is Table)
                                {
                                    Table tab = child as Table;

                                    foreach (var chilli in tab.GetChildren())
                                    {

                                    }

                                }
                            }
                            int countX = 0;
                            int countY = 0;
                            Table t = item as Table;
                            Element e = t.Hit(Input.MousePosition);

                            if (e is Label)
                            {
                                (e as Label).SetFontColor(Color.Red);
                            }
                            Rectangle rt = new Rectangle((int)t.GetX(), (int)t.GetY(), (int)t.GetWidth(), (int)t.GetHeight());
                            if (rt.Contains(mousePosition))
                            {
                                string s = "";
                                foreach (var child in t.GetChildren())
                                {
                                    if (child is Label)
                                    {
                                        (child as Label).SetFontColor(Color.Red);
                                    }
                                }
                            }
                            if (e != null)
                            {
                                string s = "";
                            }
                            foreach (var child in t.GetChildren())
                            {
                                if (countX % 4 == 0)
                                {
                                    countX = 0;
                                    countY++;
                                }
                                if (child is Label)
                                {
                                    Label l = child as Label;


                                    float f = l.GetX();
                                    float relativeX = t.GetX() + l.GetX() + countX * l.GetWidth();
                                    float relativeY = t.GetY() + l.GetY() + countY * l.GetWidth();
                                    Rectangle r = new Rectangle((int)relativeX, (int)relativeY, (int)l.GetWidth(), (int)l.GetHeight());

                                    if (r.Contains(mousePosition))
                                    {
                                        Console.WriteLine("Inside");
                                        l.SetFontColor(Color.Red);
                                    }
                                    else
                                    {
                                        l.SetFontColor(Color.White);
                                    }
                                }
                                countX++;
                            }
                        }
                    }
                }
            }
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
        private void SendKeyboardRequest(KeyboardState keyboardState)
        {
            string messageS = Newtonsoft.Json.JsonConvert.SerializeObject(keyboardState.GetPressedKeys());
            MessageManager.AddToQueue(new MessageTemplate(messageS, MessageType.Movement));
        }

    }
}
