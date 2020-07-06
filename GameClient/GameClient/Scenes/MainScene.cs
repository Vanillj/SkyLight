using Client.Managers;
using GameClient.Managers;
using GameClient.Types.Components;
using GameClient.Types.Components.SceneComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
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

        private Label label;
        private Entity player;
        Texture2D playerTexture;

        public MainScene()
        { }

        public override void Initialize()
        {
            base.Initialize();

            //TODO: Setup character from recieved data

            playerTexture = Content.Load<Texture2D>("images/playerTexture");
            player = CreateEntity("Player");
            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, "Username", new Vector2(0, 0), Color.White).SetHorizontalAlign(HorizontalAlign.Center).SetVerticalAlign(VerticalAlign.Top);
            player.SetPosition(new Vector2(0, 0))
                .AddComponent(new SpriteRenderer(playerTexture)).AddComponent(textComponent).SetRenderLayer(-200);

            FollowCamera fCamera = new FollowCamera(player, FollowCamera.CameraStyle.CameraWindow){ FollowLerp = 0.2f };
            player.AddComponent(fCamera);

            Table.Row();
            label = new Label("Logged in!").SetFontScale(3);
            Table.Add(label);
        }
        public override void Update()
        {
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

                        CreateEntity(others._name).SetPosition(others._pos).AddComponent(new SpriteRenderer(playerTexture)).AddComponent(new PlayerComponent(others)).AddComponent(new TextComponent(Graphics.Instance.BitmapFont, others._name, new Vector2(0, 0), Color.White).SetVerticalAlign(VerticalAlign.Top).SetHorizontalAlign(HorizontalAlign.Center)).SetRenderLayer(2);
                    }
                }
            }



            Vector2 ClientsidePos = LoginManagerClient.GetCharacter()._pos;
            //Vector2 ClientsidePos = LoginManagerClient.GetCharacter().physicalPosition;


            //Change later, this is for client side prediction
            MovementPrediction(ClientsidePos, player);
            
            //TODO: Ability prediction etc.
            
            base.Update();
            
        }

        private void MovementPrediction(Vector2 ClientsidePos, Entity player)
        {
            if (ClientsidePos != player.Position)
            {
                //If error is too big, 
                Vector2 recieved = LoginManagerClient.GetCharacter().physicalPosition;

                float diff = recieved.Length() - ClientsidePos.Length();
                Console.WriteLine(diff);

                if (Math.Abs(diff) > 25)
                {
                    LoginManagerClient.GetCharacter()._pos = recieved;
                    ClientsidePos = recieved;
                }

                ITween<Vector2> tween = player.Transform.TweenPositionTo(ClientsidePos, 0.01f);
                tween.Start();
            }
        }

        public override void OnStart()
        {
            AddSceneComponent<MessageSceneComponent>();
            AddSceneComponent<TileSceneComponent>();
            AddSceneComponent(new InputComponent(player, Core.Scene.Camera));
            base.OnStart();
        }

    }
}
