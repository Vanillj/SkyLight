using Client.Managers;
using GameClient.General;
using GameClient.Scenes;
using GameClient.Types.Item;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Managers.UI.Elements
{
    class CharacterWindow : Window
    {
        public CharacterWindow(Skin skin, Vector2 position, string title = "Character Information", float width = -1, float height = -1, string styleName = null) : base(title, skin, styleName)
        {
            SetResizeBorderSize(25);
            SetMovable(true);
            SetResizable(true);
            AddItems(skin);

            Pack();
            if (width != -1)
            {
                SetWidth(width);
            }
            if (height != -1)
            {
                SetHeight(height);
            }
            if (position == new Vector2(-1, -1))
            {
                SetPosition(Core.GraphicsDevice.Viewport.Width - GetWidth(), GetHeight() + 30);
            }
            else
            {
                SetPosition(position.X, position.Y);
            }
        }

        private void AddItems(Skin skin)
        {
            var textButtonStyle = new ImageButtonStyle
            {
                Up = new PrimitiveDrawable(Color.DarkGray, 6, 2),
                Over = new PrimitiveDrawable(Color.DimGray),
                Down = new PrimitiveDrawable(Color.DimGray),
                PressedOffsetX = 0,
                PressedOffsetY = 0
            };

            int count = 0;
            foreach (var i in LoginManagerClient.GetCharacter().Equipment)
            {
                float f = GetWidth();
                if (count % 5 == 0)
                    Row();
                if (i != null && i.GetSprite() == null)
                {
                    i.SetSprite(TextureContainer.GetSpriteAtlasByName("Items").GetSprite(i.TextureName));
                }

                if (i != null)
                {
                    SpriteDrawable spd = new SpriteDrawable(i.GetSprite());
                    NinePatchDrawable ninePatchDrawable = new NinePatchDrawable(i.GetSprite(), 0, 0, 0, 0) { MinHeight = 48, MinWidth = 48 };

                    ItemButton imButton = new ItemButton(ninePatchDrawable, count, i);

                    imButton.SetTouchable(Touchable.Enabled);
                    imButton.OnHovered += delegate { OnHovered(imButton, i, skin, stage); };
                    imButton.OnExited += delegate { OnExit(imButton, stage); };
                    imButton.OnMoved += delegate { OnMovedAndHovered(imButton); };
                    imButton.OnClicked += delegate { OnClickedCharacter(imButton); };
                    Add(imButton).Size(48, 48).Pad(4).Expand();
                }
                else
                {
                    ItemButton imButton = new ItemButton(textButtonStyle, count, null);
                    imButton.Add(new Label("")).Expand().Left().SetAlign(Nez.UI.Align.BottomLeft);
                    imButton.SetTouchable(Touchable.Disabled);
                    Add(imButton).Size(48, 48).Pad(4).Expand();
                }
                count++;
            }
        }

        public static bool RemoveCharacterWindow(Scene scene)
        {
            bool found = false;
            List<CharacterWindow> l = (scene as MainScene).UICanvas.Stage.FindAllElementsOfType<CharacterWindow>();
            if (l.Count > 0)
                found = true;
            foreach (var i in l)
                i.Remove();
            return found;
        }

        #region events

        private static void OnMovedAndHovered(ItemButton obj)
        {
            if (obj.HoverWindow != null)
            {
                Vector2 tempPos = CalculatePosition(obj.HoverWindow);

                obj.HoverWindow.SetBounds(tempPos.X, tempPos.Y, obj.HoverWindow.GetWidth(), obj.HoverWindow.GetHeight());
            }
        }
        private static void OnExit(ItemButton obj, Stage stage)
        {
            if (obj.HoverWindow != null)
            {
                obj.HoverWindow.SetVisible(false);
                obj.HoverWindow.SetIsVisible(false);
                obj.HoverWindow.Remove();
            }
            List<ItemHoverWindow> windows = stage.FindAllElementsOfType<ItemHoverWindow>();
            foreach (var window in windows)
            {
                window.Remove();
            }
        }

        private static void OnHovered(ItemButton obj, WeaponItem item, Skin skin, Stage stage)
        {
            ItemHoverWindow hoverWindow = new ItemHoverWindow("HoverWindow", skin, item);
            Vector2 tempPos = CalculatePosition(hoverWindow);
            hoverWindow.SetPosition(tempPos.X, tempPos.Y);

            stage.AddElement(hoverWindow);
            obj.HoverWindow = hoverWindow;
        }

        private static void OnClickedCharacter(ItemButton obj)
        {
            if (Input.LeftMouseButtonReleased)
            {
                if (obj.position != -1)
                {
                    MessageTemplate template = new MessageTemplate(obj.position.ToString(), MessageType.UnEquipItem);
                    MessageManager.AddToQueue(template);
                    obj.HoverWindow.Remove();
                }

                if (obj.GetParent() is Window)
                {

                }
            }
        }

        #endregion

        private static Vector2 CalculatePosition(Window window)
        {
            return new Vector2(Input.ScaledMousePosition.X, Input.ScaledMousePosition.Y - 30 - window.GetHeight());
        }

    }
}
