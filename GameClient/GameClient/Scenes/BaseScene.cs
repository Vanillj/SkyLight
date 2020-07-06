using GameClient.Managers;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace Server.Scenes
{
    abstract class BaseScene : Scene
    {
        abstract public Table Table { get; set; }

        public BaseScene()
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            SetupScene();
        }

        public void SetupScene()
        {
            
            AddRenderer(new DefaultRenderer());
            var UICanvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());
            Table = UICanvas.Stage.AddElement(new Table());
            Table.SetFillParent(true).Top().PadLeft(10).PadTop(50);

        }

        float timeSpan = 0;
        public override void Update()
        {
            /*timeSpan += Time.DeltaTime;
            if (messageManager != null)
            {
                MessageManager.CheckForMessage();
                //delay
                if (timeSpan > 0.033)
                {
                    MessageManager.SendQueue();
                    timeSpan = 0;
                }
            }*/
            base.Update(); 
            
        }

    }
}
