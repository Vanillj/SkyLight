using Client.Managers;
using GameClient.General;
using GameClient.Scenes;
using GameClient.Types.Components.Components;
using GameClient.Types.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Server.Scenes;
using Server.Types;

namespace GameClient.Managers
{
    class PlayerManager
    {
        public static PlayerEntity CreatePlayer(CharacterPlayer player, MainScene scene)
        {
            //SpriteAnimation Idle = TextureContainer.KnightAnimationAtlas.GetAnimation("Idle");
            //SpriteAnimation Movement = TextureContainer.KnightAnimationAtlas.GetAnimation("Movement");

            //TODO: Get Textures from ID later

            PlayerEntity pE = new PlayerEntity(player);

            scene.AddEntity(pE);
            scene.playerComponent = pE.GetComponent<PlayerComponent>();

            return pE;
        }
    }
}
