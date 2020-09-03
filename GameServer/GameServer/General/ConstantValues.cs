
using Nez.UI;
using System.Runtime.CompilerServices;

namespace GameServer.General
{
    class ConstantValues
    {
        //Constants
        public const int EquipmentLength = 10;
        public const int BaseInventoryLength = 30;
        public const int MaxConnectionsToLayer = 20;

        public const float UpdateFrequency = 1f/30;

        public const string AbilityDataFileName = "AbilityData.json";
        public const string ItemDataFileName = "ItemData.json";
        public const string MapDataFileName = "MapData.json";
        public const string ClassDataFileName = "";
        public const string TextureAtlasDataFileName = "TextureAtlasData.json";
        public const string DefaultMap = "map";
        public const string KeyBindName = "KeyBindData.json";

        public static Skin skin;

        //Variables that need to be set
        private static string _ConnectionID;
        public static string ConnectionID {
            get { return _ConnectionID; }
            set {
                if (_ConnectionID == null)
                    _ConnectionID = value;
            }
        }

        private static string _ConnectionCredential;
        public static string ConnectionCredential
        {
            get { return _ConnectionCredential; }
            set
            {
                if (_ConnectionCredential == null)
                    _ConnectionCredential = value;
            }
        }

        private static int? _Port;
        public static int? Port
        {
            get { return _Port; }
            set
            {
                if (_Port == null)
                    _Port = value;
            }
        }

        private static string _ServerString;
        public static string ServerString
        {
            get { return _ServerString; }
            set
            {
                if (_ServerString == null)
                    _ServerString = value;
            }
        }

        public static void SetSkin()
        {
            skin = Skin.CreateDefaultSkin();
        }

    }


    //Help Class
    public class CredentialInfo
    {
        public string ID { get; set; }
        public string ConnectionCredential { get; set; }
        public string ServerString { get; set; }
        public int Port { get; set; }
    }
}
