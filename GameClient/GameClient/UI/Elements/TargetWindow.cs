using GameClient.Scenes;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Managers.UI.Elements
{
    class TargetWindow : Window
    {
        public string TargetWindowName { get; set; }
        public Label TargetLabel { get; }

        public TargetWindow(string title, WindowStyle style) : base(title, style)
        {
        }

        public TargetWindow(Entity entity, Vector2 position, Skin skin, string styleName = null, string title = "") : base(title, skin, styleName)
        {
            int scale = 3;
            TargetLabel = new Label("Target: " + entity.Name).SetFontScale(scale).SetFontColor(Color.Red);

            SetWidth(TargetLabel.MinWidth + 20);
            
            SetHeight(TargetLabel.MinHeight + 10);
            TargetLabel.SetEllipsis(true);
            SetMovable(true);
            SetPosition(position.X - GetWidth(), position.Y - GetHeight());
            Add(TargetLabel);
        }

        public void SetTarget(string name)
        {
            TargetLabel.SetText("Target: " + name);
        }

        public static bool RemoveTargetWindow(Scene scene)
        {
            bool found = false;
            List<TargetWindow> l = (scene as MainScene).UICanvas.Stage.FindAllElementsOfType<TargetWindow>();
            if (l.Count > 0)
                found = true;
            foreach (var i in l)
                i.Remove();
            return found;
        }
    }
}
