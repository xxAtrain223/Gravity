using Gravity.Components;
using Gravity.Extensions;
using Gravity.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Numerics;

namespace Gravity.Entities
{
    public class Character : Drawable, IUpdatable, ICollidable
    {
        public Movement Movement { get; set; } = new Movement();

        public Vector2f Acceleration { get; set; }

        private RectangleShape Rectangle;

        private ParticleEmitter ParticleEmitter;

        public Camera Camera;

        private Color Red = new Color(237, 76, 35);
        private Color Green = new Color(125, 180, 0);
        private Color Blue = new Color(1, 159, 232);
        private Color Yellow = new Color(248, 180, 2);

        public Character(View view, Vector2f spawnPoint)
        {
            Rectangle = new RectangleShape(new Vector2f(64, 64));
            Rectangle.FillColor = Color.White;

            Movement.MaxVelocity = 10;
            Movement.Position = spawnPoint;

            ParticleEmitter = new ParticleEmitter
            {
                Color = Rectangle.FillColor,
                PlayerMovement = Movement
            };

            Camera = new Camera(view, Movement);
        }

        public void Update(TimeSpan elapsedTime)
        {
            double accelerationScale = 2.5;
            Vector2f accelerationInput = new Vector2f(0, 0);
            bool inputReceived = false;

            if (Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                accelerationInput += new Vector2f(-1, 0);
                inputReceived = true;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D) || Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                accelerationInput += new Vector2f(1, 0);
                inputReceived = true;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                accelerationInput += new Vector2f(0, -1);
                inputReceived = true;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S) || Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                accelerationInput += new Vector2f(0, 1);
                inputReceived = true;
            }

            if (inputReceived)
            {
                accelerationInput.Normalize();
                Acceleration = accelerationInput.Scale(accelerationScale);
            }

            Movement.Update(Acceleration, elapsedTime);
            ParticleEmitter.Update(elapsedTime);
            Camera.Update(elapsedTime);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(ParticleEmitter);

            Rectangle.Position = Movement.Position - Rectangle.Size / 2;
            target.Draw(Rectangle);
        }

        public FloatRect GetBoundingBox()
        {
            return new FloatRect(Movement.Position - Rectangle.Size / 2, Rectangle.Size);
        }

        public void OnWallCollide(FloatRect wallTile)
        {
            Movement.Position -= Movement.Velocity;

            var Velocity = Movement.Velocity;
            var boundingBox = GetBoundingBox();

            boundingBox.Left += Velocity.X;
            if (boundingBox.Intersects(wallTile))
            {
                boundingBox.Left -= Velocity.X;
                boundingBox.Left = (float)Math.Round(boundingBox.Left);
                float dir = Math.Sign(Velocity.X);
                while (!boundingBox.Intersects(wallTile))
                    boundingBox.Left += dir;
                boundingBox.Left -= dir * 2;
                Velocity.X = 0;

                Movement.Position = new Vector2f(boundingBox.Left + boundingBox.Width / 2, boundingBox.Top + boundingBox.Height / 2);
                Movement.Velocity = Velocity;
                return;
            }

            boundingBox.Top += Velocity.Y;
            if (boundingBox.Intersects(wallTile))
            {
                boundingBox.Top -= Velocity.Y;
                boundingBox.Top = (float)Math.Round(boundingBox.Top);
                float dir = Math.Sign(Velocity.Y);
                while (!boundingBox.Intersects(wallTile))
                    boundingBox.Top += dir;
                boundingBox.Top -= dir * 2;
                Velocity.Y = 0;

                Movement.Position = new Vector2f(boundingBox.Left + boundingBox.Width / 2, boundingBox.Top + boundingBox.Height / 2);
                Movement.Velocity = Velocity;
                return;
            }
        }
    }
}
