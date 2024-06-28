using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursework_Retake.World
{
    enum Tiles_Collision
    {
        Platform = 0,
        Passable = 1,
        Impassable = 2,
        Lava = 3,
    }

    struct Tiles
    {
        public Texture2D text;
        public Tiles_Collision Collision;
        public const int TileWidth = 40;
        public const int TileHeight = 32;
        public static readonly Vector2 Tilesize = new Vector2(TileWidth, TileHeight);

        public Tiles(Texture2D texture, Tiles_Collision collision)
        {
            text = texture;
            Collision = collision;
        }
    }
}
