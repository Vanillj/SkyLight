using Client.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Server.Scenes;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Managers
{
    class PlayerManager
    {
        public static Entity CreatePlayer(CharacterPlayer player, BaseScene scene)
        {
            //TODO: Get Textures from ID later
            Texture2D playerTexture = Core.Content.Load<Texture2D>("images/playerTexture");
             
            Entity ePlayer = scene.CreateEntity("player");
            TextComponent textComponent = new TextComponent(Graphics.Instance.BitmapFont, player._name, Vector2.Zero, Color.White).SetHorizontalAlign(HorizontalAlign.Center).SetVerticalAlign(VerticalAlign.Top);
            ePlayer.SetPosition(player._pos)
                .AddComponent(new SpriteRenderer(playerTexture)).AddComponent(textComponent).SetRenderLayer(-200);

            return ePlayer;
        }
    }
}
