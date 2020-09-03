using GameClient.UI.Component;
using GameServer.General;
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
    class ChannelBarWindow : Window
    {
        public ProgressBar bar;
        public Label tc;

        public ChannelBarWindow(string title, Skin skin, string styleName = null) : base(title, skin, styleName)
        {
            SetTouchable(Touchable.Disabled);
            bar = new ProgressBar(0, 1, 0.02f, false, ConstantValues.skin);
            tc = new Label("10000", skin);
            tc.SetText("10000000000");
            SetWidth(bar.GetWidth() + tc.PreferredWidth + 50);
            SetHeight(bar.GetHeight() + tc.PreferredHeight + 5);
            Add(bar).SetAlign(Nez.UI.Align.Left).Pad(5).SetRow();
            Row();
            Add(tc).SetAlign(Nez.UI.Align.Right).Pad(5);

            SetPosition(Screen.Width / 2 - GetWidth() / 2, Screen.Height - 0.2f * Screen.Height - GetHeight() / 2);
        }

        public void UpdateBar(float deltaVal, float TimeLeft)
        {
            bar.SetValue(deltaVal);
            tc.SetText(TimeLeft.ToString());
        }

        public void RemoveElements()
        {
            bar.Remove();
            Remove();
        }
    }
}
