using Nez;
using System.Collections.Generic;
using Server.Managers;
using Client.Managers;
using Server.Types;
using Nez.Farseer;
using Nez.Sprites;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameServer.Types.Components;

namespace GameServer.Types
{
    class GameSceneComponent : SceneComponent
    {

        public GameSceneComponent()
        {

        }

        Texture2D playerTexture;

        //TEMP
        const int playerRadius = 100;

        public override void OnEnabled()
        {
            base.OnEnabled();
            playerTexture = Scene.Content.Load<Texture2D>("images/playerTexture");

        }

        float total = 0;
        public override void Update()
        {
            base.Update();

            float delta = Time.DeltaTime;

            //updates every 1/20 second
            total += delta;
            if (total < 0.05)
                return;
            total = 0;

            HashSet<LoginManagerServer> characterlist = CharacterManager.GetLoginManagerServerList();
            if (characterlist != null || characterlist.Count > 0)
            {
                foreach (LoginManagerServer login in characterlist)
                {
                    CharacterPlayer c = login.AccountCharacter;
                    Entity e = Core.Scene.FindEntity(c._name);
                    if (e == null)
                    {
                        FSRigidBody fbody = new FSRigidBody().SetBodyType(BodyType.Dynamic).SetIgnoreGravity(true).SetLinearDamping(15f);
                        

                        Scene.CreateEntity(c._name).SetPosition(c._pos)
                            .AddComponent(fbody)
                            .AddComponent(new FSCollisionCircle(100))
                            .AddComponent(new PlayerComponent(login))
                            .AddComponent(new Mover())
                            .AddComponent(new CircleCollider(100));
                        fbody.Body.FixedRotation = true;

                    }
                }
            }
        }
    }
}
