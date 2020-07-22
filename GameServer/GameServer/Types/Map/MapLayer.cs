using Client.Managers;
using Nez;
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
        public long LayerID { get; }
        public HashSet<LoginManagerServer> LayerLogins = new HashSet<LoginManagerServer>();
        public long CreatorID { get; }

        float totalTime = 0;
        public bool ToDestroy = false;

        public MapLayer(long LayerID, long CreatorID)
        {
            this.LayerID = LayerID;
            this.CreatorID = CreatorID;
        }
        public MapLayer(long LayerID)
        {
            this.LayerID = LayerID;
        }

        public void Update()
        {
            totalTime += Time.DeltaTime;
            if (LayerLogins.Count > 0)
            {
                totalTime = 0;
            }

            //If no activity for 15 min, destroy layer
            if (totalTime > 15*60*1000)
            {
                ToDestroy = true;
            }
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

        public void RemoveLoginFromLayerID(long ID)
        {
            LayerLogins.RemoveWhere(l => l.GetUniqueID().Equals(ID));
        }

        public void RemoveLoginFromLayer(LoginManagerServer login)
        {
            RemoveLoginFromLayerID(login.GetUniqueID());
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
