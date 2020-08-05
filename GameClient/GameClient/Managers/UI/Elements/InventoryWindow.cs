using Client.Managers;
using GameClient.General;
using GameClient.Scenes;
using GameClient.Types.Item;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Textures;
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
    class InventoryWindow : Window
    {
        public Sprite sprite;
        public InventoryWindow(string title, WindowStyle style) : base(title, style)
        {

        }

        public InventoryWindow(Skin skin, Vector2 position, float width = -1, float height = -1, string title = "Inventory", string styleName = null) : base(title, skin, styleName)
        {
            sprite = TextureContainer.UIAtlas.GetSprite("bg_01_02");
            SetMovable(true);
            SetResizable(true);
            SetResizeBorderSize(20);
            AddItems(skin);
            Pack();

            if (width != -1)
                SetWidth(width);
            if (height != -1)
                SetHeight(height);
            if (position == new Vector2(-1, -1))
                SetPosition(Core.GraphicsDevice.Viewport.Width - GetWidth(), Core.GraphicsDevice.Viewport.Height - GetHeight());
            else
                SetPosition(position.X, position.Y);
            NinePatchDrawable drawable = new NinePatchDrawable(sprite, 0, 0, 0, 0) { MinHeight = this.MinHeight, MinWidth = this.MinWidth };
            SetBackground(drawable);
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
            foreach (var i in LoginManagerClient.GetCharacter().Inventory)
            {
                if (count % 5 == 0)
                    Row();
                if (i != null && i.GetSprite() == null)
                {
                    i.SetSprite(TextureContainer.ItemAtlas.GetSprite(i.TextureName));
                }
                if (i != null)
                {
                    NinePatchDrawable ninePatchDrawable = new NinePatchDrawable(i.GetSprite(), 0, 0, 0, 0) { MinHeight = 48, MinWidth = 48 };
                    ItemButton imButton = new ItemButton(ninePatchDrawable) { item = i, position = count };
                    imButton.SetTouchable(Touchable.Enabled);
                    imButton.OnHovered += delegate { OnHovered(imButton, i, skin, stage); };
                    imButton.OnExited += delegate { OnExit(imButton, stage); };
                    imButton.OnMoved += delegate { OnMovedAndHovered(imButton); };
                    imButton.OnClicked += delegate { OnClickedInventory(imButton); };
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

        public static bool RemoveInventory(Scene scene)
        {
            bool found = false;
            List<InventoryWindow> l = (scene as MainScene).UICanvas.Stage.FindAllElementsOfType<InventoryWindow>();
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

        private static void OnClickedInventory(ItemButton obj)
        {
            if (Input.LeftMouseButtonReleased)
            {
                if (obj.position != -1)
                {
                    MessageTemplate template = new MessageTemplate(obj.position.ToString(), MessageType.EquipItem);
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

        public static Element FindElementByString(string str, Scene scene)
        {
            Element element = null;
            element = (scene as MainScene).UICanvas.Stage.GetElements().Find(i =>
            {
                if (i is Window)
                {
                    return (i as Window).GetTitleLabel().GetText().Equals(str);
                }
                return false;
            });

            return element;
        }
    }
}
