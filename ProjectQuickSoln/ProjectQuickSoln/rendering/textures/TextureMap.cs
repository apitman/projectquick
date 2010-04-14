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
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.IO;

namespace QuestAdaptation
{
    /// <summary>
    /// A Singleton class which holds all the textures for the game and maps them with their
    /// names.
    /// </summary>
    class TextureMap
    {
        protected Dictionary<string, GameTexture> textures_;

        protected static TextureMap textureMapInstance_ = null;

        protected ContentManager content_;

        /// <summary>
        /// Private constructor so that only one can be created.
        /// </summary>
        private TextureMap()
        {
            textures_ = new Dictionary<string, GameTexture>();
        }

        /// <summary>
        /// Get the Singleton instance of this class.
        /// </summary>
        /// <returns></returns>
        public static TextureMap getInstance()
        {
            if (textureMapInstance_ == null)
            {
                textureMapInstance_ = new TextureMap();
            }
            return textureMapInstance_;
        }

        /// <summary>
        /// Simply calls TextureMap.getInstance().getTexture(texture)
        /// </summary>
        /// <param name="texture">Name of the texture</param>
        /// <returns>The GameTexture object requested, null if not found</returns>
        public static GameTexture fetchTexture(string texture)
        {
            return getInstance().getTexture(texture);
        }

        /// <summary>
        /// Get the ContentManager.
        /// </summary>
        /// <returns>The ContentManager for the game.</returns>
        public ContentManager getContent()
        {
            return content_;
        }

        /// <summary>
        /// Set the ContentManager.
        /// </summary>
        /// <param name="content">The ContentManager for the game.</param>
        public void setContent(ContentManager content)
        {
            content_ = content;
        }

        /// <summary>
        /// Load all the textures for the game.
        /// </summary>
        /// <param name="filepath">Filename of the texture document</param>
        /// <param name="spriteBatch">SpriteBatch for the game</param>
        /// <param name="graphics">GraphicsDevice for the game</param>
        public void loadTextures(string filename, Engine engine)
        {
            SpriteBatch spriteBatch = engine.spriteBatch_;
            GraphicsDevice graphics = engine.GraphicsDevice;

            using (ManagedXml manager = new ManagedXml(engine))
            {
                try
                {
                    XmlDocument document = manager.loadFromFile(filename);
                    XmlNodeList textureXMLs = document.GetElementsByTagName("texture");
                    foreach (XmlNode node in textureXMLs)
                    {
                        KeyValuePair<string, GameTexture> tempPair = GameTexture.loadTextureFromFile(node.InnerText, spriteBatch, graphics);
                        textures_.Add(tempPair.Key, tempPair.Value);
                    }
                    XmlNodeList images = document.GetElementsByTagName("image");
                    foreach (XmlNode node in images)
                    {
                        string key = "", image = "";
                        XmlNodeList children = node.ChildNodes;
                        foreach (XmlNode child in children)
                        {
                            if (child.LocalName == "key")
                            {
                                key = child.InnerText;
                            }
                            else if (child.LocalName == "handle")
                            {
                                image = child.InnerText;
                            }
                        }
                        textures_.Add(key, new GameTexture(image));
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("FATAL ERROR: Texture Map failed to load!");
                    throw e;
                }
            }
            // Don't collect here, because we will be reading in a lot of these XML docs
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Get a texture from the map.
        /// </summary>
        /// <param name="textureName">Name of the texture</param>
        /// <returns>GameTexture with the name textureName</returns>
        public GameTexture getTexture(string textureName)
        {
            if(!textures_.ContainsKey(textureName))
            {
                return null;
            }
            return textures_[textureName];
        }
    }
}
