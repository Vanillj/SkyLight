using Client.Managers;
using GameClient.General;
using GameClient.Managers;
using GameClient.Types.Components.SceneComponents;
using Microsoft.Xna.Framework;
using Nez.Textures;
using Nez.UI;
using Server.Managers;
using Server.Scenes;

namespace GameClient.Scenes
{
    class LoginScene : BaseScene
    {
        public override Table Table { get; set; }
        //public override MessageManager MessageManager { get; set; }

        private TextField textFieldu;
        private TextField textFieldp;

        public override void Initialize()
        {
            base.Initialize();
            var skin = Skin.CreateDefaultSkin();
            //Load Assets, always load textures before generating items so they can match texture and ID
            new TextureContainer().LoadTextures();
            Sprite textfieldTexture = TextureContainer.UIAtlas.GetSprite("Plank_03");

            TextFieldStyle textFields = TextFieldStyle.Create(Color.White, Color.White, Color.Black, Color.DarkGray);
            NinePatchDrawable drawable = new NinePatchDrawable(textfieldTexture, 0, 0, 0, 0) { MinHeight = 20, MinWidth = 50 };
            textFields.Background = drawable;

            Label label = new Label("Menu").SetFontScale(5).SetFontColor(Color.MediumVioletRed);
            Table.Add(label);

            Label user = new Label("Username").SetFontScale(2).SetFontColor(Color.MediumVioletRed);
            Table.Row().SetPadTop(10);
            Table.Add(user).Pad(20);
            textFieldu = new TextField("", textFields);

            Table.Row().SetPadTop(10);
            Table.Add(textFieldu);

            Label pass = new Label("Password").SetFontScale(2).SetFontColor(Color.MediumVioletRed);
            
            Table.Row().SetPadTop(10);
            Table.Add(pass);
            textFieldp = new TextField("", textFields);
            Table.Row().SetPadTop(10);
            textFieldp.SetPasswordMode(true);
            Table.Add(textFieldp);

            
            TextButtonStyle buttonStyle = TextButtonStyle.Create(Color.White, Color.LightGray, Color.LightSlateGray);
            buttonStyle.FontColor = Color.DimGray;
            buttonStyle.OverFontColor = Color.White;
            
            TextButton button = new TextButton("Login", skin);
            button.Pad(10);
            button.PadLeft(20);
            button.PadRight(20);
            button.OnClicked += SendLogin;
            Table.Row().SetPadTop(10);
            Table.Add(button);

            TextButton buttonr = new TextButton("Register", skin);
            buttonr.Pad(10);
            buttonr.PadLeft(20);
            buttonr.PadRight(20);
            buttonr.OnClicked += Sendregister;
            Table.Row().SetPadTop(10);
            Table.Add(buttonr);

        }

        public override void OnStart()
        {
            AddSceneComponent<MessageSceneComponent>();
            base.OnStart();
        }

        private void SendLogin(Button obj)
        {
            string usr = textFieldu.GetText();
            string pass = textFieldp.GetText();

            if (string.IsNullOrWhiteSpace(usr) || string.IsNullOrWhiteSpace(usr))
                return;

            LoginManagerClient loginManagerClient = new LoginManagerClient(usr, pass);
            if (ClientNetworkManager.client != null)
            {
                MessageManager.SetLoginManagerClient(loginManagerClient);
                loginManagerClient.SetUniqueID(ClientNetworkManager.client.UniqueIdentifier);

                if (ClientNetworkManager.client.ServerConnection == null)
                    ClientNetworkManager.TryToConnect(loginManagerClient);

                MessageManager.SendLoginRequest();
            }
        }

        private void Sendregister(Button obj)
        {
            string usr = textFieldu.GetText();
            string pass = CryptoManager.ToHash(textFieldp.GetText());

            if (string.IsNullOrWhiteSpace(usr) || string.IsNullOrEmpty(usr))
                return;

            LoginManagerClient loginManagerClient = new LoginManagerClient(usr, pass);
            if (ClientNetworkManager.client != null)
            {
                MessageManager.SetLoginManagerClient(loginManagerClient);
                loginManagerClient.SetUniqueID(ClientNetworkManager.client.UniqueIdentifier);

                if(ClientNetworkManager.client.ServerConnection == null)
                    ClientNetworkManager.TryToConnect(loginManagerClient);

                MessageManager.SendRegisterRequest();

            }
        }

    }
}
