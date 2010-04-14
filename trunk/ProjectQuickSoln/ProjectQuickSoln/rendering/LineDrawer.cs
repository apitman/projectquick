/*
***************************************************************************
* Copyright 2009 Eric Barnes, Ken Hartsook, Andrew Pitman, & Jared Segal  *
*                                                                         *
* Licensed under the Apache License, Version 2.0 (the "License");         *
* you may not use this file except in compliance with the License.        *
* You may obtain a copy of the License at                                 *
*                                                                         *
* http://www.apache.org/licenses/LICENSE-2.0                              *
*                                                                         *
* Unless required by applicable law or agreed to in writing, software     *
* distributed under the License is distributed on an "AS IS" BASIS,       *
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.*
* See the License for the specific language governing permissions and     *
* limitations under the License.                                          *
***************************************************************************
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuestAdaptation
{
    public static class LineDrawer
    {
        public static GameTexture blank_ = null;

        static readonly Color LINE_COLOR = Color.LimeGreen;
        const float LINE_DEPTH = Constants.DEPTH_DEBUG_LINES;

        public static void drawLine(Vector2 point1, Vector2 point2)
        {
            drawLine(point1, point2, LINE_COLOR);
        }

        public static void drawLine(Vector2 point1, Vector2 point2, Color color)
        {
            if (blank_ == null)
            {
                init();
            }
            Vector2 center = point1;
            center.X += point2.X;
            center.Y += point2.Y;
            center.X /= 2.0f;
            center.Y /= 2.0f;
            Vector2 rotation = point2 - point1;
            //float rotationAngle = (float)Math.Atan2((double)rotation.Y, (double)rotation.X);
            //blank_.drawImageWithDim(0, new Rectangle((int)point1.X, (int)point1.Y, (int)rotation.Length(), 2), rotationAngle, LINE_DEPTH, Vector2.Zero, LINE_COLOR);
            TextureDrawer drawer = DrawBuffer.getInstance().getUpdateStack().getNext();
            drawer.Texture = blank_;
            drawer.ImageIndex = 0;
            drawer.Direction = rotation;
            drawer.CoordinateType = CoordinateTypeEnum.RELATIVE;
            drawer.Dest = true;
            drawer.Destination = new Rectangle((int)point1.X, (int)point1.Y, (int)rotation.Length(), 2);
            drawer.Depth = LINE_DEPTH;
            drawer.Centered = false;
            drawer.Color = color;
            DrawBuffer.getInstance().getUpdateStack().push();
        }

        public static void init()
        {
            Engine e = GlobalHelper.getInstance().getEngine();
            blank_ = new GameTexture(@"Sprites\blank");
        }
    }
}
