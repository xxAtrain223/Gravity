using Gravity;
using System;

namespace Native
{
    class Program
    {
        static void Main(string[] args)
        {
            using var game = new Game();
            game.Run();
        }
    }
}
