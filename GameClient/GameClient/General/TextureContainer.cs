using GameClient.Managers;
using GameClient.Types.Item;
using GameClient.Types.Map;
using GameServer.General;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace GameClient.General
{
    class TextureContainer
    {
        private static Dictionary<string, SpriteAtlas> SpriteAtlas = new Dictionary<string, SpriteAtlas>();
        /*public static SpriteAtlas ItemAtlas;
        public static SpriteAtlas UIAtlas;
        public static SpriteAtlas KnightAnimationAtlas;
        public static SpriteAtlas FireBallAnimationAtlas;*/

        public static Texture2D LoginWallpaper;

        public static void LoadTextures()
        {
            LoginWallpaper = Core.Content.LoadTexture("Assets/Wallpaper/game_background.png");
            List<TextureAtlasData> l = FileManager.GetTextureAtlas("Data/" + ConstantValues.TextureAtlasDataFileName);
            foreach (TextureAtlasData data in l)
            {
                try
                {
                    SpriteAtlas.Add(data.Name, Core.Content.LoadSpriteAtlas(data.Path));
                }
                catch (Exception e)
                {

                }
            }
            /*ItemAtlas = Core.Content.LoadSpriteAtlas("Assets/Items/Items.atlas");
            UIAtlas = Core.Content.LoadSpriteAtlas("Assets/UI/UI.atlas");
            KnightAnimationAtlas = Core.Content.LoadSpriteAtlas("Assets/Animations/Knight.atlas");
            FireBallAnimationAtlas = Core.Content.LoadSpriteAtlas("Assets/Animations/SpriteEffects/FireBall.atlas");*/
        }

        public static SpriteAtlas GetSpriteAtlasByName(string name)
        {
            SpriteAtlas temp = null;
            SpriteAtlas.TryGetValue(name, out temp);
            return temp;
        }
    }

    class TextureAtlasData
    {
        public string Path { get; set; }
        public string Name { get; set; }

    }
}
