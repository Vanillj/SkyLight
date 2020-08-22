using Client.Managers;
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
        TextComponent HealthtextComponent;
        CharacterPlayer other;

        public OtherPlayerEntity()
        {
        }

        public OtherPlayerEntity(string name) : base(name)
        {
        }

        public override void Update()
        {
            if (other != null)
            {
                other = LoginManagerClient.OtherCharacters.Find(c => c._name.Equals(other._name));
                if(other != null)
                {
                    HealthtextComponent.SetText(other.CurrentHealth.ToString());
                }
            }
            base.Update();
        }

        public OtherPlayerEntity(CharacterPlayer others) : base(others._name)
        {
            other = others;
            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, others._name, Vector2.Zero, Color.White);
            HealthtextComponent = new TextComponent(Graphics.Instance.BitmapFont, others.CurrentHealth.ToString(), Vector2.Zero, Color.White);

            this.SetPosition(others.physicalPosition);
            AddComponent(new OtherPlayerComponent(others));
            AddComponent(textComponent);
            AddComponent(HealthtextComponent).SetHorizontalAlign(HorizontalAlign.Left).SetVerticalAlign(VerticalAlign.Bottom).SetRenderLayer(-2);
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
