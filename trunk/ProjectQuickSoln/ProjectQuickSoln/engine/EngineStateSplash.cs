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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace QuestAdaptation
{
    public class EngineStateSplash : EngineStateInterface
    {
        const string TEXTUREMAPXML = ".\\Content\\XML\\LoadScripts\\TextureLoader.xml";
        private const int MIN_FRAMES = 30; // TODO change this back to 45

        private Engine engine_;
        private GameTexture splash_;
        private int tick_ = 0;
        private bool loaded_ = false;

        public EngineStateSplash(Engine engine)
        {
            engine_ = engine;
            //splash_ = new GameTexture(@"Sprites\splash", engine.spriteBatch_, engine.GraphicsDevice, engine_.Content);
            splash_ = new GameTexture(@"Sprites\splash");
        }

        public EngineStateInterface update(GameTime gameTime)
        {
            if (tick_ > 0 && !loaded_)
            {
                //TextureMap.getInstance().setContent(engine_.Content);
               // TextureMap.getInstance().loadTextures(TEXTUREMAPXML, engine_);
                FontMap.getInstance().loadFonts("", engine_.spriteBatch_, engine_);
                SoundEngine.getInstance();
                loaded_ = true;
            }

            if (loaded_ && tick_ > MIN_FRAMES)
            {
                EngineManager.StateStack.Pop();
                EngineManager.StateStack.Push(new EngineStateStart(engine_));
                return this;
            }

            tick_++;

            return this;
        }

        public void draw()
        {
            TextureDrawer td = DrawBuffer.getInstance().getUpdateStack().getNext();
            Point p = engine_.GraphicsDevice.Viewport.TitleSafeArea.Center;
            Vector2 v = new Vector2(p.X, p.Y);
            td.set(splash_, 0, v, CoordinateTypeEnum.ABSOLUTE, Constants.DEPTH_DEBUG_LINES,
                true, Color.White, 0f, 1f);
            DrawBuffer.getInstance().getUpdateStack().push();
        }

    }
}
