using Gravity.Extensions;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gravity.Components
{
    public class Movement
    {
        public Vector2f Position { get; set; } = new Vector2f(0, 0);

        public Vector2f Velocity { get; set; } = new Vector2f(0, 0);

        public double MaxVelocity { get; set; } = 0;

        public void Update(Vector2f acceleration, TimeSpan time)
        {
            Velocity += new Vector2f
            {
                X = acceleration.X * (float)time.TotalSeconds,
                Y = acceleration.Y * (float)time.TotalSeconds
            };

            if (Velocity.Length() > MaxVelocity)
            {
                var normalizedVelocity = Velocity.Normalize();
                Velocity = normalizedVelocity.Scale(MaxVelocity);
            }

            Position += Velocity;
        }
    }
}
