using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Types
{
    class CharacterHead
    {
        public Vector2 _pos;
        public string _name;

        public CharacterHead(int x, int y)
        {
            _pos.X = x;
            _pos.Y = y;
            _name = "admin";
        }


    }
}
