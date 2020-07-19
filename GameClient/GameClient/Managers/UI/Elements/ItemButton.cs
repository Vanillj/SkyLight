using GameClient.Types.Item;
using Nez.UI;

namespace GameClient.Managers.UI.Elements
{
    class ItemButton : ImageButton
    {

        public ItemHoverWindow HoverWindow;
        public WeaponItem item;
        public int position = -1;
        public ItemButton(ImageButtonStyle style) : base(style)
        {
        }
        public ItemButton(ImageButtonStyle style, int pos, WeaponItem item) : base(style)
        {
            this.item = item;
            position = pos;
        }
        public ItemButton(IDrawable imageUp) : base(imageUp)
        {
        }
        public ItemButton(IDrawable imageUp, int pos, WeaponItem item) : base(imageUp)
        {
            this.item = item;
            position = pos;
        }

        public ItemButton(Skin skin, string styleName = null) : base(skin, styleName)
        {
        }

        public ItemButton(IDrawable imageUp, IDrawable imageDown) : base(imageUp, imageDown)
        {
        }

        public ItemButton(IDrawable imageUp, IDrawable imageDown, IDrawable imageOver) : base(imageUp, imageDown, imageOver)
        {
        }
    }
}
