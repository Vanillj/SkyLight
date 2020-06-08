﻿using BCrypt.Net;
using MySql.Data.MySqlClient;
using Server.Managers;
using Server.Types;
using System;
using System.Dynamic;

namespace Client.Managers
{
    class LoginManagerServer : LoginManagerHead
    {

        public Character AccountCharacter;

        public LoginManagerServer(string username, string password) : base(username, password)
        {}

        public bool SetupLogin()
        {
            string[] st = SQLManager.GetDataFromSQL(username);
            string hashed = st[1];
            bool testP = CryptoManager.CheckHash(password, hashed);
            if (testP)
            {
                AccountCharacter = Newtonsoft.Json.JsonConvert.DeserializeObject<Character>(st[0]);
            }
            return true;
        }

        public void SetCharacter(Character character)
        {
            AccountCharacter = character;
        }

        public Character GetCharacter()
        {
            return AccountCharacter;
        }

        public override int GetHashCode()
        {
            return GetUniqueID().GetHashCode();
        }
    }
}
