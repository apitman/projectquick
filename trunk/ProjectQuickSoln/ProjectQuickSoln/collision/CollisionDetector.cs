using Microsoft.Xna.Framework;
using CSharpQuadTree;
using System.Collections.Generic;
using System;

namespace QuestAdaptation
{
    public class CollisionDetector
    {
        private QuadTree<Collider> tree;

        public CollisionDetector()
        {
            // TODO: evaluate these numbers
            // sample usage says
            // "Use larger min size, and higher min object values for better performance"
            tree = new QuadTree<Collider>(new DoubleSize(25, 25), 0, false);
        }

        public void register(Collider ci)
        {
            ci.forCollisionDetectorUseOnly(this);
            tree.Insert(ci);
        }

        public List<Collider> query(DoubleRect dr)
        {
            return tree.Query(dr);
        }

        public void remove(Collider ci)
        {
            tree.Remove(ci);
            ci.forCollisionDetectorUseOnly(null);
        }

        public List<QuadTree<Collider>.QuadNode> getAllNodes()
        {
            return tree.GetAllNodes();
        }

        public void handleMovement(Collider mover, Vector2 dp)
        {
            DoubleRect newbounds = mover.Bounds + dp;

            List<Collider> collisions = tree.Query(newbounds);
            List<Collider>.Enumerator i = collisions.GetEnumerator();
            Vector2 allowedMovement = dp;
            Vector2 temp;
            while (i.MoveNext())
            {
                // we will usually collide with our old position - ignore that case
                if (i.Current != mover)
                {
                    bool canMove = CollisionHandler.handleMovement(mover, i.Current, dp, out temp);
                    if (!canMove)
                    {
                        return; // don't allow movement
                    }
                    if (allowedMovement.X > 0.0f)
                        allowedMovement.X = Math.Min(allowedMovement.X, temp.X);
                    else if (allowedMovement.X < 0.0f)
                        allowedMovement.X = Math.Max(allowedMovement.X, temp.X);
                    if (allowedMovement.Y > 0.0f)
                        allowedMovement.Y = Math.Min(allowedMovement.Y, temp.Y);
                    else if (allowedMovement.Y < 0.0f)
                        allowedMovement.Y = Math.Max(allowedMovement.Y, temp.Y);
                }
            }

            mover.move(allowedMovement);
        }
    }
}