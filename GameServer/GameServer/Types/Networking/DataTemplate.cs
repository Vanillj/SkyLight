
using Nez.BitmapFonts;
using Server.Types;
using System.Collections.Generic;

namespace GameServer.Types
{
    class DataTemplate
    {
        public CharacterPlayer RecieverCharacter { get; set; }
        public List<CharacterPlayer> OthersCharacters { get; set; }
    }
}
