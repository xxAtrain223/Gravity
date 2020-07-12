using Gravity.Interfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gravity.Entities
{
    public class Door : Drawable
    {
        private RectangleShape DoorBack;
        private RectangleShape DoorStripe;
        private bool Invert;
        public bool Closed { get; private set; }

        public Door(Vector2f position, float rotation, Color color, bool invert)
        {
            DoorBack = new RectangleShape(new Vector2f(32, 128));
            DoorBack.Origin = new Vector2f(16, 64);
            DoorBack.Position = position;
            DoorBack.Rotation = rotation;
            DoorBack.FillColor = new Color(63, 0, 63);

            DoorStripe = new RectangleShape(new Vector2f(8, 128));
            DoorStripe.Origin = new Vector2f(4, 64);
            DoorStripe.Position = position;
            DoorStripe.Rotation = rotation;
            DoorStripe.FillColor = color;
            Invert = invert;
            Closed = !invert;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (Closed)
            {
                target.Draw(DoorBack);
                target.Draw(DoorStripe);
            }
        }

        public void Activate(bool activated)
        {
            Closed = !(activated ^ Invert);
        }

        public FloatRect GetBoundingBox()
        {
            return DoorBack.GetGlobalBounds();
        }
    }
}
