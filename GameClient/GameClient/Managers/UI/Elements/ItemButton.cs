using GameClient.Types.Item;
using Nez.UI;

namespace GameClient.Managers.UI.Elements
{
    class ItemButton : ImageButton
    {

        public Window HoverWindow;
        public WeaponItem item;
        public ItemButton(ImageButtonStyle style) : base(style)
        {
        }

        public ItemButton(IDrawable imageUp) : base(imageUp)
        {
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
