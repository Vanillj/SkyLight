using Client.Managers;
using GameClient.Types.Components.SceneComponents;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Server.Types;

namespace GameClient.Types.Components
{
    class OtherPlayerComponent : Component, IUpdatable
    {
        private CharacterPlayer _character;

        public OtherPlayerComponent(CharacterPlayer character)
        {
            _character = character;
        }
        public void Update()
        {
            //gets character, if it exists move it
            CharacterPlayer character = null;
            foreach (CharacterPlayer characterPlayer in LoginManagerClient.OtherCharacters)
            {
                if (_character._name.Equals(characterPlayer._name))
                {
                    character = characterPlayer;
                }
            }
            if (character != null)
                _character = character;
            //Change to find later
            //CharacterPlayer character = LoginManagerClient.Othercharacters.Find(item => item._name.Equals(_character._name));
            if (LoginManagerClient.OtherCharacters != null && character == null)
            {
                Entity.Destroy();
            }
            else
            {
                Entity.Transform.Position = new Vector2(MathHelper.Lerp(Entity.Transform.Position.X, _character.physicalPosition.X, 0.02f), MathHelper.Lerp(Entity.Transform.Position.Y, _character.physicalPosition.Y, 0.02f));
            }

        }
    }
}
