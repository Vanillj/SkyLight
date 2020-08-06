using GameClient.Managers;
using GameClient.Types.KeyBinding;
using GameServer.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.General
{
    class KeyBindContainer
    {
        public static List<KeyBind> KeyBinds;
        public static void SetKeyBinds()
        {
            KeyBinds = FileManager.GetKeyBinds("Data/" + ConstantValues.KeyBindName);
        }
    }
}
