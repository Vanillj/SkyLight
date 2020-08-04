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

            //Entity ePlayer = scene.CreateEntity(player._name);
            //TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, player._name, Vector2.Zero, Color.White);
            //ePlayer.SetPosition(player.physicalPosition)
            //    .AddComponent(textComponent).SetHorizontalAlign(HorizontalAlign.Center).SetVerticalAlign(VerticalAlign.Top).SetRenderLayer(-200);
            scene.AddEntity(pE);
            scene.playerComponent = pE.GetComponent<PlayerComponent>();
            //ePlayer.AddComponent(scene.playerComponent);

            //SpriteAnimator ani = ePlayer.AddComponent<SpriteAnimator>();
            //ani.AddAnimation("Idle", Idle);
            //ani.AddAnimation("Movement", Movement);
            //ani.Play("Idle");
            //ePlayer.SetScale(3.5f);
            return pE;
        }
    }
}
