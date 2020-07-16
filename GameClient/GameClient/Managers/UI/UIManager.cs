using Client.Managers;
using GameClient.Types.Item;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using System;

namespace GameClient.Managers.UI
{
    class UIManager
    {
        public static Window GenerateInventory(Skin skin, Stage stage)
        {
            Window window = new Window("Inventory", skin).SetMovable(true).SetResizable(true);
            window.SetResizeBorderSize(20);

            int count = 0;
            foreach (var i in LoginManagerClient.GetCharacter().Inventory)
            {
                if (count % 4 == 0)
                    window.Row();

                if (i != null)
                {
                    var textButtonStyle = new ImageButtonStyle
                    {
                        Up = new PrimitiveDrawable(Color.DimGray, 6, 2),
                        Over = new PrimitiveDrawable(Color.DimGray),
                        Down = new PrimitiveDrawable(Color.DimGray),
                        PressedOffsetX = 0,
                        PressedOffsetY = 0
                    };

                    ImageButton imButton = new ImageButton(textButtonStyle);
                    imButton.Add(new Label(i.Name)).Fill().Expand().Left().SetAlign(Align.BottomLeft);
                    imButton.SetTouchable(Touchable.Enabled);
                    imButton.OnHovered += delegate { OnHovered(imButton, i, skin, stage); };
                    imButton.OnExited += delegate { OnExit(imButton, stage); };
                    imButton.OnMoved += delegate { OnMovedAndHovered(imButton, stage); };
                    window.Add(imButton).Pad(4).Fill().Expand();
                }
                count++;
            }
            window.Pack();
            window.DebugAll();
                
            return window;
        }

        private static void OnMovedAndHovered(ImageButton obj, Stage stage)
        {
            if (obj.HoverWindow != null)
            {
                Vector2 tempPos = CalculatePosition(obj.HoverWindow);

                obj.HoverWindow.SetBounds(tempPos.X, tempPos.Y, obj.HoverWindow.GetWidth(), obj.HoverWindow.GetHeight());
            }
        }

        private static void OnExit(ImageButton obj, Stage stage)
        {
            if (obj.HoverWindow != null)
            {
                obj.HoverWindow.SetVisible(false);
                obj.HoverWindow.SetIsVisible(false);
                obj.HoverWindow.Remove();
            }
        }

        private static void OnHovered(ImageButton obj, WeaponItem item, Skin skin, Stage stage)
        {

            Window window = new Window("Inventory", skin);
            window.GetTitleLabel().SetVisible(false);
            window.GetTitleLabel().SetIsVisible(false);

            window.Add(new Label(item.Name, skin).SetFontScale(1.5f)).Expand().SetRow();
            window.Add(new Label("Attributes", skin)).SetRow();
            window.Add(new Label("Intelligence:" + item.Intelligence.ToString(), skin)).Fill().Expand().SetRow();
            window.Add(new Label("Strength:" + item.Strength.ToString(), skin)).Fill().Expand().SetRow();
            window.Add(new Label("Dexterity:" + item.Dexterity.ToString(), skin)).Fill().Expand().SetRow();

            window.Add(new Label("Reqs", skin)).SetRow();
            window.Add(new Label("Intelligence:" + item.IntelligenceReq.ToString(), skin)).Fill().Expand().SetRow();
            window.Add(new Label("Strength:" + item.StrengthReq.ToString(), skin)).Fill().Expand().SetRow();
            window.Add(new Label("Dexterity:" + item.DexterityReq.ToString(), skin)).Fill().Expand().SetRow();

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
            return  new Vector2(Input.ScaledMousePosition.X, Input.ScaledMousePosition.Y - 30 - window.GetHeight());
        }
    }
}
