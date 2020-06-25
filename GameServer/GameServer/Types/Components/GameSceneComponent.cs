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

        public override void OnEnabled()
        {
            base.OnEnabled();
            playerTexture = Scene.Content.Load<Texture2D>("images/playerTexture");

            //Create world later
            Scene.CreateEntity("Object").SetPosition(new Vector2(200, 200))
                .AddComponent<FSRigidBody>()
                .SetBodyType(BodyType.Static)
                .AddComponent<FSCollisionCircle>()
                .SetRadius(playerTexture.Width / 2)
                .AddComponent(new SpriteRenderer(playerTexture));
        }

        float total = 0;
        public override void Update()
        {
            base.Update();

            float delta = Time.DeltaTime;

            total += delta;
            if (total < 0.05)
                return;
            total = 0;

            HashSet<LoginManagerServer> characterlist =  CharacterManager.GetLoginManagerServerList();
            if (characterlist == null || characterlist.Count < 1)
                return;

            foreach (LoginManagerServer login in characterlist)
            {
                CharacterPlayer c = login.AccountCharacter;
                Entity e = Core.Scene.FindEntity(c._name);
                if (e != null)
                {
                    var tween = e.Transform.TweenPositionTo(c._pos, 0.033f);
                    tween.Start();
                }
                else
                {
                    Entity temp = Scene.CreateEntity(c._name).SetPosition(c._pos);
                    temp.AddComponent<FSRigidBody>()
                    .SetBodyType(BodyType.Dynamic)
                    .AddComponent<FSCollisionCircle>()
                    .SetRadius(playerTexture.Width / 2)
                    .AddComponent(new SpriteRenderer(playerTexture))
                    .AddComponent(new PlayerComponent(login));

                    var body = temp.GetComponent<FSRigidBody>();
                    body.Body.FixedRotation = true;
                    body.SetGravityScale(0);
                }

            }
        }
    }
}
