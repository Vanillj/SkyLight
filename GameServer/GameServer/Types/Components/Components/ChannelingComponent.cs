using GameServer.Types.Abilities;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Components.Components
{
    class ChannelingComponent : Component, IUpdatable
    {
        private float ChannelTime = 0; //In seconds
        private float TotalTime = 0;
        private PlayerComponent playerComponent;
        private Entity target;

        public ChannelingComponent(PlayerComponent pc, float ChannelTime)
        {
            this.ChannelTime = ChannelTime;
            playerComponent = pc;
            target = playerComponent.Target;
            playerComponent.isChanneling = true;
        }

        public void Update()
        {
            TotalTime += Time.DeltaTime;
            if (TotalTime * 100 >= ChannelTime)
            {
                //execute something on Entity such as ability
                if (target != null)
                {
                    target.GetComponent<DamageComponent>().AddDoTAbility(new DoTAbility());
                }
                this.RemoveComponent();
            }
        }
        public override void OnAddedToEntity()
        {

            base.OnAddedToEntity();
        }
        public override void OnRemovedFromEntity()
        {
            playerComponent.isChanneling = false;
            base.OnRemovedFromEntity();
        }

        public override void OnDisabled()
        {
            //removes this component
            playerComponent.isChanneling = false;
            base.OnDisabled();
        }
    }
}
