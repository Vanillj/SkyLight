using GameClient.Types.Item;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.General
{
    class TextureContainer
    {
        public static HashSet<Texture2DE> Texture2DEList = new HashSet<Texture2DE>();
        public void LoadTextures()
        {
            Texture2D tempTexture = Core.Content.Load<Texture2D>("images/playerTexture");
            Texture2DE tempT2DE = new Texture2DE(tempTexture.GraphicsDevice, tempTexture.Width, tempTexture.Height);
            int count = tempTexture.Width * tempTexture.Height;
            Color[] data = new Color[count];
            tempTexture.GetData(0, tempTexture.Bounds, data, 0, count);
            tempT2DE.SetData(data);
            Texture2DEList.Add(tempT2DE.SetID(2));
        }
        
        public static Texture2DE GetTextureByID(int ID)
        {
            return Texture2DEList.FirstOrDefault(t => t.ID.Equals(ID));
        }
    }
}
