using GameClient.Managers;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace Server.Scenes
{
    abstract class BaseScene : Scene
    {
        abstract public Table Table { get; set; }

        public static MessageManager messageManager { get; set; }
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
            //Table.AddElement(new TextField("", TextFieldStyle.Create(Color.White, Color.Red, Color.Black, Color.Green)));

        }

        public override void Update()
        {
            base.Update(); 
            if(messageManager != null)
            {
                MessageManager.CheckForMessage();
                MessageManager.SendQueue();
            }
        }

    }
}
