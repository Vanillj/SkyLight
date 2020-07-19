using GameClient.General;
using Nez.Textures;
using Nez.UI;
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

        public InventoryWindow(Skin skin, string title = "Inventory", string styleName = null) : base(title, skin, styleName)
        {
            sprite = TextureContainer.UIAtlas.GetSprite("bg_01_02");
        }
    }
}
