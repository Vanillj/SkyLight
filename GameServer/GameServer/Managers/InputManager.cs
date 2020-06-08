using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Managers
{
    class InputManager
    {
        ServerNetworkManager networkManager;
        public InputManager(ServerNetworkManager _networkManager)
        {
            networkManager = _networkManager;
        }

    }
}
