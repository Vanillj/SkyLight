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

        public override void OnEnabled()
        {
            base.OnEnabled();

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
        }
    }
}
