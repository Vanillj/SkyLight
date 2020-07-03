
using System.Runtime.CompilerServices;

namespace GameServer.General
{
    class StaticConstantValues
    {

        private static string _ConnectionID;
        public static string ConnectionID {
            get { return _ConnectionID; }
            set {
                if (_ConnectionID == null)
                    _ConnectionID = value;
            }
        }

        public static string _ConnectionPWS;
        public static string ConnectionPWS
        {
            get { return _ConnectionPWS; }
            set
            {
                if (_ConnectionPWS == null)
                    _ConnectionPWS = value;
            }
        }

        public const int EquipmentLength = 10;

    }


    //Help Class
    public class CredentialInfo
    {
        public string ID { get; set; }
        public string PSW { get; set; }
        public string ServerString { get; set; }
        public int Port { get; set; }
    }
}
