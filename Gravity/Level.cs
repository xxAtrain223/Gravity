using SFML.System;
using SFML.Graphics;
using Gravity.Interfaces;
using System;
using Gravity.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace Gravity
{
    public class Level : Drawable, IUpdatable
    {
        public enum TileType
        {
            Void = 0,
            Wall = 1,
            Air = 2
        }

        private Vector2u TileSize;

        private TileType[,] Tiles;

        private Sprite Sprite;

        private IEnumerable<object> GlobalEntities;

        public Vector2f SpawnPoint { get; }

        private class LevelJson
        {
            public TileType[][] Tiles { get; set; }
            public float[] SpawnPoint { get; set; }
        }

        public Level(IEnumerable<object> globalEntities, string fileName)
        {
            GlobalEntities = globalEntities;

            string file = System.IO.File.ReadAllText(fileName);

            TileSize = new Vector2u(32, 32);
            var levelJson = JsonSerializer.Deserialize<LevelJson>(file);

            Tiles = new TileType[levelJson.Tiles.Length, levelJson.Tiles[0].Length];

            uint TileLength0 = (uint)Tiles.GetLength(0);
            uint TileLength1 = (uint)Tiles.GetLength(1);
            for (uint i = 0; i < TileLength0; i++)
            {
                for (uint j = 0; j < TileLength1; j++)
                {
                    Tiles[i, j] = levelJson.Tiles[i][j];
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

            SpawnPoint = new Vector2f(levelJson.SpawnPoint[0], levelJson.SpawnPoint[1]);
        }

        public void Update(TimeSpan elapsedTime)
        {
            foreach (var entity in GlobalEntities)
            {
                if (entity is ICollidable collidable)
                {
                    var wallOverlap = GetWallOverlap(collidable);
                    while (wallOverlap != null)
                    {
                        collidable.OnWallCollide(wallOverlap.Value);
                        wallOverlap = GetWallOverlap(collidable);
                    }
                }
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Sprite);
        }

        public static Color TileTypeToColor(TileType tile) =>
            tile switch
            {
                TileType.Void => new Color(7, 7, 7),
                TileType.Wall => new Color(31, 31, 31),
                TileType.Air => new Color(63, 63, 63),
                _ => throw new NotImplementedException()
            };

        private FloatRect? GetWallOverlap(ICollidable collidable)
        {
            FloatRect boundingBox = collidable.GetBoundingBox();

            IntRect searchRect = new IntRect(
                (int)boundingBox.Left / (int)TileSize.X,
                (int)boundingBox.Top / (int)TileSize.Y,
                (int)(boundingBox.Left + boundingBox.Width) / (int)TileSize.X - (int)boundingBox.Left / (int)TileSize.X + 1,
                (int)(boundingBox.Top + boundingBox.Height) / (int)TileSize.Y - (int)boundingBox.Top / (int)TileSize.Y + 1);

            if (searchRect.Width < 0)
            {
                searchRect.Left += searchRect.Width;
                searchRect.Width *= -1;
            }
            if (searchRect.Height < 0)
            {
                searchRect.Top += searchRect.Height;
                searchRect.Height *= -1;
            }

            for (int y = searchRect.Top; y < searchRect.Top + searchRect.Height; y++)
            {
                for (int x = searchRect.Left; x < searchRect.Left + searchRect.Width; x++)
                {
                    if (y < 0 || y >= Tiles.GetLength(0) ||
                        x < 0 || x >= Tiles.GetLength(1))
                    {
                        continue;
                    }

                    if (Tiles[y, x] == TileType.Wall)
                    {
                        return new FloatRect(x * TileSize.X, y * TileSize.Y, TileSize.X, TileSize.Y);
                    }
                }
            }

            return null;
        }
    }
}
