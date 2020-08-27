using GameClient.General;
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
    class AbilityBarWindow : Window
    {
        public AbilityBarWindow(string title, WindowStyle style) : base(title, style)
        {
            SetMovable(false);
            SetResizable(false);

            AddAbilities();

            SetHeight(50);
            SetPosition(Screen.Width / 2 - GetWidth(), Screen.Height - GetHeight());
        }

        private void AddAbilities()
        {
            var textButtonStyle = new ImageButtonStyle
            {
                Up = new PrimitiveDrawable(Color.DarkGray, 6, 2),
                Over = new PrimitiveDrawable(Color.DarkGray),
                Down = new PrimitiveDrawable(Color.DarkGray),
                PressedOffsetX = 0,
                PressedOffsetY = 0
            };

            foreach (var keybinds in KeyBindContainer.KeyBinds)
            {
                ImageButton button = new ImageButton(textButtonStyle);
                button.SetTouchable(Touchable.Enabled);
                button.Add(new Label(keybinds.BindedKey.ToString()).SetPosition(24/2, 24/2));
                button.Row();
                button.Add(new Label(keybinds.GetAbility().AbilityName).SetPosition(24 / 2, 24));
                Add(button).Size(24, 24).Pad(5);
            }

        }

        public AbilityBarWindow(Skin skin, string title = "", string styleName = null) : base(title, skin, styleName)
        {
            SetMovable(false);
            SetResizable(false);

            AddAbilities();

            SetHeight(50);
            SetPosition(Screen.Width / 2 - GetWidth(), Screen.Height - GetHeight());
        }
    }
}
