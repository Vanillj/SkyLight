using Client.Managers;
using GameServer.General;
using GameServer.Managers.Networking;
using Nez;
using Nez.BitmapFonts;
using Nez.Farseer;
using Server.Managers;
using Server.Types;
using System.Collections.Generic;

namespace GameServer.Types.Components
{
    class PlayerComponent : Component, IUpdatable
    {
        LoginManagerServer loginManager;
        public MapLayer CurrentLayer;
        public bool isChanneling = false;
        public Entity Target;
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
            //CheckIfConnected();

            timeSpan += Time.DeltaTime;
            //Updates every 1/20 second
            if (timeSpan > 0.05)
            {
                if (CurrentLayer != null)
                {
                    HashSet<LoginManagerServer> characterlist = CurrentLayer.LayerLogins;

                    if (characterlist != null && characterlist.Count > 0 && loginManager != null)
                    {
                        CharacterPlayer tempc = loginManager.GetCharacter();

                        //sets the position of the entity's physical body as the physical position
                        FSRigidBody v = Core.Scene.FindEntity(tempc._name).GetComponent<FSRigidBody>();

                        tempc.physicalPosition = v.Transform.Position;

                        DataTemplate dataTemplate = new DataTemplate
                        {
                            RecieverCharacter = tempc,
                            OthersCharacters = FillRecieverList(characterlist, tempc)
                        };
                        //makes the position relative to client's side of the map position
                        dataTemplate.RecieverCharacter.physicalPosition = dataTemplate.RecieverCharacter.physicalPosition - MapContainer.GetMapByName(CurrentLayer.MapName).Entity.Position;
                        string posString = Newtonsoft.Json.JsonConvert.SerializeObject(dataTemplate);

                        MessageManager.SendStringToUniqueID(Entity.Scene, posString, loginManager.GetUniqueID(), MessageType.GameUpdate);
                    }
                }

            }

        }

        public void SetMapLayer(MapLayer layer)
        {
            CurrentLayer = layer;
        }

        private List<CharacterPlayer> FillRecieverList(HashSet<LoginManagerServer> characterlist, CharacterPlayer reciever)
        {
            List<CharacterPlayer> others = new List<CharacterPlayer>();
            foreach (LoginManagerServer l in characterlist)
            {
                CharacterPlayer tempC = l.GetCharacter();

                double deltaDistance = reciever._pos.Length() - tempC._pos.Length();

                //TODO: distance should depend on settings or screen resolution
                if (!loginManager.GetUniqueID().Equals(l.GetUniqueID()) && deltaDistance < 2000)
                {
                    tempC.physicalPosition -= MapContainer.GetMapByName(CurrentLayer.MapName).Entity.Position;
                    tempC._pos -= MapContainer.GetMapByName(CurrentLayer.MapName).Entity.Position;
                    others.Add(tempC);
                }
            }
            return others;
        }

        private void CheckIfConnected()
        {
            if (ServerNetworkSceneComponent.GetNetServer().Connections.Find(c => c.RemoteUniqueIdentifier.Equals(loginManager.GetUniqueID())) == null)
            {
                //HashSet<LoginManagerServer> characterlist = CharacterManager.GetLoginManagerServerList();
                //characterlist.Remove(loginManager);
                //CharacterManager.RemoveLoginManagerServerFromList(loginManager);
                //Entity.Destroy();
            }
        }
    }
}
