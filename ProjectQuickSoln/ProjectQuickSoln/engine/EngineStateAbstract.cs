namespace QuestAdaptation
{
    public abstract class EngineAbstract : EngineStateInterface
    {
        protected Engine engine_;

        public EngineAbstract(Engine engine)
        {
            engine_ = engine;
        }

        public abstract EngineStateInterface update(Microsoft.Xna.Framework.GameTime gameTime);

        public abstract void draw();

    }
    
}