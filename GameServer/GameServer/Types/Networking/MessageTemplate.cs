using Newtonsoft.Json.Converters;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Managers
{
    class MessageTemplate
    {
        public MessageType MessageType;
        public string JsonMessage;

        public MessageTemplate(string JsonMessage, MessageType MessageType)
        {
            this.JsonMessage = JsonMessage;
            this.MessageType = MessageType;
        }

        public string TemplateToJson()
        {
            return MessageTemplate.TemplateToJson(this);
        }

        public static string TemplateToJson(MessageTemplate obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, new StringEnumConverter());
        }

        public static MessageTemplate JsonToTemplate(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MessageTemplate>(str, new StringEnumConverter());
        }

        internal static string TemplateToJsonList(List<MessageTemplate> queueList)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(queueList);
        }
    }
}
