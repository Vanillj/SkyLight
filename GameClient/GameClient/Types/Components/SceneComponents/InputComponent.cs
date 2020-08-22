using Client.Managers;
using GameClient.General;
using GameClient.Managers;
using GameClient.Managers.UI;
using GameClient.Managers.UI.Elements;
using GameClient.Scenes;
using GameClient.Types.Components.Components;
using GameClient.UI.Component;
using GameServer.General;
using GameServer.Types.Abilities.SharedAbilities;
using GameServer.Types.Networking;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.UI;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Direction = Nez.Direction;

namespace GameClient.Types.Components.SceneComponents
{
    class InputComponent : SceneComponent
    {
        private float timer = 0;
        private float AbiliyCoolDown = 0;
        public Camera Camera { get; set; }

        private float currentMouseWheelValue, previousMouseWheelValue;
        private Entity Entity;
        public static Skin skin = Skin.CreateDefaultSkin();

        public static Direction direction = Direction.Right;
        public static bool IsMoving = false;

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
            timer += Time.DeltaTime;
            AbiliyCoolDown += Time.DeltaTime;

            if (timer >= ConstantValues.UpdateFrequency)
            {
                FixedUpdate();
                timer -= ConstantValues.UpdateFrequency;
            }
            CheckForInput();

            base.Update();
        }

        private void FixedUpdate()
        {
            CheckFixed();
        }

        #region Fixed Update
        private void CheckFixed()
        {
            if (ClientNetworkManager.client.ConnectionStatus != NetConnectionStatus.Connected || Core.Scene is LoginScene)
                return;

            if (Input.CurrentKeyboardState.GetPressedKeys().Length > 0)
                SendKeyboardRequest(new KeyboardState(KeyboardFixed()));


            if (Input.LeftMouseButtonPressed || Input.RightMouseButtonPressed || Input.MousePositionDelta != Point.Zero)
                FixedMouseChange();

        }

        private void FixedMouseChange()
        {


        }

