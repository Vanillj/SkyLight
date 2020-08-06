using GameServer.Scenes;
using GameServer.Types.Abilities.SharedAbilities;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Abilities
{
    class Ability : AbilityHead
    {
        private MapLayer AbilityLayer;
        private MainScene Scene;

        public Ability()
        {

        }

        public void SetMapLayer(MapLayer layer)
        {
            AbilityLayer = layer;
        }

        public Entity GenerateAbilityEntity()
        {
            if (Scene != null)
            {
                Scene.CreateEntity(AbilityName);
            }
            return null;
        }
    }
}
