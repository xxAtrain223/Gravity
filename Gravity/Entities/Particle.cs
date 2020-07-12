using Gravity.Components;
using Gravity.Interfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gravity.Entities
{
    public class Particle : Drawable, IUpdatable
    {
        public Particle(Shape shape, TimeSpan timeToLive, Vector2f position, Vector2f velocity)
        {
            Shape = shape;
            InitialTimeToLive = timeToLive;
            TimeToLive = timeToLive;
            Movement.Position = position;
            Movement.MaxVelocity = 1;
            Movement.Velocity = velocity;
        }

        public Shape Shape { get; }

        private TimeSpan InitialTimeToLive;

        public TimeSpan TimeToLive { get; private set; }

        public Movement Movement { get; set; } = new Movement();

        public void Update(TimeSpan elapsedTime)
        {
            TimeToLive -= elapsedTime;
            Movement.Update(new Vector2f(0, 0), elapsedTime);
            Shape.Position = Movement.Position;

            var color = Shape.FillColor;
            color.A = (byte)(TimeToLive / InitialTimeToLive * 255);
            Shape.FillColor = color;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Shape);
        }
    }
}
