
//using var game = new Coursework_Retake.Game1();
//game.Run();

using System;

namespace Coursework_Retake
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}

