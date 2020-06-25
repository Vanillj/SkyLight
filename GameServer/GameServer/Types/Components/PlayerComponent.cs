using Client.Managers;
using Nez;
using Nez.BitmapFonts;
using Nez.Farseer;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            timeSpan += Time.DeltaTime;
            if (timeSpan > 0.05)
            {
                HashSet<LoginManagerServer> characterlist = CharacterManager.GetLoginManagerServerList();
                if (characterlist != null && characterlist.Count > 0 && loginManager != null)
                {
                    CharacterPlayer tempc = loginManager.GetCharacter();
                    var v = Core.Scene.FindEntity(tempc._name).GetComponent<FSRigidBody>();
                    tempc.physicalPosition = v.Transform.Position;
                    DataTemplate dataTemplate = new DataTemplate { RecieverCharacter = tempc };

                    dataTemplate.OthersCharacters = new List<CharacterPlayer>();

                    foreach (LoginManagerServer l in characterlist)
                    {
                        CharacterPlayer tempC = l.GetCharacter();
                        //double distance = Math.Sqrt(tempC._pos.X * tempC._pos.X + tempC._pos.Y * tempC._pos.Y);
                        double distance = tempC._pos.Length();

                        //TODO: distance should depend on screen resolution or settings
                        if (!loginManager.GetUniqueID().Equals(l.GetUniqueID()) && distance < 2000)
                        {
                            dataTemplate.OthersCharacters.Add(tempC);
                        }
                    }
                    string posString = Newtonsoft.Json.JsonConvert.SerializeObject(dataTemplate);
                    MessageManager.SendStringToUniqueID(posString, loginManager.GetUniqueID(), MessageType.GameUpdate);
                }
            }

        }
    }
}
