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
            var skin = Skin.CreateDefaultSkin();
            Table = UICanvas.Stage.AddElement(new Window("test", skin).SetMovable(true).SetResizable(true));
            Table.SetFillParent(true).Top();

        }

        public override void Update()
        {
            base.Update(); 
        }
    }
}
