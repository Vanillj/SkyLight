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
        public PlayerEntity()
        {

        }

        public PlayerEntity(string name) : base(name)
        {

        }

        public PlayerEntity(CharacterPlayer player)
        {
            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, player._name, Vector2.Zero, Color.White);

            this.SetPosition(player.physicalPosition);
            AddComponent(textComponent).SetHorizontalAlign(HorizontalAlign.Center).SetVerticalAlign(VerticalAlign.Top).SetRenderLayer(-200);
            AddComponent(new PlayerComponent());

            SpriteAnimation Idle = TextureContainer.KnightAnimationAtlas.GetAnimation("Idle");
            SpriteAnimation Movement = TextureContainer.KnightAnimationAtlas.GetAnimation("Movement");

            SpriteAnimator ani = textComponent.AddComponent<SpriteAnimator>();
            ani.AddAnimation("Idle", Idle);
            ani.AddAnimation("Movement", Movement);
            ani.Play("Idle");
            this.SetScale(3.5f);
        }


    }
}
