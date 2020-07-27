using GameServer.Scenes;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Abilities
{
    class Ability
    {
        private MapLayer AbilityLayer;
        private MainScene Scene;

        public Class ClassAbility { get; set; }
        public AbilityType AbilityType { get; set; }
        public int ID { get; set; }
        public string AbilityName { get; set; }
        public int BaseDamage { get; set; }
        public int LevelRequirement { get; set; }

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

    public enum AbilityType
    {
        TravelAbility,
        ConeAbility,
        DoTAbility
    }

    public enum Class
    {
        Warrior,
        Mage,
        Archer
    }
}
