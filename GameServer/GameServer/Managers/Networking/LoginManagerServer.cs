﻿using Newtonsoft.Json.Converters;
using Nez;
using Server.Managers;
using Server.Types;

namespace Client.Managers
{
    class LoginManagerServer : LoginManagerHead
    {
        private CharacterPlayer AccountCharacter;
        private Entity CharacterEntity;

        public LoginManagerServer(string username, string password) : base(username, password)
        {}

        /** True if logged in and  **/
        public bool SetupLogin()
        {
            string[] st = SQLManager.GetDataFromSQL(username);
            string hashed = st[1];
            bool testP = CryptoManager.CheckHash(password, hashed);
            if (testP)
            {
                AccountCharacter = Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterPlayer>(st[0], new StringEnumConverter());
                return true;
            }
            return false;
        }

        public void SetCharacter(CharacterPlayer character)
        {
            AccountCharacter = character;
        }

        public CharacterPlayer GetCharacter()
        {
            return AccountCharacter;
        }

        public override int GetHashCode()
        {
            return GetUniqueID().GetHashCode();
        }

        public void SetEntity(Entity entity)
        {
            CharacterEntity = entity;
        }
        public Entity GetEntity()
        {
            return CharacterEntity;
        }
    }
}
