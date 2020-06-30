using Client.Managers;
using FarseerPhysics.Dynamics;
using GameClient.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Farseer;
using Nez.Sprites;
using Nez.Tweens;
using Nez.UI;
using Server.Scenes;
using Server.Types;
using System;

namespace GameClient.Scenes
{
    class MainScene : BaseScene
    {
        public override Table Table { get; set; }
        //public override MessageManager MessageManager { get; set; }
        public InputManager InputManager { get; set; }

        private Label label;
        private Entity player;
        Texture2D playerTexture;
        public override void Initialize()
        {
            base.Initialize();

            playerTexture = Content.Load<Texture2D>("images/playerTexture");
            player = CreateEntity("Player");
            player.SetPosition(new Vector2(0, 0))
                .AddComponent(new SpriteRenderer(playerTexture));

            CreateEntity("Object").SetPosition(new Vector2(200, 200))
                .AddComponent(new SpriteRenderer(playerTexture));
            FollowCamera fCamera = new FollowCamera(player, FollowCamera.CameraStyle.LockOn);
            fCamera.FollowLerp = 0.1f;
            player.AddComponent(fCamera);

            Table.Row();
            label = new Label("Logged in!").SetFontScale(3);
            Table.Add(label);
        }
        public override void Update()
        {
            if (InputManager != null)
                MessageManager.inputManager.CheckForInput();

            if(LoginManagerClient.Othercharacter != null)
            {
                foreach (CharacterPlayer others in LoginManagerClient.Othercharacter)
                {
                    Entity e = Core.Scene.FindEntity(others._name);
                    
                    float delta = Time.DeltaTime;
                    if (e != null)
                    {
                        //Vector2 diff = e.Transform.LocalToWorldTransform.Translation - others._pos;
                        //e.Transform.Position += diff;
                        var tween = e.Transform.TweenPositionTo(others.physicalPosition, 0.01f);
                        tween.Start();
                    }
                    else
                    {
                        Entity temp = CreateEntity(others._name).SetPosition(others._pos);
                            temp.AddComponent(new SpriteRenderer(playerTexture));

                    }
                }
            }

            //Vector2 ClientsidePos = LoginManagerClient.GetCharacter()._pos;
            Vector2 ClientsidePos = LoginManagerClient.GetCharacter().physicalPosition;

            if (ClientsidePos != player.Position)
            {
                //If error is too big, 
                Vector2 recieved = LoginManagerClient.GetCharacter().physicalPosition;

                float diff = recieved.Length() - ClientsidePos.Length();
                Console.WriteLine(diff);

                if (Math.Abs(diff) > 5)
                {
                    LoginManagerClient.GetCharacter()._pos = recieved;
                    ClientsidePos = recieved;
                }

                ITween<Vector2> tween = player.Transform.TweenPositionTo(ClientsidePos, 0.01f);
                tween.Start();
            }
            
            base.Update();
            
        }
    }
}
