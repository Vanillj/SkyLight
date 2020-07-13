using GameClient.Managers;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace Server.Scenes
{
    abstract class BaseScene : Scene
    {
        abstract public Table Table { get; set; }
        public UICanvas UICanvas;
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
            UICanvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());

            Table = UICanvas.Stage.AddElement(new Table());
            Table.SetFillParent(true).Top();

        }

        public override void Update()
        {
            base.Update(); 
        }
    }
}
