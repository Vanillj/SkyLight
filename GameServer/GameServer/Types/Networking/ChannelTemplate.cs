using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Networking
{
    class ChannelTemplate
    {
        public string AbilityName { get; set; }
        public ChannelType ChannelType { get; set; }

    }

    public enum ChannelType {
        Ability
    }
}
