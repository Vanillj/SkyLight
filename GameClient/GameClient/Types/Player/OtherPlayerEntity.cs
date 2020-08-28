using Client.Managers;
using GameClient.General;
using GameClient.Scenes;
using GameClient.Types.Components;
using GameServer.General;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Nez.UI;
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
        //TextComponent HealthtextComponent;
        CharacterPlayer other;
        ProgressBar bar;
        private int past = 100;
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
                if (other != null)
                {
                    other.MaxHealth = 100;
                    Vector2 p = Scene.Camera.WorldToScreenPoint(other.physicalPosition);
                    bar.SetPosition(p.X - bar.PreferredWidth / 2, p.Y - bar.PreferredHeight / 2);
                    if (past != other.CurrentHealth)
                    {
                        bar.SetValue(other.CurrentHealth / (float)other.MaxHealth);
                        //HealthtextComponent.SetText(other.CurrentHealth.ToString());
                        past = other.CurrentHealth;
                    }
                }
            }
            base.Update();
        }

        public OtherPlayerEntity(CharacterPlayer others) : base(others._name)
        {
            others.MaxHealth = 100;

            other = others;
            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, others._name, Vector2.Zero, Color.White);
            //HealthtextComponent = new TextComponent(Graphics.Instance.BitmapFont, others.CurrentHealth.ToString(), Vector2.Zero, Color.White);

            this.SetPosition(others.physicalPosition);
            AddComponent(new OtherPlayerComponent(others));
            AddComponent(textComponent);
            //AddComponent(HealthtextComponent).SetHorizontalAlign(HorizontalAlign.Left).SetVerticalAlign(VerticalAlign.Bottom).SetRenderLayer(-2);
            SetTag(7);
            this.SetScale(3.5f);

            SpriteAnimation Idle = TextureContainer.KnightAnimationAtlas.GetAnimation("Idle");
            SpriteAnimation Movement = TextureContainer.KnightAnimationAtlas.GetAnimation("Movement");

            SpriteAnimator animator = AddComponent<SpriteAnimator>();
            animator.AddAnimation("Idle", Idle);
            animator.AddAnimation("Movement", Movement);
            animator.Play("Idle");

        }

        public override void OnAddedToScene()
        {
            other.MaxHealth = 100;
            past = other.MaxHealth;
            bar = new ProgressBar(0, 1, 0.02f, false, ConstantValues.skin);
            bar.SetWidth(100);
            bar.SetValue(1);
            Vector2 p = Scene.Camera.WorldToScreenPoint(other.physicalPosition);
            bar.SetPosition(p.X - bar.PreferredWidth/2, p.Y - bar.PreferredHeight/2);
            (Scene as MainScene).UICanvas.Stage.AddElement(bar);
            base.OnAddedToScene();
        }
    }
}
