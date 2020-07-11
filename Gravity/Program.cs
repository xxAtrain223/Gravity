using System;
using System.Timers;
using Gravity.Entities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Gravity
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new RenderWindow(new VideoMode(800, 600), "Gravity");

            window.SetVerticalSyncEnabled(true);
            window.SetKeyRepeatEnabled(false);

            window.Closed += (_, __) => window.Close();
            
            var character = new Character();

            var t1 = DateTime.Now;
            while (window.IsOpen)
            {
                var t2 = DateTime.Now;
                var elapsedTime = t2 - t1;
                t1 = t2;

                window.DispatchEvents();
                window.Clear(new Color(63, 63, 63));
                character.Update(elapsedTime);
                window.Draw(character);
                window.Display();
            }
        }
    }
}
