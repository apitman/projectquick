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

namespace QuestAdaptation
{
    /// <summary>
    /// Singleton that stores GameFonts and allows text to be drawn to the screen
    /// </summary>
    class FontMap
    {
        /// <summary>
        /// The Dictionary (or Map) of FontEnums to GameFonts
        /// </summary>
        protected Dictionary<FontEnum, GameFont> fonts_;

        /// <summary>
        /// The instance data member to implement a Singleton class
        /// </summary>
        protected static FontMap fontMapInstance_ = null;

        /// <summary>
        /// The constructor
        /// </summary>
        private FontMap()
        {
            fonts_ = new Dictionary<FontEnum, GameFont>();
        }

        /// <summary>
        /// Gets an instance of the Singleton FontMap
        /// </summary>
        /// <returns>Returns the instance to use</returns>
        public static FontMap getInstance()
        {
            if (fontMapInstance_ == null)
            {
                fontMapInstance_ = new FontMap();
            }
            return fontMapInstance_;
        }

        /// <summary>
        /// Loads SpriteFont information from file
        /// </summary>
        /// <param name="filepath">The .xml file that contains all the Font information</param>
        /// <param name="spriteBatch">The spriteBatch that will be used to draw text</param>
        /// <param name="engine">The main Engine class</param>
        public void loadFonts(string filename, SpriteBatch spriteBatch, Engine engine)
        {
            //TODO: Eventually, create automatic loading of fonts based on an xml file.
            //      For now, just create the load for each font in this function
            fonts_.Add(FontEnum.Kootenay8, new GameFont("SpriteFonts/Kootenay8", spriteBatch, engine));
            fonts_.Add(FontEnum.Kootenay14, new GameFont("SpriteFonts/Kootenay", spriteBatch, engine));
            fonts_.Add(FontEnum.Kootenay48, new GameFont("SpriteFonts/Kootenay48", spriteBatch, engine));
            fonts_.Add(FontEnum.Lindsey, new GameFont("SpriteFonts/Lindsey", spriteBatch, engine));
            fonts_.Add(FontEnum.Miramonte, new GameFont("SpriteFonts/Miramonte", spriteBatch, engine));
            fonts_.Add(FontEnum.MiramonteBold, new GameFont("SpriteFonts/MiramonteBold", spriteBatch, engine));
            fonts_.Add(FontEnum.Pericles, new GameFont("SpriteFonts/Pericles", spriteBatch, engine));
            fonts_.Add(FontEnum.PericlesLight, new GameFont("SpriteFonts/PericlesLight", spriteBatch, engine));
            fonts_.Add(FontEnum.Pescadero, new GameFont("SpriteFonts/Pescadero", spriteBatch, engine));
            fonts_.Add(FontEnum.PescaderoBold, new GameFont("SpriteFonts/PescaderoBold", spriteBatch, engine));
        }

        /// <summary>
        /// Gets the GameFont used for drawing text
        /// </summary>
        /// <param name="fontName">The font you want to draw</param>
        /// <returns>The GameFont used for actually drawing the text</returns>
        public GameFont getFont(FontEnum fontName)
        {
            if (!fonts_.ContainsKey(fontName))
            {
                return null;
            }
            return fonts_[fontName];
        }
    }
}
