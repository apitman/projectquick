using System;

namespace QuestAdaptation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //TestHarness.WorldBuilder.test();
            using (Engine game = new Engine())
            {
                game.Run();
            }
        }
    }
}

