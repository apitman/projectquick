using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace QuestAdaptation
{
    public static class EngineManager
    {
        public static Engine Engine { get { return engine_; } }
        public static Stack<EngineStateInterface> StateStack;

        private static Engine engine_;

        public static void initialize(Engine engine)
        {
            engine_ = engine;
            StateStack = new Stack<EngineStateInterface>();
        }

        public static EngineStateInterface Peek()
        {
            return StateStack.Peek();
        }

        public static void ReplaceCurrent(EngineStateInterface esi)
        {
            StateStack.Pop();
            StateStack.Push(esi);
        }

        public static void Update(GameTime gameTime)
        {
            StateStack.Peek().update(gameTime);
        }

        public static void Draw()
        {
            StateStack.Peek().draw();
        }
    }
}
