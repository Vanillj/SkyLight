using Client.Managers;
using Nez;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Types.Components
{
    class PlayerComponent : Component, IUpdatable
    {
        private CharacterPlayer _character;

        public PlayerComponent(CharacterPlayer character)
        {
            _character = character;
        }

        public void Update()
        {
            //gets character, if it exists move it
            CharacterPlayer character = null;
            foreach (CharacterPlayer characterPlayer in LoginManagerClient.Othercharacters)
            {
                float f1 = _character.GetHashCode();
                float f2 = characterPlayer.GetHashCode();
                if (_character._name.Equals(characterPlayer._name))
                {
                    character = characterPlayer;
                }
            }
            if(character != null)
                _character = character;
            //Find doesn't work here.
            //CharacterPlayer character = LoginManagerClient.Othercharacters.Find(item => item.Equals(_character));
            if (LoginManagerClient.Othercharacters != null && character == null)
            {
                Entity.Destroy();
            }
            else
            {

                var tween = Entity.Transform.TweenPositionTo(_character.physicalPosition, 0.01f);
                TextComponent text = Entity.GetComponent<TextComponent>();
                
                tween.Start();
            }

        }
    }
}
