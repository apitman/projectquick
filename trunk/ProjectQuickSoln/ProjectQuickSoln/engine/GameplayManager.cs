using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace QuestAdaptation
{
    public class GameplayManager
    {

        public static EngineStateGameplay GameplayState { get { return gameplayState; } }
        private static EngineStateGameplay gameplayState = null;

        /*
        public static PlayerController Player { get { return playerController; } }
        private static PlayerController playerController = null;

        public static Area ActiveArea { get { return activeArea; } }
        private static Area activeArea = null;
        */

        public static void initialize(EngineStateGameplay esg /*add other arguments*/)
        {
            if (GameplayState != null)
                throw new Exception("Only one EngineStateGameplay allowed at once!");

            gameplayState = esg;
        }

        public static void drawHUD()
        {

        }
    }
}
