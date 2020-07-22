using Client.Managers;
using FarseerPhysics.Dynamics;
using GameServer.General;
using GameServer.Types;
using GameServer.Types.Components;
using Nez;
using Nez.Farseer;
using Server.Types;
using System.Collections.Generic;

namespace Server.Managers
{
    class CharacterManager
    {

        public static void RemoveCharacterFromScene(Scene scene, string name)
        {
            var e = scene.FindEntity(name);
            if (e != null)
                e.Destroy();
        }

    }
}
