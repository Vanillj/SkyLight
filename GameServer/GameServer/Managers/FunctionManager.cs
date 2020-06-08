using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server.Managers
{
    [Serializable()]
    class FunctionManager
    {
        public string typeName;
        public string methodName;
        public List<object> args = new List<object>();

        //private static string response = State.False.ToString();

        public FunctionManager(string _typeName, string _methodName, List<object> _args)
        {
            typeName = _typeName;
            args = _args;
            methodName = _methodName;
        }

        public string CheckForMethod(Socket socket)
        {
            //check for call of a method
            return CheckForCharacterMethods(socket);
        }

        private string CheckForCharacterMethods(Socket socket)
        {
            string response = State.False.ToString();
            Type type = Type.GetType(typeName.Replace("Client", "Server")); //converts from client to server functions
            if (type != null)
            {
                var method = type.GetMethod(methodName);
                if (method != null)
                {
                    method.Invoke(type, args.ToArray());
                    response = State.True.ToString();
                }

            }

            return response;

        }

        public static FunctionManager JsonToFunctionManager(string jsonString)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<FunctionManager>(jsonString);
        }

    }
}
