using SFML.System;
using SFML.Graphics;
using Gravity.Interfaces;
using System;
using Gravity.Entities;

namespace Gravity
{
    public class Level : Drawable, IUpdatable
    {
        public enum TileType
        {
            Void,
            Wall,
            Air
        }

        private Vector2u TileSize;

        private TileType[,] Tiles;

        private Sprite Sprite;

        public Level()
        {
            TileSize = new Vector2u(32, 32);
            Tiles = new TileType[32, 40];

            uint TileLength0 = (uint)Tiles.GetLength(0);
            uint TileLength1 = (uint)Tiles.GetLength(1);
            for (uint i = 0; i < TileLength0; i++)
            {
                for (uint j = 0; j < TileLength1; j++)
                {
                    if (i == 0 || i == (TileLength0 - 1) ||
                        j == 0 || j == (TileLength1 - 1))
                    {
                        Tiles[i, j] = TileType.Wall;
                    }
                    else
                    {
                        Tiles[i, j] = TileType.Air;
                    }
                }
            }

            RenderTexture renderTexture = new RenderTexture(TileSize.X * TileLength1, TileSize.Y * TileLength0);
            RectangleShape tile = new RectangleShape(new Vector2f(TileSize.X, TileSize.Y));

            renderTexture.Clear(Color.Magenta);
            for (uint i = 0; i < TileLength0; i++)
            {
                for (uint j = 0; j < TileLength1; j++)
                {
                    tile.FillColor = TileTypeToColor(Tiles[i, j]);
                    tile.Position = new Vector2f(TileSize.X * j, TileSize.Y * i);
                    renderTexture.Draw(tile);
                }
            }
            renderTexture.Display();
            Sprite = new Sprite(renderTexture.Texture);
        }

        public void Update(TimeSpan elapsedTime)
        {
            
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Sprite);
        }

        public static Color TileTypeToColor(TileType tile) =>
            tile switch
            {
                TileType.Void => new Color(0, 0, 0),
                TileType.Wall => new Color(31, 31, 31),
                TileType.Air => new Color(63, 63, 63),
                _ => throw new NotImplementedException()
            };

    }
}
