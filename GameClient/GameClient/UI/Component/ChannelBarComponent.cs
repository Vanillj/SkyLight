using Nez;

namespace GameClient.UI.Component
{
    class ChannelBarComponent : TextComponent, IUpdatable
    {
        private float TotalTime = 0;
        private float StartTime;

        public ChannelBarComponent(int startTime)
        {
            SetText(startTime.ToString());
            StartTime = startTime;
        }

        public void Update()
        {
            TotalTime += Time.DeltaTime;
            double diff = System.Math.Round(StartTime - TotalTime, 2);
            if (diff <= 0)
            {
                if(this != null)
                    this.RemoveComponent();
            }
            else
            {
                SetText(diff.ToString());
            }
        }
    }
}