        private Keys[] KeyboardFixed()
        {
            KeyboardState newState = Input.CurrentKeyboardState;
            KeyboardState OldKeyboardState = Input.PreviousKeyboardState;

            List<Keys> keys = new List<Keys>();
            float speed = 500;
            var dir = Vector2.Zero;

            //might be usable later for abilities and more.
            if (newState.IsKeyDown(Keys.T) && !OldKeyboardState.IsKeyDown(Keys.T))
            {
                keys.Add(Keys.T);
            }

            //Generated inventory
            if (newState.IsKeyDown(Keys.I) && OldKeyboardState.IsKeyUp(Keys.I))
            {
                keys.Add(Keys.I);
            }

            if (newState.IsKeyDown(Keys.C) && OldKeyboardState.IsKeyUp(Keys.C))
            {
                keys.Add(Keys.C);
            }

            if (newState.IsKeyDown(Keys.S) && !OldKeyboardState.IsKeyDown(Keys.S))
            {
                IsMoving = true;
            }
            else if (newState.IsKeyDown(Keys.S) && OldKeyboardState.IsKeyDown(Keys.S))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.Y += 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.Y += 1f;
                keys.Add(Keys.S);
                dir.Y = 1f;

                IsMoving = true;
            }
            else if (!newState.IsKeyDown(Keys.S) && OldKeyboardState.IsKeyDown(Keys.S))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.W) && !OldKeyboardState.IsKeyDown(Keys.W))
            {

                IsMoving = true;
            }
            else if (newState.IsKeyDown(Keys.W) && OldKeyboardState.IsKeyDown(Keys.W))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.Y -= 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.Y -= 1f;
                dir.Y = -1f;
                keys.Add(Keys.W);
                IsMoving = true;

            }
            else if (!newState.IsKeyDown(Keys.W) && OldKeyboardState.IsKeyDown(Keys.W))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.A) && !OldKeyboardState.IsKeyDown(Keys.A))
            {
                IsMoving = true;
            }
            else if (newState.IsKeyDown(Keys.A) && OldKeyboardState.IsKeyDown(Keys.A))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.X -= 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.X -= 1f;
                keys.Add(Keys.A);
                dir.X = -1f;
                IsMoving = true;
            }
            else if (!newState.IsKeyDown(Keys.A) && OldKeyboardState.IsKeyDown(Keys.A))
            {
                // the player was holding the key down, but has just let it go

            }

            if (newState.IsKeyDown(Keys.D) && !OldKeyboardState.IsKeyDown(Keys.D))
            {
                IsMoving = true;

            }
            else if (newState.IsKeyDown(Keys.D) && OldKeyboardState.IsKeyDown(Keys.D))
            {
                // the player is holding the key down
                //LoginManagerClient.GetCharacter()._pos.X += 3*60f * Time.DeltaTime;
                //LoginManagerClient.GetCharacter()._pos.X += 1f;
                keys.Add(Keys.D);
                dir.X = 1f;
                IsMoving = true;
            }
            else if (!newState.IsKeyDown(Keys.D) && OldKeyboardState.IsKeyDown(Keys.D))
            {
                // the player was holding the key down, but has just let it go

            }
            var movement = dir * speed * Time.DeltaTime;
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
        #endregion

        #region Free Update
        public void CheckForInput()
        {
            if (ClientNetworkManager.client.ConnectionStatus != NetConnectionStatus.Connected || Core.Scene is LoginScene)
                return;

            //Update Keyboard
            if (Input.CurrentKeyboardState.GetPressedKeys().Length > 0)
                SendKeyboardRequest(new KeyboardState(KeyboardChange()));
            else
                IsMoving = false;

            //Update mouse
            if (Input.LeftMouseButtonPressed || Input.RightMouseButtonPressed || Input.MousePositionDelta != Point.Zero)
            {
                FreeMouseChange();
            }

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
            if (AbiliyCoolDown > 1)
            {
                foreach (var KeyBind in KeyBindContainer.KeyBinds)
                {
                    if (newState.IsKeyDown(KeyBind.BindedKey) && OldKeyboardState.IsKeyUp(KeyBind.BindedKey) && KeyBind.BindedAbilitityID != -1 && Entity.GetComponent<ChannelBarComponent>() == null)
                    {
                        AbilityHead ability = KeyBind.GetAbility();
                        MessageTemplate template;
                        ChannelTemplate ct = new ChannelTemplate(KeyBind.GetAbility().AbilityName, ChannelType.Ability);
                        if ((Scene as MainScene).UICanvas.Stage.FindAllElementsOfType<TargetWindow>() != null)
                        {
                            if (ability.ChannelTime > 0)
                            {
                                template = new MessageTemplate(ct.ToJson(), MessageType.StartChanneling);
                                //ProgressBar bar = new ProgressBar(0, 1, 0.1f, false, ProgressBarStyle.Create(Color.LawnGreen, Color.White));
                                //Window window = new Window("", skin);
                                //window.AddElement(bar);
                                ChannelBarComponent label = new ChannelBarComponent(ability.ChannelTime);
                                Label l = new Label("");
                                Entity.AddComponent(label);
                                //(Scene as MainScene).UICanvas.AddComponent(label);
                            }
                            else
                                template = new MessageTemplate(ct.ToJson(), MessageType.DamageTarget);
                            MessageManager.AddToQueue(template);
                            AbiliyCoolDown = 0;
                        }
                        
                    }
                }
            }
            
            //Generated inventory
            if (newState.IsKeyDown(Keys.I) && OldKeyboardState.IsKeyUp(Keys.I))
            {
                if (!InventoryWindow.RemoveInventory(Scene))
                {
                    UIManager.GenerateInventoryWindow(skin, Scene, new Vector2(-1, -1), -1, -1);
                }

            }

            if (newState.IsKeyDown(Keys.C) && OldKeyboardState.IsKeyUp(Keys.C))
            {
                if (!CharacterWindow.RemoveCharacterWindow(Scene))
                {
                    UIManager.GenerateCharacterWindow(skin, Scene, new Vector2(-1, -1), -1, -1);
                }

            }

            if (newState.IsKeyDown(Keys.D1) && !OldKeyboardState.IsKeyDown(Keys.D1))
            {

            }
            if (newState.IsKeyDown(Keys.S) && !OldKeyboardState.IsKeyDown(Keys.S))
            {
                direction = Direction.Down;

                IsMoving = true;
            }
            if (newState.IsKeyDown(Keys.W) && !OldKeyboardState.IsKeyDown(Keys.W))
            {
                direction = Direction.Up;

                IsMoving = true;
            }
            if (newState.IsKeyDown(Keys.A) && !OldKeyboardState.IsKeyDown(Keys.A))
            {
                direction = Direction.Left;
                IsMoving = true;
            }
            if (newState.IsKeyDown(Keys.D) && !OldKeyboardState.IsKeyDown(Keys.D))
            {
                direction = Direction.Right;
                IsMoving = true;

            }
            if (newState.IsKeyDown(Keys.T) && !OldKeyboardState.IsKeyDown(Keys.T))
            {
                keys.Add(Keys.T);

            }
            if (IsMoving)
            {
                Entity.RemoveComponent<ChannelBarComponent>();
            }

            return keys.ToArray();
        }

        private bool targeting = false;
        private void FreeMouseChange()
        {
            Vector2 pos = Entity.Scene.Camera.MouseToWorldPoint();
            List<Entity> entities = Scene.FindEntitiesWithTag(7);

            if (Input.LeftMouseButtonPressed && Scene is MainScene)
            {
                if ((Scene as MainScene).UICanvas.Stage.Hit(Input.MousePosition) == null && targeting)
                {
                    TargetWindow.RemoveTargetWindow(Scene);
                    MessageTemplate template = new MessageTemplate("", MessageType.Target);
                    MessageManager.AddToQueue(template);
                    targeting = false;
                }

            }

            foreach (var entity in entities)
            {
                SpriteAnimator sp = entity.GetComponent<SpriteAnimator>();


                Rectangle rect = sp.CurrentAnimation.Sprites.ElementAt(0).SourceRect;
                Rectangle rectangle = new Rectangle(entity.Position.ToPoint(), new Point((int)(rect.Width * entity.Scale.X), (int)(rect.Height * entity.Scale.Y)));
                if (rectangle.Contains(pos) && Input.LeftMouseButtonPressed)
                {
                    MessageTemplate template = new MessageTemplate(entity.Name, MessageType.Target);
                    MessageManager.AddToQueue(template);
                    targeting = true;
                    if (Scene is MainScene)
                    {
                        MainScene scene = Scene as MainScene;
                        TargetWindow window = new TargetWindow(entity, new Vector2(Screen.Width / 2, 30), skin);
                        scene.UICanvas.Stage.AddElement(window);
                    }

                }
            }
        }

        private void SendKeyboardRequest(KeyboardState keyboardState)
        {
            string messageS = Newtonsoft.Json.JsonConvert.SerializeObject(keyboardState.GetPressedKeys());
            MessageManager.AddToQueue(new MessageTemplate(messageS, MessageType.Movement));
        }
        #endregion
    }
}
