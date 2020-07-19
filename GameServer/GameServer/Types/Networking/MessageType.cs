using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Types
{
    enum MessageType
    {
        Login,
        Disconnected,
        Chat,
        Movement,
        Attack,
        InventoryInteraction,
        LoginSuccess,
        LoginFailure,
        Register,
        GameUpdate,
        EquipItem,
        UnEquipItem
    };
}
