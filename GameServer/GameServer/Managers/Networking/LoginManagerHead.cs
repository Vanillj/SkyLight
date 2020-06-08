using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Server.Managers
{
    class LoginManagerHead
    {
        public string username { get; set; }
        public string password { get; set; }
        private long UniqueID;

        public LoginManagerHead(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public void SetUniqueID(long id)
        {
            UniqueID = id;
        }

        public long GetUniqueID()
        {
            return UniqueID;
        }

        //other stuff for comparison etc.
        public override int GetHashCode()
        {
            return UniqueID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            LoginManagerHead character = obj as LoginManagerHead;
            if (character == null)
                return false;
            else
            {
                if (character.GetUniqueID() == this.UniqueID)
                    return true;
                else
                    return false;
            }

        }

    }

}
