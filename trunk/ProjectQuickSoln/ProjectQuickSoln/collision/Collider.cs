using Microsoft.Xna.Framework;
using CSharpQuadTree;
using System;
using System.Collections.Generic;

namespace QuestAdaptation
{
    public enum ColliderType
    {
        PC, NPC, Scenery, Movable, Effect, Trigger
    }

    public class Collider : IQuadObject
    {
        private CollisionDetector cd;
        private DoubleRect bounds;

        public Collidable owner;
        public ColliderType type;

        public Collider(Collidable owner, Rectangle bounds, ColliderType type)
        {
            this.owner = owner;
            this.bounds = new DoubleRect(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            this.type = type;
        }

        public List<Collider> queryDetector(Rectangle queryRect)
        {
            return queryDetector(new DoubleRect(bounds.X, bounds.Y, bounds.Width, bounds.Height));
        }

        public List<Collider> queryDetector(DoubleRect queryRect)
        {
            return cd.query(queryRect);
        }

        public void move(Vector2 dp)
        {
            bounds += dp;
            owner.DrawPosition += dp;

            RaiseBoundsChanged();
        }

        public DoubleRect Bounds
        {
            get { return bounds; }
        }

        public void handleMovement(Vector2 dp)
        {
            cd.handleMovement(this, dp);
        }

        public void forCollisionDetectorUseOnly(CollisionDetector cd)
        {
            this.cd = cd;
        }

        private void RaiseBoundsChanged()
        {
            EventHandler handler = BoundsChanged;
            if (handler != null)
                handler(this, new EventArgs());
        }

        public event System.EventHandler BoundsChanged;
    }

    public interface Collidable
    {
        Collider getCollider();

        Vector2 DrawPosition
        {
            get;
            set;
        }
    }
}