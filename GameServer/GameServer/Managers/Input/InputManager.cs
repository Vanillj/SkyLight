using FarseerPhysics.Dynamics;
using GameServer.Managers;
using GameServer.Types.Item;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Farseer;
using Server.Types;
using System;

namespace Server.Managers
{
    class InputManager
    {
        public InputManager()
        {
        }

        public static void CalculateMovement(CharacterPlayer character, Keys[] keys)
        {
            if (character == null)
                return;
            float speed = 100;
            try
            {
                
                var dir = Vector2.Zero;
                foreach (var key in keys)
                {
                    //add proper movement with deltaTime later
                    switch (key)
                    {
                        case Keys.W:
                            //character._pos.Y -= 3f;
                            dir.Y = -1f;
                            break;
                        case Keys.A:
                            //character._pos.X -= 3f;
                            dir.X = -1f;
                            break;
                        case Keys.S:
                            //character._pos.Y += 3f;
                            dir.Y = 1f;
                            break;
                        case Keys.D:
                            //character._pos.X += 3f;
                            dir.X = 1f;
                            break;
                        case Keys.T:
                            Entity e = Core.Scene.FindEntity(character._name);
                            e.Position = Vector2.Zero;
                            e.Transform.Position = Vector2.Zero;
                            character.AddItemToInventory(ItemManager.GenerateItem());
                            break;

                        default:
                            break;
                    }
                    
                }
                if (dir != Vector2.Zero)
                {
                    var movement = dir * speed * Time.DeltaTime;
                    Entity e = Core.Scene.FindEntity(character._name);
                    FSRigidBody body = e.GetComponent<FSRigidBody>();

                    Mover mover = e.GetComponent<Mover>();

                    if(mover.Move(movement, out CollisionResult collisionResult))
                    {
                        //Debug.DrawLine(e.Position, e.Position + collisionResult.Normal * 100, Color.Black, 1f);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            
        }

    }

}
