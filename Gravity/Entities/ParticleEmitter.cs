using Gravity.Components;
using Gravity.Extensions;
using Gravity.Interfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gravity.Entities
{
    public class ParticleEmitter : Drawable, IUpdatable
    {
        private IList<Particle> Particles = new List<Particle>();

        private Random Random = new Random();

        public uint Shape { get; set; } = 4;

        public Color Color { get; set; } = Color.White;

        public TimeSpan EmitTime { get; set; } = TimeSpan.FromSeconds(0.01);

        public TimeSpan RandomOffset { get; set; } = TimeSpan.FromSeconds(0.005);

        private TimeSpan TimeToSpawn = TimeSpan.Zero;

        public Movement PlayerMovement { get; set; }

        public void Update(TimeSpan elapsedTime)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Update(elapsedTime);
                if (Particles[i].TimeToLive <= TimeSpan.Zero)
                {
                    Particles.RemoveAt(i--);
                }
            }

            TimeToSpawn -= elapsedTime;

            if (TimeToSpawn <= TimeSpan.Zero)
            {
                var position = PlayerMovement.Position + new Vector2f((float)Random.NextDouble() - 0.5f, (float)Random.NextDouble() - 0.5f).Normalize().Scale(Random.Next(28));
                var shape = new CircleShape(5, Shape)
                {
                    FillColor = Color,
                    Rotation = Random.Next(-360 / (int)Shape / 2, 360 / (int)Shape / 2),
                    Position = position
                };
                Particles.Add(new Particle(shape, TimeSpan.FromSeconds(1), position, PlayerMovement.Velocity));

                TimeToSpawn = EmitTime + RandomOffset * Random.NextDouble();
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var particle in Particles)
            {
                target.Draw(particle);
            }
        }
    }
}
