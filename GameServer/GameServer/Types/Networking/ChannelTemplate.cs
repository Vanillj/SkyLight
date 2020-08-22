using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types.Networking
{
    class ChannelTemplate
    {
        public string ChannelName { get; set; }
        public ChannelType ChannelType { get; set; }
        public ChannelTemplate(string ChannelName, ChannelType ChannelType)
        {
            this.ChannelName = ChannelName;
            this.ChannelType = ChannelType;
        }
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }

    public enum ChannelType {
        Ability
    }
}
