using GameClient.Types.Item;
using Microsoft.Xna.Framework;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Managers.UI.Elements
{
    class ItemHoverWindow : Window
    {
        public WeaponItem ButtonItem { get; }
        public ItemHoverWindow(string title, WindowStyle style, WeaponItem item) : base(title, style)
        {
            ButtonItem = item;
        }

        public ItemHoverWindow(string title, Skin skin, WeaponItem item, string styleName = null) : base(title, skin, styleName)
        {
            ButtonItem = item;
            GetTitleLabel().SetVisible(false);
            GetTitleLabel().SetIsVisible(false);
            GenerateInfomation(skin);
            SetResizable(false);
            SetMovable(false);
            SetVisible(true);
            SetIsVisible(true);
        }

        private void GenerateInfomation(Skin skin)
        {
            if (ButtonItem != null)
            {

                Add(new Label(ButtonItem.Name, skin).SetFontScale(1.5f)).Expand().SetRow();
                Add(new Label("Attributes", skin)).SetRow();
                Add(new Label("Intelligence:" + ButtonItem.Intelligence.ToString(), skin)).Fill().Expand().SetRow();
                Add(new Label("Strength:" + ButtonItem.Strength.ToString(), skin)).Fill().Expand().SetRow();
                Add(new Label("Dexterity:" + ButtonItem.Dexterity.ToString(), skin)).Fill().Expand().SetRow();

                Add(new Label("Reqs", skin)).SetRow();
                Add(new Label("Intelligence:" + ButtonItem.IntelligenceReq.ToString(), skin)).Fill().Expand().SetRow();
                Add(new Label("Strength:" + ButtonItem.StrengthReq.ToString(), skin)).Fill().Expand().SetRow();
                Add(new Label("Dexterity:" + ButtonItem.DexterityReq.ToString(), skin)).Fill().Expand().SetRow();
            }

        }
    }
}
