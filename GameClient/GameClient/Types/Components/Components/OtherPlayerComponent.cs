using Client.Managers;
using GameClient.Types.Components.SceneComponents;
using GameServer.General;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Sprites;
using Server.Types;
using System.Threading;

namespace GameClient.Types.Components
{
    class OtherPlayerComponent : Component, IUpdatable
    {
        private CharacterPlayer _character;
        private Vector2 LastPosition = Vector2.Zero;
        private float timer = 0;

        public OtherPlayerComponent(CharacterPlayer character)
        {
            _character = character;
        }

        public void Update()
        {
            timer += Time.DeltaTime;

            if (timer >= ConstantValues.UpdateFrequency)
            {
                timer -= ConstantValues.UpdateFrequency;
                FixedUpdate();
            }
            
        }

        private void FixedUpdate()
        {
            //gets character, if it exists move it
            CharacterPlayer character = null;
            foreach (CharacterPlayer characterPlayer in LoginManagerClient.OtherCharacters)
            {
                if (_character._name.Equals(characterPlayer._name))
                {
                    character = characterPlayer;
                    SpriteAnimator ani = Entity.GetComponent<SpriteAnimator>();
                    if (LastPosition != characterPlayer._pos)
                    {
                        if (ani.CurrentAnimationName != "Movement")
                        {
                            ani.Play("Movement");
                        }
                    }
                    else
                    {
                        if (ani.CurrentAnimationName != "Idle")
                        {
                            ani.Play("Idle");
                        }
                    }
                    LastPosition = characterPlayer._pos;
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
                //Entity.Transform.Position = new Vector2(MathHelper.Lerp(Entity.Transform.Position.X, _character.physicalPosition.X, 0.01f), MathHelper.Lerp(Entity.Transform.Position.Y, _character.physicalPosition.Y, 0.01f));
                Entity.SetPosition(_character.physicalPosition);
            }
        }
    }
}
