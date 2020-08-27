using Client.Managers;
using FarseerPhysics.Dynamics;
using GameServer.General;
using GameServer.Managers;
using GameServer.Types.Components;
using GameServer.Types.Components.Components;
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
        public static bool IsShiftDown;
        public InputManager()
        {
        }

        public static bool CalculateMovement(CharacterPlayer character, Keys[] keys, long ID)
        {
            if (character == null)
                return false;
            float speed = 350;
            try
            {
                IsShiftDown = false;
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
                            character.AddItemToInventory(ItemManager.GenerateItem());
                            LoginManagerServer login = MapContainer.GetLoginByID(ID);
                            MapContainer.MoveLoginToMap(login, MapContainer.GetMapByName("MapTwo"));
                            break;
                        case Keys.LeftShift:
                            IsShiftDown = true;
                            break;

                        default:
                            break;
                    }
                    
                }
                if (dir != Vector2.Zero)
                {
                    Entity e = Core.Scene.FindEntity(character._name);
                    e.RemoveComponent<ChannelingComponent>();

                    var movement = dir * speed * Time.DeltaTime;

                    FSRigidBody body = e.GetComponent<FSRigidBody>();
                    Mover mover = e.GetComponent<Mover>();

                    if(mover.Move(movement, out CollisionResult collisionResult))
                    {
                        //Debug.DrawLine(e.Position, e.Position + collisionResult.Normal * 100, Color.Black, 1f);
                    }
                    return true;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
            
        }

    }

}
