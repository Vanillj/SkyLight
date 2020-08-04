using GameClient.General;
using GameClient.Types.Components;
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
    class OtherPlayerEntity : Entity
    {
        public OtherPlayerEntity()
        {
        }

        public OtherPlayerEntity(string name) : base(name)
        {
        }

        public OtherPlayerEntity(CharacterPlayer others) : base(others._name)
        {
            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, others._name, Vector2.Zero, Color.White);
            this.SetPosition(others.physicalPosition);
            AddComponent(new OtherPlayerComponent(others));
            AddComponent(new TextComponent(Graphics.Instance.BitmapFont, others._name, new Vector2(0, 0), Color.White)
                .SetVerticalAlign(VerticalAlign.Top).SetHorizontalAlign(HorizontalAlign.Center)).SetRenderLayer(2);
            AddComponent(textComponent);
            SetTag(7);
            this.SetScale(3.5f);

            SpriteAnimation Idle = TextureContainer.KnightAnimationAtlas.GetAnimation("Idle");
            SpriteAnimation Movement = TextureContainer.KnightAnimationAtlas.GetAnimation("Movement");

            SpriteAnimator animator = AddComponent<SpriteAnimator>();
            animator.AddAnimation("Idle", Idle);
            animator.AddAnimation("Movement", Movement);
            animator.Play("Idle");
        }
    }
}
