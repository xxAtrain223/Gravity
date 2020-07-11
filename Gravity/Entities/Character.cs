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
    public class Character : Drawable, IUpdatable
    {
        public Movement Movement { get; set; } = new Movement();

        public Vector2f Acceleration { get; set; }

        private RectangleShape Rectangle;

        public Character()
        {
            Rectangle = new RectangleShape(new Vector2f(64, 64))
            {
                FillColor = new Color(255, 140, 0)
            };
        }

        public void Update(TimeSpan elapsedTime)
        {
            double accelerationScale = 1;
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
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Rectangle.Position = Movement.Position - Rectangle.Size / 2;
            target.Draw(Rectangle);
        }
    }
}
