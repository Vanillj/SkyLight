using GameClient.Scenes;
using GameClient.Types.Components.Components;
using GameClient.Types.Components.SceneComponents;
using GameClient.UI.Elements;
using GameServer.General;
using GameServer.Types.Abilities.SharedAbilities;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.UI.Component
{
    class ChannelBarComponent : SceneComponent
    {
        private float TotalTime = 0;
        private float StartTime;
        ChannelBarWindow channelBarWindow;
        AbilityHead ability;
        Vector2 source;
        Entity target;

        public ChannelBarComponent(int startTime, AbilityHead ability, Entity target, Vector2 source)
        {
            //SetText(startTime.ToString());
            StartTime = startTime;
            this.ability = ability;
            this.source = source;
            this.target = target;
        }

        public override void Update()
        {
            TotalTime += Time.DeltaTime;
            double diff = Math.Round(StartTime - TotalTime, 2);
            if (diff <= 0)
            {
                if (this != null)
                {
                    Scene.AddEntity(new AbilityAnimationEntity(ability, target, source));
                    channelBarWindow.RemoveElements();
                    Scene.RemoveSceneComponent<ChannelBarComponent>();
                }
            }
            else
            {
                channelBarWindow.UpdateBar((TotalTime / StartTime), (float)Math.Round((StartTime - TotalTime), 3));
            }
            base.Update();
        }
        
        public override void OnRemovedFromScene()
        {
            if(channelBarWindow != null)
                channelBarWindow.RemoveElements();
            base.OnRemovedFromScene();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            channelBarWindow = new ChannelBarWindow("", ConstantValues.skin);
            (Scene as MainScene).UICanvas.Stage.AddElement(channelBarWindow);
        }
    }
}
