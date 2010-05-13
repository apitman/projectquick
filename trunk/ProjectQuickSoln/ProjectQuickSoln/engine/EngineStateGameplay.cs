using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using CSharpQuadTree;
using System;

namespace QuestAdaptation
{
    public class EngineStateGameplay : EngineAbstract
    {
        private GameTexture mJared = new GameTexture(@"Sprites\sprite1");
        public EngineStateGameplay(Engine engine) : base(engine)
        {
            // Init logic here
            GameplayManager.initialize(this);
        }

        public override EngineStateInterface update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            // Insert input handling and game logic here

            return this;
        }

        public override void draw()
        {

            //GameplayManager.Player.draw();
            //GameplayManager.ActiveArea.draw();
            //GameplayManager.drawHUD();

            // point this function to quad nodes to draw the collision detection stuff 
            // drawCollisionDetector(false);
        }

        protected void drawCollisionDetector(bool drawQuadTree)
        {
            // point this to the collision detector's nodes to be able to draw them
            List<QuadTree<Collider>.QuadNode> nodes = null;
            foreach (QuadTree<Collider>.QuadNode node in nodes)
            {
                if (drawQuadTree)
                {
                    DoubleRect dr = node.Bounds;
                    LineDrawer.drawLine(new Vector2((float)dr.X, (float)dr.Y),
                                        new Vector2((float)dr.X + (float)dr.Width, (float)dr.Y),
                                        Color.AliceBlue);
                    LineDrawer.drawLine(new Vector2((float)dr.X, (float)dr.Y),
                                        new Vector2((float)dr.X, (float)dr.Y + (float)dr.Height),
                                        Color.AliceBlue);
                    LineDrawer.drawLine(new Vector2((float)dr.X + (float)dr.Width, (float)dr.Y),
                                        new Vector2((float)dr.X + (float)dr.Width, (float)dr.Y + (float)dr.Height),
                                        Color.AliceBlue);
                    LineDrawer.drawLine(new Vector2((float)dr.X, (float)dr.Y + (float)dr.Height),
                                        new Vector2((float)dr.X + (float)dr.Width, (float)dr.Y + (float)dr.Height),
                                        Color.AliceBlue);
                }
                
                foreach (Collider collider in node.quadObjects)
                {
                    DoubleRect dr2 = collider.Bounds;
                    LineDrawer.drawLine(new Vector2((float)dr2.X, (float)dr2.Y),
                                    new Vector2((float)dr2.X + (float)dr2.Width, (float)dr2.Y),
                                    Color.LimeGreen);
                    LineDrawer.drawLine(new Vector2((float)dr2.X, (float)dr2.Y),
                                        new Vector2((float)dr2.X, (float)dr2.Y + (float)dr2.Height),
                                        Color.LimeGreen);
                    LineDrawer.drawLine(new Vector2((float)dr2.X + (float)dr2.Width, (float)dr2.Y),
                                        new Vector2((float)dr2.X + (float)dr2.Width, (float)dr2.Y + (float)dr2.Height),
                                        Color.LimeGreen);
                    LineDrawer.drawLine(new Vector2((float)dr2.X, (float)dr2.Y + (float)dr2.Height),
                                        new Vector2((float)dr2.X + (float)dr2.Width, (float)dr2.Y + (float)dr2.Height),
                                        Color.LimeGreen);
                }
            }
        }
    }
}