using GameClient.Scenes;
using GameClient.UI.Elements;
using GameServer.General;
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

        public ChannelBarComponent(int startTime)
        {
            //SetText(startTime.ToString());
            StartTime = startTime;
            
        }

        public override void Update()
        {
            TotalTime += Time.DeltaTime;
            double diff = Math.Round(StartTime - TotalTime, 2);
            if (diff <= 0)
            {
                if (this != null)
                {
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
