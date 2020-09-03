using Client.Managers;
using GameClient.Types.Components.SceneComponents;
using GameServer.General;
using Microsoft.Xna.Framework;
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
        private float timer = 0;
        private Entity Target;

        public void Update()
        {
            timer += Time.DeltaTime;

            if (timer >= ConstantValues.UpdateFrequency)
            {
                timer -= ConstantValues.UpdateFrequency;
                FixedUpdate();

            }

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

        private void FixedUpdate()
        {
            Vector2 ClientsidePos = LoginManagerClient.GetCharacter().physicalPosition;
            MovementPrediction(ClientsidePos);
        }

        private void MovementPrediction(Vector2 ClientsidePos)
        {
            if (ClientsidePos != Entity.Position)
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

                if (Entity.Position.Length() < 1250)
                {
                    //player.Position = new Vector2(MathHelper.Lerp(player.Position.X, ClientsidePos.X, 0.1f), MathHelper.Lerp(player.Position.Y, ClientsidePos.Y, 0.1f));
                    Entity.Position = ClientsidePos;
                }
                else
                {
                    Entity.Position = ClientsidePos;
                }
            }
        }

        public Entity GetTarget()
        {
            return Target;
        }
        public void SetTarget(Entity target)
        {
            Target = target;
        }
        public void RemoveTarget()
        {
            SetTarget(null);
        }
    }
}
