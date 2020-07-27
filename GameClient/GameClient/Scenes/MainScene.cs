using Client.Managers;
using GameClient.General;
using GameClient.Managers;
using GameClient.Types.Components;
using GameClient.Types.Components.Components;
using GameClient.Types.Components.SceneComponents;
using GameClient.Types.Item;
using GameServer.General;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.Tweens;
using Nez.UI;
using Server.Scenes;
using Server.Types;
using System;
using System.Collections.Generic;

namespace GameClient.Scenes
{
    class MainScene : BaseScene
    {
        float timer = 0;

        SpriteAnimation Idle = TextureContainer.KnightAnimationAtlas.GetAnimation("Idle");
        SpriteAnimation Movement = TextureContainer.KnightAnimationAtlas.GetAnimation("Movement");
        public override Table Table { get; set; }

        public Entity player;
        public PlayerComponent playerComponent;

        public MainScene() : base() { }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            timer += Time.DeltaTime;

            if (timer >= ConstantValues.UpdateFrequency)
            {
                timer = 0;
                if (LoginManagerClient.OtherCharacters != null)
                {
                    foreach (CharacterPlayer others in LoginManagerClient.OtherCharacters)
                    {
                        Entity OtherEntity = FindEntity(others._name);

                        if (OtherEntity == null)
                        {
                            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, others._name, Vector2.Zero, Color.White);
                            OtherEntity = CreateEntity(others._name);
                            OtherEntity.SetPosition(others.physicalPosition)
                                .AddComponent(new OtherPlayerComponent(others))
                                .AddComponent(new TextComponent(Graphics.Instance.BitmapFont, others._name, new Vector2(0, 0), Color.White)
                                .SetVerticalAlign(VerticalAlign.Top).SetHorizontalAlign(HorizontalAlign.Center))
                                .SetRenderLayer(2);
                            OtherEntity.AddComponent(textComponent);
                            OtherEntity.SetTag(7);
                            OtherEntity.SetScale(3.5f);

                            SpriteAnimator animator = OtherEntity.AddComponent<SpriteAnimator>();
                            animator.AddAnimation("Idle", Idle);
                            animator.AddAnimation("Movement", Movement);
                            animator.Play("Idle");
                        }
                    }
                }


                //Vector2 ClientsidePos = LoginManagerClient.GetCharacter()._pos;
                Vector2 ClientsidePos = LoginManagerClient.GetCharacter().physicalPosition;


                //Change later, this is for client side prediction
                if (player != null)
                    MovementPrediction(ClientsidePos, player);

                //TODO: Ability prediction etc.

            }
            base.Update();
        }

        private void MovementPrediction(Vector2 ClientsidePos, Entity player)
        {
            if (ClientsidePos != player.Position)
            {
                //If error is too big, 
                Vector2 recieved = LoginManagerClient.GetCharacter().physicalPosition;

                float diff = recieved.Length() - ClientsidePos.Length();
                //Console.WriteLine(diff);

                if (Math.Abs(diff) > 25)
                {
                    LoginManagerClient.GetCharacter()._pos = recieved;
                    ClientsidePos = recieved;
                }

                if (player.Position.Length() < 1250)
                {
                    player.Position = new Vector2(MathHelper.Lerp(player.Position.X, ClientsidePos.X, 0.1f), MathHelper.Lerp(player.Position.Y, ClientsidePos.Y, 0.1f));
                }
                else
                {
                    player.Position = ClientsidePos;
                }
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
