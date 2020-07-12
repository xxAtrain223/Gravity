using Gravity.Interfaces;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gravity.Entities
{
    public class Button : Drawable, IUpdatable
    {
        private RectangleShape ButtonBase;
        private RectangleShape ButtonTop;
        private bool _pushed = false;
        public Door Door { get; }

        private TimeSpan PushTime;

        private TimeSpan RemainingTime = TimeSpan.Zero;

        public bool Pushed
        { 
            get
            {
                return _pushed;
            }

            set
            {
                _pushed = value;
                if (value == true)
                {
                    ButtonTop.Size = new Vector2f(32, 8);
                    Door.Activate(true);
                    RemainingTime = PushTime;
                }
                else
                {
                    ButtonTop.Size = new Vector2f(32, 16);
                    Door.Activate(false);
                }
            }
        }

        public Button(Vector2f position, float rotation, Color color, Door door, TimeSpan pushTime)
        {
            ButtonBase = new RectangleShape(new Vector2f(64, 4));
            ButtonBase.Origin = new Vector2f(32, 0);
            ButtonBase.Position = position;
            ButtonBase.Rotation = rotation;
            ButtonBase.FillColor = new Color(63, 0, 63);

            ButtonTop = new RectangleShape(new Vector2f(32, 16));
            ButtonTop.Origin = new Vector2f(16, 0);
            ButtonTop.Position = position;
            ButtonTop.Rotation = rotation;
            ButtonTop.FillColor = color;

            Door = door;
            PushTime = pushTime;
        }

        public void Update(TimeSpan elapsedTime)
        {
            if (Pushed)
            {
                RemainingTime -= elapsedTime;
                if (PushTime > TimeSpan.Zero && RemainingTime <= TimeSpan.Zero)
                {
                    Pushed = false;
                }
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(ButtonTop);
            target.Draw(ButtonBase);
        }

        public FloatRect GetBoundingBox()
        {
            return ButtonTop.GetGlobalBounds();
        }
    }
}
