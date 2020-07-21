using Client.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Types
{
    class MapLayer
    {
        public int LayerID { get; }
        public HashSet<LoginManagerServer> LayerLogins = new HashSet<LoginManagerServer>();

        public MapLayer(int LayerID)
        {
            this.LayerID = LayerID;
        }

        public void Update()
        {

        }

        public void AddLoginToLayer(LoginManagerServer login)
        {
            LayerLogins.Add(login);
        }

        public void MoveLoginToLayer(LoginManagerServer login, MapLayer newLayer)
        {
            if (newLayer != null)
            {
                LayerLogins.Remove(login);
                newLayer.AddLoginToLayer(login);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is MapLayer && (obj as MapLayer).LayerID.Equals(LayerID))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
