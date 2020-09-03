using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClient.General;
using GameClient.Scenes;
using GameServer.Types.Abilities.SharedAbilities;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;

namespace GameClient.Types.Components.SceneComponents
{
    class AbilityAnimationEntity : Entity
    {
        SpriteAnimation animation;
        AbilityHead ability;
        Entity target;
        float totalTime = 0;
        Vector2 source;
        SpriteAnimator animator;

        public AbilityAnimationEntity(AbilityHead ability, Entity target, Vector2 source)
        {
            this.ability = ability;
            this.target = target;
            this.SetScale(0.25f);
            this.source = source;

            switch (ability.ID)
            {
                case 1:
                    animation = TextureContainer.GetSpriteAtlasByName("FireBall").GetAnimation("travel");
                    break;
                default:
                    break;
            }

            float deltaX = target.Position.X - source.X;
            this.SetPosition(source);
            animator = AddComponent<SpriteAnimator>();
            if (deltaX > 0)
                animator.FlipX = true;
            animator.AddAnimation("travel", animation);
            animator.Play("travel");
        }

        public override void Update()
        {
            totalTime += Time.DeltaTime;

            float deltaX = target.Position.X - Position.X;
            float deltaY = target.Position.Y - Position.Y;

            float percentTime = totalTime / ability.TravelTime;
            Console.WriteLine(percentTime);
            this.SetPosition(source + new Vector2(deltaX * percentTime, deltaY * percentTime));

            if (totalTime >= ability.TravelTime)
            {
                Destroy();
            }

            float deltaM = target.Position.Length() - Position.Length();

            if (deltaX > 0 && !animator.FlipX)
                animator.FlipX = true;
            else if (deltaX < 0 && animator.FlipX)
                animator.FlipX = false;


            base.Update();
        }
    }
}
