using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CSharpQuadTree;

namespace QuestAdaptation
{
    public class CollisionHandler
    {
        public static bool handleMovement(Collider mover, Collider other, Vector2 dp, out Vector2 allowedMovement)
        {
            switch (mover.type)
            {
                case ColliderType.PC:
                    return handlePCMovement(mover, other, dp, out allowedMovement);
                //case ColliderType.NPC:
                //    break;
                //case ColliderType.Effect:
                //    break;
                //case ColliderType.Movable:
                //    break;
                default:
                    throw new Exception("Something's moving that shouldn't be");
            }
            return false;
        }

        protected static Vector2 scaleBackVelocity(Collider mover, Collider other, Vector2 dp)
        {
            // whew! a lot work just to make it so that you can slide along objects when
            //  using multiple directions

            const float EPS = 0.01f;
            
            DoubleRect dr2 = other.Bounds;

            double reducedX = dp.X;
            double reducedY = dp.Y;

            // Project x only movement and fix x velocity
            Vector2 temp = new Vector2(dp.X, 0);
            DoubleRect dr1 = mover.Bounds + temp;
            if (dr1.IntersectsWith(dr2))
            if (dp.X > 0 && dr1.X + dr1.Width >= dr2.X) // moving right, rhs collision
            {
                double overlap = dr1.X + dr1.Width - dr2.X + EPS;
                reducedX = Math.Max(0.0f, dp.X - overlap);
            }
            else if (dp.X < 0 && dr1.X <= dr2.X + dr2.Width) // moving left, lhs collision
            {
                double overlap = (dr2.X + dr2.Width) - dr1.X + EPS;
                reducedX = Math.Min(0.0f, dp.X + overlap);
            }

            // Project y only movement and fix y velocity
            temp = new Vector2(0, dp.Y);
            dr1 = mover.Bounds + temp;
            if (dr1.IntersectsWith(dr2))
            if (dp.Y > 0 && dr1.Y + dr1.Height >= dr2.Y) // moving down, bottom collision
            {
                double overlap = dr1.Y + dr1.Height - dr2.Y + EPS;
                reducedY = Math.Max(0.0f, dp.Y - overlap);
            }
            else if (dp.Y < 0 && dr1.Y <= dr2.Y + dr2.Height) // moving up, top collision
            {
                double overlap = (dr2.Y + dr2.Height) - dr1.Y + EPS;
                reducedY = Math.Min(0.0f, dp.Y + overlap);
            }

            // TODO make this more intelligent?  not sure how
            // if neither X nor Y reduced, must be a corner collision
            // for now, we just won't allow this (if we allow, you can get stuck, but not bad)
            if (reducedX == dp.X && reducedY == dp.Y)
            {
                return Vector2.Zero;
            }

            return new Vector2((float)reducedX, (float)reducedY);
        }

        protected static bool handlePCMovement(Collider mover, Collider other, Vector2 dp, out Vector2 allowedMovement)
        {
            switch (other.type)
            {
                case ColliderType.Scenery:
                    allowedMovement = scaleBackVelocity(mover, other, dp);
                    return true;
                //case ColliderType.PC:
                //    return handlePCMovement(mover, other, dp, out allowedMovement);
                //    break;
                case ColliderType.NPC:
                    allowedMovement = scaleBackVelocity(mover, other, dp);
                    return true;
                //    break;
                //case ColliderType.Effect:
                //    break;
                //case ColliderType.Movable:
                //    break;
                //case ColliderType.Trigger:
                //    allowedMovement = Vector2.Zero;
                //    ((Trigger)other.owner).handleImpact(mover);
                //    return false;
                default:
                    throw new Exception("PC moved into something it shouldn't have");
            }
        }
    }
}
