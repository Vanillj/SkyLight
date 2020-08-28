using GameClient.UI.Component;
using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.UI.Elements
{
    class ChannelBarEntity : Window
    {
        private ChannelBarComponent CBar;
        private ProgressBar bar;

        public ChannelBarEntity(string title, Skin skin, string styleName = null) : base(title, skin, styleName)
        {

        }
    }
}
