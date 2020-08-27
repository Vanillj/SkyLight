using Client.Managers;
using GameServer.General;
using Nez;
using Nez.UI;
using Server.Managers;

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
            ConstantValues.SetSkin();
            SetupScene();
        }

        public void SetupScene()
        {
            AddRenderer(new DefaultRenderer());
            var UICanvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());
            Table = UICanvas.Stage.AddElement(new Table());
            Table.SetFillParent(true).Top().PadLeft(10).PadTop(50);

        }

        public override void Update()
        {
            base.Update();
        }

    }
}
