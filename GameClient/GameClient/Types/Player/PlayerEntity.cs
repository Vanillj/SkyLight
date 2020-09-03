using Client.Managers;
using GameClient.General;
using GameClient.Types.Components.Components;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.Player
{
    class PlayerEntity : Entity
    {
        CharacterPlayer player;
        private TextComponent HealthtextComponent;

        public PlayerEntity()
        {
            
        }

        public PlayerEntity(string name) : base(name)
        {

        }

        public override void Update()
        {
            if (player != null)
            {
                player = LoginManagerClient.GetCharacter();

                HealthtextComponent.SetText(player.CurrentHealth.ToString());
            }
            base.Update();
        }


        public PlayerEntity(CharacterPlayer player)
        {
            this.player = player;
            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, player._name, Vector2.Zero, Color.White);
            HealthtextComponent = new TextComponent(Graphics.Instance.BitmapFont, player.CurrentHealth.ToString(), Vector2.Zero, Color.White);
            SetTag(2);
            this.SetPosition(player.physicalPosition);
            AddComponent(textComponent).SetHorizontalAlign(HorizontalAlign.Center).SetVerticalAlign(VerticalAlign.Top).SetRenderLayer(-200);
            AddComponent(new PlayerComponent());
            AddComponent(HealthtextComponent).SetHorizontalAlign(HorizontalAlign.Right).SetVerticalAlign(VerticalAlign.Bottom).SetRenderLayer(-200);

            SpriteAnimation Idle = TextureContainer.GetSpriteAtlasByName("Knight").GetAnimation("Idle");
            SpriteAnimation Movement = TextureContainer.GetSpriteAtlasByName("Knight").GetAnimation("Movement");

            SpriteAnimator ani = textComponent.AddComponent<SpriteAnimator>();
            ani.AddAnimation("Idle", Idle);
            ani.AddAnimation("Movement", Movement);
            ani.Play("Idle");
            this.SetScale(3.5f);
        }


    }
}
