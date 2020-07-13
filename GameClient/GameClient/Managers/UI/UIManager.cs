using Client.Managers;
using Nez;
using Nez.UI;

namespace GameClient.Managers.UI
{
    class UIManager
    {
        public static Table GenerateInventory(Skin skin)
        {
            Window table = new Window("Title", skin);

            table.SetBounds(Core.GraphicsDevice.Viewport.Width - 200, Core.GraphicsDevice.Viewport.Height - 200, 1, 1);
            table.Add(new Label("Inventory", skin).SetFontScale(1.5f));
            table.Row().SetPadTop(10).SetPadLeft(10).SetPadBottom(10).SetPadRight(10);
            int count = 0;
            foreach (var i in LoginManagerClient.GetCharacter().Inventory)
            {
                if (count % 4 == 0)
                    table.Row().SetPadTop(10).SetPadLeft(10).SetPadBottom(10).SetPadRight(10);

                if (i != null)
                {
                    Label invL = new Label(i.Name, skin);
                    invL.SetWidth(25);
                    invL.SetHeight(25);
                    table.SetWidth(table.GetWidth() + invL.GetWidth());
                    table.SetHeight(table.GetHeight() + invL.GetHeight());
                    table.Add(invL);
                }
                count++;
            }
            return table;
        }
    }
}
