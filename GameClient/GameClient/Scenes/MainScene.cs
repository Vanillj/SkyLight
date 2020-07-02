using Client.Managers;
using FarseerPhysics.Dynamics;
using GameClient.Managers;
using GameClient.Types.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.BitmapFonts;
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

            //TODO: Setup character from recieved data

            playerTexture = Content.Load<Texture2D>("images/playerTexture");
            player = CreateEntity("Player");
            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, "Username", new Vector2(0, 0), Color.White).SetHorizontalAlign(HorizontalAlign.Center).SetVerticalAlign(VerticalAlign.Top);
            player.SetPosition(new Vector2(0, 0))
                .AddComponent(new SpriteRenderer(playerTexture)).AddComponent(textComponent);

            CreateEntity("Object").SetPosition(new Vector2(200, 200))
                .AddComponent(new SpriteRenderer(playerTexture));
            FollowCamera fCamera = new FollowCamera(player, FollowCamera.CameraStyle.LockOn){ FollowLerp = 0.2f };
            player.AddComponent(fCamera);

            Table.Row();
            label = new Label("Logged in!").SetFontScale(3);
            Table.Add(label);
        }
        public override void Update()
        {
            if (InputManager != null)
                MessageManager.inputManager.CheckForInput();

            if(LoginManagerClient.Othercharacters != null)
            {
                foreach (CharacterPlayer others in LoginManagerClient.Othercharacters)
                {
                    Entity e = FindEntity(others._name);
                    
                    float delta = Time.DeltaTime;
                    if (e == null)
                    {
                        //TODO: Change so it's based on player size, might not be nessesary
                        //CreateEntity(others._name).SetPosition(others._pos).AddComponent(new SpriteRenderer(playerTexture)).AddComponent(new PlayerComponent(others)).AddComponent(new TextComponent(Graphics.Instance.BitmapFont, others._name, new Vector2(-others.playerTexture.Width / 2, -others.playerTexture.Height / 2), Color.White));

                        CreateEntity(others._name).SetPosition(others._pos).AddComponent(new SpriteRenderer(playerTexture)).AddComponent(new PlayerComponent(others)).AddComponent(new TextComponent(Graphics.Instance.BitmapFont, others._name, new Vector2(0, 0), Color.White).SetVerticalAlign(VerticalAlign.Top).SetHorizontalAlign(HorizontalAlign.Center));
                    }
                }
            }



            //Vector2 ClientsidePos = LoginManagerClient.GetCharacter()._pos;
            Vector2 ClientsidePos = LoginManagerClient.GetCharacter().physicalPosition;


            //Change later, this is for client side prediction
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
