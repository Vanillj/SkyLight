using Client.Managers;
using GameServer.Managers.Networking;
using Nez;
using Nez.Farseer;
using Server.Managers;
using Server.Types;
using System.Collections.Generic;

namespace GameServer.Types.Components
{
    class PlayerComponent : Component, IUpdatable
    {
        LoginManagerServer loginManager;

        public PlayerComponent(LoginManagerServer loginManager)
        {
            this.loginManager = loginManager;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();

        }

        float timeSpan = 0;
        public void Update()
        {
            //checks if client is still connected
            CheckIfConnected();

            timeSpan += Time.DeltaTime;
            //Updates every 1/20 second
            if (timeSpan > 0.05)
            {
                HashSet<LoginManagerServer> characterlist = CharacterManager.GetLoginManagerServerList();

                if (characterlist != null && characterlist.Count > 0 && loginManager != null)
                {
                    CharacterPlayer tempc = loginManager.GetCharacter();

                    //sets the position of the entity's physical body as the physical position
                    FSRigidBody v = Core.Scene.FindEntity(tempc._name).GetComponent<FSRigidBody>();
                    
                    tempc.physicalPosition = v.Transform.Position;

                    DataTemplate dataTemplate = new DataTemplate { 
                        RecieverCharacter = tempc, 
                        OthersCharacters = FillRecieverList(characterlist) 
                    };


                    string posString = Newtonsoft.Json.JsonConvert.SerializeObject(dataTemplate);
                    
                    MessageManager.SendStringToUniqueID(posString, loginManager.GetUniqueID(), MessageType.GameUpdate);
                }
            }

        }

        private List<CharacterPlayer> FillRecieverList(HashSet<LoginManagerServer> characterlist)
        {
            List<CharacterPlayer> temp = new List<CharacterPlayer>();
            foreach (LoginManagerServer l in characterlist)
            {
                CharacterPlayer tempC = l.GetCharacter();

                double distance = tempC._pos.Length();

                //TODO: distance should depend on settings or screen resolution
                if (!loginManager.GetUniqueID().Equals(l.GetUniqueID()) && distance < 2000)
                {
                    temp.Add(tempC);
                }
            }
            return temp;
        }

        private void CheckIfConnected()
        {
            if (ServerNetworkSceneComponent.GetNetServer().Connections.Find(c => c.RemoteUniqueIdentifier.Equals(loginManager.GetUniqueID())) == null)
            {
                HashSet<LoginManagerServer> characterlist = CharacterManager.GetLoginManagerServerList();
                characterlist.Remove(loginManager);
                Entity.Destroy();
            }
        }
    }
}
