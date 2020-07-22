using GameServer.General;
using System;
using System.Diagnostics;
using Npgsql;
using GameServer;
using Lidgren.Network;
using Client.Managers;

namespace Server.Managers
{
    class SQLManager
    {

        private static string connectionString; 
        private static NpgsqlConnection cn;

        public static void SetUpSQL()
        {
            connectionString = "Server=balarama.db.elephantsql.com; Port=5432;User Id=" + ConstantValues.ConnectionID + "; Password=" + ConstantValues.ConnectionCredential + "; Database=hxhyscft;";
            try
            {
                cn = new NpgsqlConnection(connectionString);
                cn.Open();
                Debug.WriteLine("Connected to database.");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to connect to database. " + e.Message);
            }
        }

        public static void SaveAllToSQL()
        {
            foreach (NetConnection Connection in ServerNetworkSceneComponent.GetNetServer().Connections)
            {
                LoginManagerServer user = MapContainer.GetLoginByID(Connection.RemoteUniqueIdentifier);
                if (user != null && user.GetCharacter() != null)
                {
                    UpdateToSQL(user.username, user.GetCharacter().CreateJsonFromCharacter());
                }
            }
        }

        public static void CloseSQL()
        {
            SaveAllToSQL();
            cn.Close();
        }

        public static void UpdateToSQL(string username, string data)
        {

            var updateSQL = "UPDATE Users SET accountData = @data WHERE accountInfo = @usr";

            using (var cmd = new NpgsqlCommand(updateSQL, cn))
            {
                cmd.Parameters.AddWithValue("@data", data);
                cmd.Parameters.AddWithValue("@usr", username);
                string tmp = cmd.CommandText.ToString();

                Debug.WriteLine(tmp);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }

        }

        public static void AddToSQL(string username, string password, string data)
        {
            //if username already in database
            if (CheckIfExistInSQL(username))
            {
                return;
            }

            var InserSQL = "INSERT INTO Users(accountInfo, accountData, accountInfoSecond) VALUES(@accountInfo, @accountData, @accountInfoSecond)";

            using (var cmd = new NpgsqlCommand(InserSQL, cn))
            {
                cmd.Parameters.AddWithValue("@accountInfo", username);
                cmd.Parameters.AddWithValue("@accountData", data);
                cmd.Parameters.AddWithValue("@accountInfoSecond", password);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Added \"" + username + " : " + data + "\" to Database");
            }
        }

        //TODO: Create SQL with parameters
        internal static string[] GetDataFromSQL(string username)
        {
            string[] returner = { "", "" };

            var sqlGet = "SELECT * FROM Users WHERE accountInfo = @usr";

            using (var cmd = new NpgsqlCommand(sqlGet, cn))
            {
                cmd.Parameters.AddWithValue("@usr", username);
                cmd.Prepare();

                using (NpgsqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        returner[0] = rd["accountData"].ToString();
                        returner[1] = rd["accountInfoSecond"].ToString();
                    }
                }
            }
            return returner;
        }

        public static bool CheckIfExistInSQL(string username)
        {
            //TODO: Proper sql request
            var sqlCheck = "SELECT COUNT(1) FROM Users WHERE accountInfo = @user";

            using (var cmd = new NpgsqlCommand(sqlCheck, cn))
            {
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Prepare();
                var c = Convert.ToInt32(cmd.ExecuteScalar());
                if (c > 0)
                {
                    return true;
                }

            }

            return false;
        }

    }
}
