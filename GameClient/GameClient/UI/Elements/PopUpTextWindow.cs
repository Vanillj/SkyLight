using FarseerPhysics.Dynamics;
using GameClient.Scenes;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.UI.Elements
{
    class PopUpTextWindow : Window
    {
        MainScene scene;
        public PopUpTextWindow(MainScene scene, string text, string title, Skin skin, string styleName = null) : base(title, skin, styleName)
        {
            //SetDebug(true);
            SetTouchable(Touchable.Enabled);
            SetResizable(false);
            SetMovable(true);
            SetPosition(Screen.Center.X, Screen.Center.Y);
            
            Label textLabel = new Label(text);
            textLabel.SetAlignment(Nez.UI.Align.TopLeft);
            Add(textLabel).Pad(5).SetAlign(Nez.UI.Align.TopLeft);
            SetWidth(textLabel.PreferredWidth + 25);
            SetHeight(textLabel.PreferredHeight + 25);
            scene.AddSceneComponent(new PupUpTextWindowComponent(this));
            this.scene = scene;
        }

        public void Update()
        {
            Vector2 wPoint = scene.Camera.ScreenToWorldPoint(Input.MousePosition);
            //Console.WriteLine("Mouse: " + Input.MousePosition.ToPoint().ToString()  + " : x " + GetX() + ", t " + GetY() + ", w " + GetWidth() + ", h " + GetHeight());

            //Vector2 wP = stage.Camera.ScreenToWorldPoint(Input.MousePosition.ToPoint());
            if (scene.UICanvas.Stage.Hit(Input.MousePosition) == this && Input.LeftMouseButtonDown)
            {
                Remove();
                scene.RemoveSceneComponent<PupUpTextWindowComponent>();
            }
        }
    }

    class PupUpTextWindowComponent : SceneComponent
    {
        PopUpTextWindow window;
        public PupUpTextWindowComponent(PopUpTextWindow window)
        {
            this.window = window;
        }
        public override void Update()
        {
            window.Update();
            /*Vector2 wP = Scene.Camera.ScreenToWorldPoint(Input.MousePosition.ToPoint());
            if (window.Hit(wP) == window)
            {
                window.Remove();
            }*/
            base.Update();
        }
    }
}
