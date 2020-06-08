﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Server.Managers;

namespace Server.Scenes
{
    abstract class BaseScene : Scene
    {
        abstract public Table Table { get; set; }

        abstract public MessageManager MessageManager { get; set; }
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

        public override void Update()
        {
            MessageManager.CheckForMessageAvailable();
            base.Update();
        }

    }
}
