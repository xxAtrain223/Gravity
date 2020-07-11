using System;
using System.Collections.Generic;
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
            var window = new RenderWindow(new VideoMode(1280, 720), "Gravity");

            window.SetVerticalSyncEnabled(true);
            window.SetKeyRepeatEnabled(false);

            window.Closed += (_, __) => window.Close();

            window.KeyPressed += (_, keyArgs) =>
            {
                if (keyArgs.Code == Keyboard.Key.Escape)
                {
                    window.Close();
                }
            };

            List<object> globalEntities = new List<object>();

            var character = new Character();
            character.Movement.Position = new Vector2f(64, 64);
            globalEntities.Add(character);

            var level = new Level(globalEntities);

            var t1 = DateTime.Now;
            while (window.IsOpen)
            {
                var t2 = DateTime.Now;
                var elapsedTime = t2 - t1;
                t1 = t2;

                window.DispatchEvents();
                window.Clear(Color.Black);
                character.Update(elapsedTime);
                level.Update(elapsedTime);
                window.Draw(level);
                window.Draw(character);
                window.Display();
            }
        }
    }
}
