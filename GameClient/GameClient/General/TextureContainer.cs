using GameClient.Types.Item;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System.Collections.Generic;
using System.Linq;

namespace GameClient.General
{
    class TextureContainer
    {
        public static SpriteAtlas ItemAtlas;
        public static SpriteAtlas UIAtlas;
        public static SpriteAtlas KnightAnimationAtlas;

        public static Texture2D LoginWallpaper;
        public static void LoadTextures()
        {
            ItemAtlas = Core.Content.LoadSpriteAtlas("Assets/Items/Items.atlas");
            UIAtlas = Core.Content.LoadSpriteAtlas("Assets/UI/UI.atlas");
            LoginWallpaper = Core.Content.LoadTexture("Assets/Wallpaper/game_background.png");
            KnightAnimationAtlas = Core.Content.LoadSpriteAtlas("Assets/Animations/Knight.atlas");
        }

    }
}
