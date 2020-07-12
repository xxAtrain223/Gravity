﻿using Gravity.Components;
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

        public Character()
        {
            Rectangle = new RectangleShape(new Vector2f(64, 64));
            Rectangle.FillColor = new Color(255, 140, 0);

            Movement.MaxVelocity = 10;

            ParticleEmitter = new ParticleEmitter
            {
                Color = Rectangle.FillColor
            };
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
            ParticleEmitter.Position = Movement.Position;
            ParticleEmitter.Velocity = Movement.Velocity;
            ParticleEmitter.Update(elapsedTime);
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
            //FloatRect xBoundingBox = GetBoundingBox();
            //FloatRect yBoundingBox = GetBoundingBox();
            //Vector2f direction;
            //if (Movement.Velocity.Length() > 1)
            //{
            //    direction = Movement.Velocity.Normalize();
            //}
            //else
            //{
            //    direction = Movement.Velocity;
            //}
            //while (true)
            //{
            //    xBoundingBox.Left -= direction.X;
            //    if (!wallTile.Intersects(xBoundingBox))
            //    {
            //        Movement.Position = new Vector2f(xBoundingBox.Left + Rectangle.Size.X / 2, xBoundingBox.Top + Rectangle.Size.Y / 2);
            //        Movement.Velocity = new Vector2f(0, Movement.Velocity.Y);
            //        break;
            //    }
            //    yBoundingBox.Top -= direction.Y;
            //    if (!wallTile.Intersects(yBoundingBox))
            //    {
            //        Movement.Position = new Vector2f(yBoundingBox.Left + Rectangle.Size.X / 2, yBoundingBox.Top + Rectangle.Size.Y / 2);
            //        Movement.Velocity = new Vector2f(Movement.Velocity.X, 0);
            //        break;
            //    }
            //}

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
