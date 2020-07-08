using Client.Managers;
using FarseerPhysics.Dynamics;
using GameServer.Types.Components;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Farseer;
using Server.Scenes;
using Server.Types;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Server.Managers
{
    class CharacterManager
    {
        private static HashSet<LoginManagerServer> LoginManagerServerList = new HashSet<LoginManagerServer>();

        public static void AddLoginManagerServerToList(LoginManagerServer login)
        {
            LoginManagerServerList.Add(login);
        }

        public static void RemoveLoginManagerServerFromListUniqueID(float UniqueID)
        {
            LoginManagerServerList.RemoveWhere(l => l.GetUniqueID().Equals(UniqueID)); ;
        }

        public static void RemoveLoginManagerServerFromListLoginManager(LoginManagerServer login)
        {
            LoginManagerServerList.Remove(login);
        }
        public static void ChangeCharacterPosition(Vector2 vector, float uniqueID)
        {
            CharacterPlayer c = GetLoginManagerFromUniqueID(uniqueID).GetCharacter();
            if (c != null)
            {
                c._pos.X += vector.X;
                c._pos.Y += vector.Y;
            }

        }

        public static HashSet<LoginManagerServer> GetLoginManagerServerList()
        {
            return LoginManagerServerList;
        }

        public static LoginManagerServer GetLoginManagerFromUniqueID(float uniqueID)
        {
            foreach (LoginManagerServer l in LoginManagerServerList)
            {
                if (l.GetUniqueID() == uniqueID)
                {
                    return l;
                }
            }
            return null;
        }

        public static void AddCharacterToScene(Scene scene, LoginManagerServer login)
        {
            CharacterPlayer character = login.GetCharacter();
            //might not be needed
            Entity e = Core.Scene.FindEntity(character._name);
            if (e == null)
            {
                FSRigidBody fbody = new FSRigidBody().SetBodyType(BodyType.Dynamic).SetIgnoreGravity(true).SetLinearDamping(15f);

                scene.CreateEntity(character._name).SetPosition(character.physicalPosition)
                    .AddComponent(fbody)
                    .AddComponent(new FSCollisionCircle(100))
                    .AddComponent(new PlayerComponent(login))
                    .AddComponent(new Mover())
                    .AddComponent(new CircleCollider(100));
                fbody.Body.FixedRotation = true;

            }
        }

        public static void RemoveCharacterFromScene(Scene scene, string name)
        {
            scene.FindEntity(name).Destroy();
        }

    }
}
