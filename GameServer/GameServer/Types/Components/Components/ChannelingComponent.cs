using GameServer.Types.Abilities;
using GameServer.Types.Abilities.SharedAbilities;
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

        public ChannelingComponent(PlayerComponent pc, float ChannelTime)
        {
            this.ChannelTime = ChannelTime;
            playerComponent = pc;
            playerComponent.isChanneling = true;
        }

        public void Update()
        {
            TotalTime += Time.DeltaTime;
            if (TotalTime >= ChannelTime)
            {
                ChannelExecute();
                this.RemoveComponent();
            }
        }

        public virtual void ChannelExecute()
        {

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
