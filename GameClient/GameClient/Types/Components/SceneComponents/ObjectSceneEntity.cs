using GameClient.Scenes;
using GameClient.UI.Elements;
using GameServer.General;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.Components.SceneComponents
{
    class ObjectSceneEntity : Entity
    {
        public readonly float Width, Height;
        public readonly string Type, Name;
        private readonly TmxObject obj;
        private readonly int TileSize;

        public ObjectSceneEntity(TmxObject obj, int tileSize)
        {
            
            Width = obj.Width;
            Height = obj.Height;
            Type = obj.Type;
            Name = obj.Name;
            TileSize = tileSize;
            this.obj = obj;
            
        }

        public override void Update()
        {
            Rectangle r = new Rectangle((int)obj.X, (int)obj.Y, (int)Width, (int)Height);
            Vector2 wPoint = Scene.Camera.ScreenToWorldPoint(Input.MousePosition);
            
            if (r.Contains(wPoint) && Input.LeftMouseButtonDown)
            {
                //Do something with object
                switch (Type)
                {
                    case "Sign":
                        string val = "Error Loading Data.";
                        obj.Properties.TryGetValue("TextBox", out val);
                        if(val != "")
                        {
                            //only one window at a time.
                            (Scene as MainScene).UICanvas.Stage.FindAllElementsOfType<PopUpTextWindow>().ForEach(e => e.Remove());
                            //create new window
                            (Scene as MainScene).UICanvas.Stage.AddElement(new PopUpTextWindow(Scene as MainScene, val, Name, ConstantValues.skin));
                        }
                        break;
                }
            }
            base.Update();
        }
        public override void OnAddedToScene()
        {
            var text = new TextComponent().SetText(Name);
            
            AddComponent(text);
            this.SetPosition(new Vector2(obj.X + obj.Width / 2 - text.Width / 2, obj.Y - 10));
            base.OnAddedToScene();
        }
        public override void OnRemovedFromScene()
        {
            base.OnRemovedFromScene();
        }
    }
}
