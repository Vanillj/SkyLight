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
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameClient.Managers.UI
{
    class UIManager
    {
        public static InventoryWindow GenerateInventoryWindow(Skin skin, Scene scene, Vector2 position, float width, float height)
        {
            Stage stage = (scene as MainScene).UICanvas.Stage;

            //removes window if it exists
            InventoryWindow.RemoveInventory(scene);

            //Creates new window after removing old one
            InventoryWindow inventoryWindow = new InventoryWindow(skin, position, width, height);
            stage.AddElement(inventoryWindow);
            return inventoryWindow;
        }

        public static Window GenerateCharacterWindow(Skin skin, Scene scene, Vector2 position, float width, float height)
        {
            Stage stage = (scene as MainScene).UICanvas.Stage;

            //removes window if it exists
            CharacterWindow.RemoveCharacterWindow(scene);

            //Creates new window after removing old one
            CharacterWindow window = new CharacterWindow(skin, position);
            stage.AddElement(window);
            return window;
        }
    }
}
