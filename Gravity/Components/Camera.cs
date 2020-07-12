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
            Position += (PlayerMovement.Position - Position).Normalize().Scale(followRate * elapsedTime.TotalSeconds);

            View.Size = WindowSize.Scale(Zoom);
            View.Center = Position;
        }
    }
}
