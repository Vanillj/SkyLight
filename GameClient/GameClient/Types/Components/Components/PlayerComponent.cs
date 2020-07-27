using GameClient.Types.Components.SceneComponents;
using GameServer.General;
using Nez;
using Nez.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.Components.Components
{
    class PlayerComponent : Component, IUpdatable
    {
        private Direction oldDir;

        public void Update()
        {

            Direction dir = InputComponent.direction;
            bool mov = InputComponent.IsMoving;
            SpriteAnimator ani = Entity.GetComponent<SpriteAnimator>();
            if (oldDir != dir)
            {
                if (InputComponent.direction == Direction.Left)
                {
                    ani.FlipX = true;
                }
                else if (dir == Direction.Right)
                {
                    ani.FlipX = false;
                }
                oldDir = dir;
            }

            if (mov)
                ani.Play("Movement");
            else
                ani.Play("Idle");
        }
    }
}
