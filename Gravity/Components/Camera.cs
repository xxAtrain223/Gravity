using Gravity.Extensions;
using Gravity.Interfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Gravity.Components
{
    public class Camera : IUpdatable
    {
        public View View;

        private Movement PlayerMovement;

        private Vector2f WindowSize;

        private float Zoom;

        private Vector2f Position;

        public Camera(View view, Movement playerMovement)
        {
            View = view;
            PlayerMovement = playerMovement;
            WindowSize = view.Size;
            Zoom = 1;
            Position = playerMovement.Position;
        }

        public void Update(TimeSpan elapsedTime)
        {
            var newZoom = (float)(PlayerMovement.Velocity.Length() / PlayerMovement.MaxVelocity + 1);
            Zoom += (float)((newZoom - Zoom) * elapsedTime.TotalSeconds);

            double distance = (PlayerMovement.Position - Position).Length();
            double followRate = distance * 3;
            var move = (PlayerMovement.Position - Position);
            if (move.X != 0 || move.Y != 0)
            {
                move = move.Normalize().Scale(followRate * elapsedTime.TotalSeconds);
            }
            Position += move;

            View.Size = WindowSize.Scale(Zoom);
            View.Center = Position;
        }
    }
}
