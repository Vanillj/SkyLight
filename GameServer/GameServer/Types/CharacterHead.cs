using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Server.Types
{
    class CharacterHead
    {
        public Vector2 _pos;
        public string _name;
        private Texture2D playerTexture { get; set; }
        public int playerTextureID { get; set; }
        public CharacterHead(float x, float y, string name)
        {
            _pos.X = x;
            _pos.Y = y;
            _name = name;
        }

    }
}
