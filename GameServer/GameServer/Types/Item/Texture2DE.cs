using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Types.Item
{
    class Texture2DE : Texture2D
    {
        public Texture2DE(GraphicsDevice graphicsDevice, int width, int height) : base(graphicsDevice, width, height)
        {
        }

        public Texture2DE(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format) : base(graphicsDevice, width, height, mipmap, format)
        {
        }

        public Texture2DE(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format, int arraySize) : base(graphicsDevice, width, height, mipmap, format, arraySize)
        {
        }

        protected Texture2DE(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format, SurfaceType type, bool shared, int arraySize) : base(graphicsDevice, width, height, mipmap, format, type, shared, arraySize)
        {
        }

        public int ID { get; set; }
        public Texture2DE SetID(int _ID)
        {
            ID = _ID;
            return this;
        }
    }
}
