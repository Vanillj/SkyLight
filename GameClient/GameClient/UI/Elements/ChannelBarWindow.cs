using Nez;
using Nez.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.UI.Elements
{
    class ChannelBarEntity : ProgressBar
    {
        public ChannelBarEntity(float min, float max, float stepSize, bool vertical, Skin skin, string styleName = null) : base(min, max, stepSize, vertical, skin, styleName)
        {

        }

    }
}
