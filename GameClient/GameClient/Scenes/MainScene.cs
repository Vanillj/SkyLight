using Client.Managers;
using GameClient.General;
using GameClient.Managers;
using GameClient.Types.Abilities;
using GameClient.Types.Components;
using GameClient.Types.Components.Components;
using GameClient.Types.Components.SceneComponents;
using GameClient.Types.Item;
using GameClient.Types.Player;
using GameClient.UI.Elements;
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

        public PlayerEntity player;
        public PlayerComponent playerComponent;

        public MainScene() : base() { }

        public override void Initialize()
        {
            SetDesignResolution(Screen.Width, Screen.Height, SceneResolutionPolicy.ShowAll);
            
            base.Initialize();
            
        }

        public override void Update()
        {
            timer += Time.DeltaTime;

            if (timer >= ConstantValues.UpdateFrequency)
            {
                timer -= ConstantValues.UpdateFrequency;
                FixedUpdate();

            }
            base.Update();
        }

        private void FixedUpdate()
        {
            if (LoginManagerClient.OtherCharacters != null)
            {
                foreach (CharacterPlayer others in LoginManagerClient.OtherCharacters)
                {
                    Entity OtherEntity = FindEntity(others._name);

                    if (OtherEntity == null)
                    {
                        OtherPlayerEntity oPE = new OtherPlayerEntity(others);
                        AddEntity(oPE);
                    }
                }
            }

           
        }


        public override void OnStart()
        {
            AddSceneComponent<MessageSceneComponent>();
            AddSceneComponent<TileSceneComponent>();
            AddSceneComponent(new InputComponent(player, Core.Scene.Camera));
            UICanvas.Stage.AddElement(new AbilityBarWindow("", InputComponent.skin));
            base.OnStart();
        }

    }
}
