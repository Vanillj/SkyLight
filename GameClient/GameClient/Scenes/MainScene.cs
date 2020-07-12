using Client.Managers;
using GameClient.General;
using GameClient.Managers;
using GameClient.Types.Components;
using GameClient.Types.Components.SceneComponents;
using GameClient.Types.Item;
using GameServer.General;
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
        Texture2D playerTexture = Core.Content.Load<Texture2D>("images/playerTexture");
        public override Table Table { get; set; }

        public Entity player;

        public MainScene()
        { }

        public override void Initialize()
        {
            base.Initialize();

            //Load Assets, always load textures before generating items so they can match texture and ID
            new TextureContainer().LoadTextures();
            int TextureArrayLength = 2;

            new ItemContainer().GenerateItems(TextureContainer.Texture2DEList);
            
        }
        public override void Update()
        {
            if(LoginManagerClient.OtherCharacters != null)
            {
                foreach (CharacterPlayer others in LoginManagerClient.OtherCharacters)
                {
                    Entity e = FindEntity(others._name);
                    
                    float delta = Time.DeltaTime;
                    if (e == null)
                    {
                        CreateEntity(others._name).SetPosition(others._pos).AddComponent(new SpriteRenderer(playerTexture)).AddComponent(new PlayerComponent(others)).AddComponent(new TextComponent(Graphics.Instance.BitmapFont, others._name, new Vector2(0, 0), Color.White).SetVerticalAlign(VerticalAlign.Top).SetHorizontalAlign(HorizontalAlign.Center)).SetRenderLayer(2);
                    }
                }
            }


            //Vector2 ClientsidePos = LoginManagerClient.GetCharacter()._pos;
            Vector2 ClientsidePos = LoginManagerClient.GetCharacter().physicalPosition;


            //Change later, this is for client side prediction
            if (player != null)
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

                player.Position = new Vector2(MathHelper.Lerp(player.Position.X, ClientsidePos.X, 0.1f), MathHelper.Lerp(player.Position.Y, ClientsidePos.Y, 0.1f));
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
