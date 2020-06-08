using Client.Managers;
using GameClient.Managers;
using Nez.UI;
using Server.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Scenes
{
    class MainScene : BaseScene
    {
        public override Table Table { get; set; }
        //public override MessageManager MessageManager { get; set; }
        public InputManager InputManager { get; set; }

        public override void Initialize()
        {
            base.Initialize();
            Table.Row();
            Table.Add(new Label("Logged in!").SetFontScale(3));
        }
        public override void Update()
        {
            base.Update();
            if (InputManager != null)
                MessageManager.inputManager.CheckForInput();
        }
    }
}
