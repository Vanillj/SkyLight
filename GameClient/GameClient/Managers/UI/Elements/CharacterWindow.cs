using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Managers.UI.Elements
{
    class CharacterWindow : Window
    {
        public CharacterWindow(string title, WindowStyle style) : base(title, style)
        {

        }

        public CharacterWindow(Skin skin, string title = "Character Information", string styleName = null) : base(title, skin, styleName)
        {

        }
    }
}
