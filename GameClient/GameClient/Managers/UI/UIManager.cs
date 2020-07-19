using Client.Managers;
using GameClient.General;
using GameClient.Managers.UI.Elements;
using GameClient.Scenes;
using GameClient.Types.Item;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Server.Managers;
using Server.Types;
using System.Collections.Generic;

namespace GameClient.Managers.UI
{
    class UIManager
    {
        public static InventoryWindow GenerateInventoryWindow(Skin skin, Scene scene, Vector2 position, float width, float height)
        {
            Stage stage = (scene as MainScene).UICanvas.Stage;

            //removes window if it exists
            FindElementByStringAndRemove("Inventory", scene);

            //Creates new window after removing old one
            InventoryWindow inventoryWindow = new InventoryWindow(skin);
            inventoryWindow.SetMovable(true).SetResizable(true);
            inventoryWindow.SetResizeBorderSize(20);

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
                    inventoryWindow.Row();
                if (i != null && i.GetSprite() == null)
                {
                    i.SetSprite(TextureContainer.ItemAtlas.GetSprite(i.TextureName));
                }
                if (i != null)
                {
                    NinePatchDrawable ninePatchDrawable = new NinePatchDrawable(i.GetSprite(), 0, 0, 0, 0) { MinHeight = 48, MinWidth = 48 };
                    ItemButton imButton = new ItemButton(ninePatchDrawable){ item = i, position = count };
                    imButton.SetTouchable(Touchable.Enabled);
                    imButton.OnHovered += delegate { OnHovered(imButton, i, skin, stage); };
                    imButton.OnExited += delegate { OnExit(imButton, stage); };
                    imButton.OnMoved += delegate { OnMovedAndHovered(imButton); };
                    imButton.OnClicked += delegate { OnClickedInventory(imButton); };
                    inventoryWindow.Add(imButton).Size(48, 48).Pad(4).Expand();
                }
                else
                {
                    ItemButton imButton = new ItemButton(textButtonStyle, count, null);
                    imButton.Add(new Label("")).Expand().Left().SetAlign(Align.BottomLeft);
                    imButton.SetTouchable(Touchable.Disabled);
                    inventoryWindow.Add(imButton).Size(48, 48).Pad(4).Expand();
                }
                count++;
            }
            inventoryWindow.Pack();
            //window.DebugAll();
            if (width != -1)
            {
                inventoryWindow.SetWidth(width);
            }
            if (height != -1)
            {
                inventoryWindow.SetHeight(height);
            }
            if (position == new Vector2(-1, -1))
            {
                inventoryWindow.SetPosition(Core.GraphicsDevice.Viewport.Width - inventoryWindow.GetWidth(), Core.GraphicsDevice.Viewport.Height - inventoryWindow.GetHeight());
            }
            else
            {
                inventoryWindow.SetPosition(position.X, position.Y);
            }

            NinePatchDrawable drawable = new NinePatchDrawable(inventoryWindow.sprite, 0, 0, 0, 0) { MinHeight = inventoryWindow.MinWidth, MinWidth = inventoryWindow.MinHeight };
            inventoryWindow.SetBackground(drawable);
            stage.AddElement(inventoryWindow);

            return inventoryWindow;
        }

        public static Window GenerateCharacterWindow(Skin skin, Scene scene, Vector2 position, float width, float height)
        {
            Stage stage = (scene as MainScene).UICanvas.Stage;

            //removes window if it exists
            FindElementByStringAndRemove("Character Information", scene);

            //Creates new window after removing old one
            CharacterWindow window = new CharacterWindow(skin);
            window.SetResizeBorderSize(25);
            window.SetMovable(true).SetResizable(true);
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
                float f = window.GetWidth();
                if (count % 5 == 0)
                    window.Row();
                if (i != null && i.GetSprite() == null)
                {
                    i.SetSprite(TextureContainer.ItemAtlas.GetSprite(i.TextureName));
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
                    window.Add(imButton).Size(48, 48).Pad(4).Expand();
                }
                else
                {
                    ItemButton imButton = new ItemButton(textButtonStyle, count, null);
                    imButton.Add(new Label("")).Expand().Left().SetAlign(Align.BottomLeft);
                    imButton.SetTouchable(Touchable.Disabled);
                    window.Add(imButton).Size(48, 48).Pad(4).Expand();
                }
                count++;
            }
            window.Pack();
            if (width != -1)
            {
                window.SetWidth(width);
            }
            if (height != -1)
            {
                window.SetHeight(height);
            }
            if (position == new Vector2(-1, -1))
            {
                window.SetPosition(Core.GraphicsDevice.Viewport.Width - window.GetWidth(), window.GetHeight() + 30);
            }
            else
            {
                window.SetPosition(position.X, position.Y);
            }
            stage.AddElement(window);
            return window;
        }

        #region Mouse hover item

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
            if (Nez.Input.LeftMouseButtonReleased)
            {
                if (obj.position != -1)
                {
                    MessageTemplate template = new MessageTemplate(obj.position.ToString(), MessageType.EquipItem);
                    MessageManager.AddToQueue(template);

                }

                if (obj.GetParent() is Window)
                {

                }
            }
        }
        private static void OnClickedCharacter(ItemButton obj)
        {
            if (Nez.Input.LeftMouseButtonReleased)
            {
                if (obj.position != -1)
                {
                    MessageTemplate template = new MessageTemplate(obj.position.ToString(), MessageType.UnEquipItem);
                    MessageManager.AddToQueue(template);

                }

                if (obj.GetParent() is Window)
                {

                }
            }
        }

        #endregion

        //HELP METHODS
        private static Vector2 CalculatePosition(Window window)
        {
            return new Vector2(Nez.Input.ScaledMousePosition.X, Nez.Input.ScaledMousePosition.Y - 30 - window.GetHeight());
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

        public static void RemoveAllHoverWindows(Scene scene)
        {
            if (scene is MainScene)
            {
                List<ItemHoverWindow> ListOfItems = (scene as MainScene).UICanvas.Stage.FindAllElementsOfType<ItemHoverWindow>();
                foreach (var item in ListOfItems)
                {
                    item.Remove();
                }
            }
        }

        public static void FindElementByStringAndRemove(string str, Scene scene)
        {
            bool removed = false;
            List<Element> elements = null;
            elements = (scene as MainScene).UICanvas.Stage.GetElements().FindAll(i =>
            {
                if (i is Window)
                {
                    return (i as Window).GetTitleLabel().GetText().Equals(str);
                }
                return false;
            });
            elements.ForEach(i => { i.Remove(); removed = true; } );
            //foreach (var element in elements)
            //{
            //    if (element != null && element is Window)
            //    {
            //        foreach (var item in (element as Window).GetChildren())
            //        {
            //            if (item is ItemButton && (item as ItemButton).HoverWindow != null)
            //            {
            //                (item as ItemButton).HoverWindow.Remove();
            //                removed = true;
            //            }
            //        }
            //    }
            //    //}
            //    ////Removes every window if present
            //    //if (element != null)
            //    //    element.Remove();
            //}
        }
    }
}
