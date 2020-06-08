using Client.Managers;
using GameClient.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using Nez.UI;
using Server.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClient.Scenes
{
    class MainScene : BaseScene
    {
        public override Table Table { get; set; }
        //public override MessageManager MessageManager { get; set; }
        public InputManager InputManager { get; set; }

        private Label label;
        private Entity player;
        public override void Initialize()
        {
            base.Initialize();
            Texture2D playerTexture = Content.Load<Texture2D>("images/playerTexture");
            player = CreateEntity("Player");
            player.AddComponent(new SpriteRenderer(playerTexture));
            player.Position = new Vector2(0, 0);
            Table.Row();
            label = new Label("Logged in!").SetFontScale(3);
            Table.Add(label);
            

        }
        public override void Update()
        {
            if (InputManager != null)
                MessageManager.inputManager.CheckForInput();
            Vector2 vector2 = LoginManagerClient.GetCharacter()._pos;
            if (vector2 != player.Position)
            {
                var newP = vector2 - player.Transform.Position;
                player.Transform.Position += newP;
            }
            base.Update();
            
        }
    }
}
