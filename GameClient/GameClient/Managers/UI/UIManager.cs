using Client.Managers;
using GameClient.Types.Item;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;

namespace GameClient.Managers.UI
{
    class UIManager
    {
        public static Window GenerateInventory(Skin skin, Stage stage)
        {
            Window window = new Window("Inventory", skin).SetMovable(true).SetResizable(true);
            
            int count = 0;
            foreach (var i in LoginManagerClient.GetCharacter().Inventory)
            {
                if (count % 4 == 0)
                    window.Row();

                if (i != null)
                {
                    TextButton l = new TextButton(i.Name, skin);
                    l.SetTouchable(Touchable.Enabled);
                    l.OnHovered += delegate { OnHovered(l, i, skin, stage); };
                    l.OnExited += delegate { OnExit(l, stage); };
                    l.OnMoved += delegate { OnMovedAndHovered(l, stage); };
                    window.Add(l).Pad(10);
                }
                count++;
            }
            window.Pack();
            window.DebugAll();

            return window;
        }

        private static void OnMovedAndHovered(TextButton obj, Stage stage)
        {
            if (obj.HoverWindow != null)
            {
                Vector2 tempPos = CalculatePosition(obj.HoverWindow);
                obj.HoverWindow.MoveBy(obj.HoverWindow.GetX() - tempPos.X, obj.HoverWindow.GetY() - tempPos.Y);
            }
        }

        private static void OnExit(TextButton obj, Stage stage)
        {
            if (obj.HoverWindow != null)
            {
                obj.HoverWindow.SetVisible(false);
                obj.HoverWindow.SetIsVisible(false);
                obj.HoverWindow.Remove();
            }
        }

        private static void OnHovered(TextButton obj, WeaponItem item, Skin skin, Stage stage)
        {
            obj.GetLabel().SetFontColor(Color.Red);

            Window window = new Window(item.Name, skin);
            window.Add(new Label("Attributes", skin)).SetRow();
            window.Add(new Label("Int:" + item.Intelligence.ToString(), skin)).Fill().Expand().SetRow();
            window.Add(new Label("Str:" + item.Strength.ToString(), skin)).Fill().Expand().SetRow();

            window.Add(new Label("Reqs", skin)).SetRow();
            window.Add(new Label("Int:" + item.IntelligenceReq.ToString(), skin)).Fill().Expand().SetRow();
            window.Add(new Label("Str:" + item.StrengthReq.ToString(), skin)).Fill().Expand().SetRow();

            Vector2 tempPos = CalculatePosition(window);
            window.SetPosition(tempPos.X, tempPos.Y);
            stage.AddElement(window);
            obj.HoverWindow = window;
            obj.HoverWindow.SetVisible(true);
            obj.HoverWindow.SetIsVisible(true);
        }


        //HELP METHODS
        private static Vector2 CalculatePosition(Window window)
        {
            return Input.RawMousePosition.ToVector2() + new Vector2(0, -30 - window.GetHeight());
        }
    }
}
