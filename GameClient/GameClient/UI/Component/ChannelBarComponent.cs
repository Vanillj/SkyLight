using GameClient.Scenes;
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
        private TextComponent HealthtextComponent;
        private ProgressBar bar;

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
                    bar.Remove();
                    Scene.RemoveSceneComponent<ChannelBarComponent>();
                }
            }
            else
            {
                bar.SetValue((TotalTime / StartTime));
            }
            base.Update();
        }

        public override void OnRemovedFromScene()
        {
            if(bar != null)
                bar.Remove();
            base.OnRemovedFromScene();
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            bar = new ProgressBar(0, 1, 0.01f, false, ConstantValues.skin);
            bar.SetPosition(Screen.Width / 2 - 0.75f*bar.PreferredWidth / 2, (Screen.Height + 0.2f * Screen.Height) / 2 - bar.GetHeight() / 2);
            (Scene as MainScene).UICanvas.Stage.AddElement(bar);
        }
    }
}
