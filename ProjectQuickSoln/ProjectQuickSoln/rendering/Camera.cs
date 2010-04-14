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

namespace QuestAdaptation
{
    public class Camera
    {
        protected float curX_;

        protected float curY_;

        protected float screenWidth_;

        protected float screenHeight_;

        public Camera()
            :this(0,0)
        {
        }

        public Camera(float sWidth, float sHeight)
        {
            screenWidth_ = sWidth;
            screenHeight_ = sHeight;
            curX_ = 0;
            curY_ = 0;
        }
        public void setPosition(float x, float y)
        {
            curX_ = x;
            curY_ = y;
        }

        public void setCenter(float x, float y)
        {
            curX_ = x - (screenWidth_ / 2f);
            curY_ = y - (screenHeight_ / 2f);
        }

        public Vector2 getPosition()
        {
            return new Vector2(curX_, curY_);
        }

        public float getX()
        {
            return curX_;
        }

        public float getY()
        {
            return curY_;
        }

        public float getScreenWidth()
        {
            return screenWidth_;
        }

        public float getScreenHeight()
        {
            return screenHeight_;
        }

        public void setScreenWidth(float width)
        {
            screenWidth_ = width;
        }

        public void setScreenHeight(float height)
        {
            screenHeight_ = height;
        }

        public void move(float X, float Y)
        {
            curX_ += X;
            curY_ += Y;
        }
    }
}
