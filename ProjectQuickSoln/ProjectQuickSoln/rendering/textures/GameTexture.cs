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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;

namespace QuestAdaptation
{
    public class GameTexture
    {
        #region Static

        internal static SpriteBatch spriteBatch_ { get; set; }

        internal static ContentManager content_ { get; set; }

        #endregion

        //Texture image for this GameTexture
        internal Texture2D texture_ { get; set; }

        //Each Vector4 is the bounds (x of top right corner, y of top right corner,
        //tileWidth, tileHeight) of each individual frame, or image in this texture
        internal Rectangle[] imageDimensions_ { get; set; }

        /// <summary>
        /// Basic constructor for a GameTexture
        /// </summary>
        public GameTexture(string assetName)
        {
            texture_ = content_.Load<Texture2D>(assetName);

            imageDimensions_ = new Rectangle[1];
            imageDimensions_[0] = new Rectangle(0, 0, texture_.Width, texture_.Height);
        }

        public GameTexture(string assetName, Rectangle[] imageDimensions)
        {
            texture_ = content_.Load<Texture2D>(assetName);

            imageDimensions_ = imageDimensions;
        }

        /// <summary>
        /// Load a texture from an xml file.
        /// </summary>
        /// <param name="filepath">Filename of the xml file</param>
        /// <param name="spriteBatch">SpriteBatch of the game</param>
        /// <param name="graphics">GraphicsDevice of the game</param>
        /// <returns>KeyValuePair with the key(name of the texture) and value(GameTexture loaded)</returns>
        
        public static KeyValuePair<string, GameTexture> loadTextureFromFile(string filename, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            XmlTextReader reader = new XmlTextReader(filename);
            reader.ReadToFollowing("Key");
            string key = reader.ReadElementString();
            reader.ReadToFollowing("ImageFilename");
            string imageFilename = reader.ReadElementString();
            reader.ReadToFollowing("ImageDimensions");
            XmlReader dimReader = reader.ReadSubtree();
            dimReader.ReadToFollowing("NumberOfImages");
            int numberOfImages = dimReader.ReadElementContentAsInt();
            Rectangle[] imageDimensions = new Rectangle[numberOfImages];
            for (int i = 0; i < numberOfImages; i++)
            {
                dimReader.ReadToFollowing("Image");
                int x, y, w, h;
                XmlReader imReader = dimReader.ReadSubtree();
                imReader.ReadToFollowing("x");
                x = imReader.ReadElementContentAsInt();
                imReader.ReadToFollowing("y");
                y = imReader.ReadElementContentAsInt();
                imReader.ReadToFollowing("w");
                w = imReader.ReadElementContentAsInt();
                imReader.ReadToFollowing("h");
                h = imReader.ReadElementContentAsInt();
                imageDimensions[i] = new Rectangle(x, y, w, h);
            }
            return new KeyValuePair<string, GameTexture>(key, new GameTexture(imageFilename, imageDimensions));
        }
         

        /// <summary>
        /// Get the image of this texture
        /// </summary>
        /// <returns>Texture2D for this GameTexture</returns>
        public Texture2D getTexture()
        {
            return texture_;
        }

        /// <summary>
        /// Set the image of this texture
        /// </summary>
        /// <param name="tex">Texture2D for this GameTexture</param>
        public void setTexture(Texture2D tex)
        {
            texture_ = tex;
        }

        /// <summary>
        /// Get the imageDimensions for this GameTexture
        /// </summary>
        /// <returns>Array of Rectangles representing each image's dimension</returns>
        public Rectangle[] getImageDimensions()
        {
            return imageDimensions_;
        }

        /// <summary>
        /// Set the imageDimensions for this GameTexture
        /// </summary>
        /// <param name="dims">Array of Rectangles representing each image's dimension</param>
        public void setImageDimensions(Rectangle[] dims)
        {
            Array.Copy(dims, imageDimensions_, dims.GetLength(0));
        }

        /// <summary>
        /// Get the number of images in this Texture.
        /// </summary>
        /// <returns>number of images</returns>
        public int getNumberOfImages()
        {
            return imageDimensions_.GetLength(0);
        }

        /// <summary>
        /// Get the SpriteBatch for the game.
        /// </summary>
        /// <returns>The SpriteBatch</returns>
        public SpriteBatch getSpriteBatch()
        {
            return spriteBatch_;
        }

        /// <summary>
        /// Set the SpriteBatch for the game.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch</param>
        public void setSpriteBatch(SpriteBatch spriteBatch)
        {
            spriteBatch_ = spriteBatch;
        }

        internal TextureDrawer getDrawer(Vector2 position, float depth)
        {
            TextureDrawer def = new TextureDrawer(this, position, depth);
            //def.Origin =
            //    new Vector2(((float)imageDimensions_[def.ImageIndex].Width) / 2.0f, ((float)imageDimensions_[def.ImageIndex].Height) / 2.0f);
            return def;
        }
    }

    internal enum CoordinateTypeEnum
    {
        RELATIVE,
        ABSOLUTE
    }

    internal class TextureDrawer
    {
        internal GameTexture Texture { get; set; }
        internal Vector2 Position { get; set; }
        internal Rectangle Destination { get; set; }
        internal bool Dest { get; set; }
        internal float Rotation { get; set; }
        internal Vector2 Direction
        {
            get
            {
                return CommonFunctions.getVector(Rotation);
            }

            set
            {
                Rotation = CommonFunctions.getAngle(value);
            }
        }
        internal CoordinateTypeEnum CoordinateType { get; set; }
        internal float Depth { get; set; }
        internal Color Color { get; set; }
        internal int ImageIndex { get; set; }
        internal SpriteEffects Effects { get; set; }
        internal bool Centered { get; set; }
        internal float Scale { get; set; }

        internal TextureDrawer() 
        {
            Texture = null;
            Position = Vector2.Zero;
            Destination = Rectangle.Empty;
            Dest = false;
            Rotation = 0.0f;
            CoordinateType = CoordinateTypeEnum.RELATIVE;
            Depth = 0.0f;
            Color = Color.White;
            ImageIndex = 0;
            Effects = SpriteEffects.None;
            Centered = false;
            Scale = 0.0f;
        }

        internal TextureDrawer(GameTexture texture, Vector2 position, float depth)
        {
            Texture = texture;
            Position = position;
            Depth = depth;
            Rotation = 0;
            CoordinateType = CoordinateTypeEnum.RELATIVE;
            Color = Color.White;
            ImageIndex = 0;
            Effects = SpriteEffects.None;
            Scale = 1.0f;
            Centered = true;
        }
        /*
         * td.Texture = sprites_;
            td.ImageIndex = currentFrame_;
            td.Position = position_;
            td.Dest = false;
            td.CoordinateType = CoordinateTypeEnum.RELATIVE;
            td.Depth = depth_;
            td.Centered = true;
            td.Color = Color.White;
            td.Effects = SpriteEffects.None;
            td.Direction = rotation_;
            td.Scale = 1.0f;
         */

        internal void set(GameTexture texture,
                            int imageIndex,
                            Vector2 position,
                            CoordinateTypeEnum coordType,
                            float depth,
                            bool centered,
                            Color color,
                            float rotation,
                            float scale)
        {
            Texture = texture;
            Position = position;
            Dest = false;
            ImageIndex = imageIndex;
            CoordinateType = coordType;
            Depth = depth;
            Centered = centered;
            Effects = SpriteEffects.None;
            Color = color;
            Rotation = rotation;
            Scale = scale;
        }

        internal void set(GameTexture texture,
                            int imageIndex,
                            Rectangle destination,
                            CoordinateTypeEnum coordType,
                            float depth,
                            bool centered,
                            Color color,
                            float rotation,
                            float scale)
        {
            Texture = texture;
            Destination = destination;
            Dest = true;
            ImageIndex = imageIndex;
            CoordinateType = coordType;
            Depth = depth;
            Centered = centered;
            Effects = SpriteEffects.None;
            Color = color;
            Rotation = rotation;
            Scale = scale;
        }

        internal void set(GameTexture texture,
                            int imageIndex,
                            Vector2 position,
                            CoordinateTypeEnum coordType,
                            float depth,
                            bool centered,
                            Color color,
                            Vector2 direction,
                            float scale)
        {
            Texture = texture;
            Position = position;
            Dest = false;
            ImageIndex = imageIndex;
            CoordinateType = coordType;
            Depth = depth;
            Centered = centered;
            Effects = SpriteEffects.None;
            Color = color;
            Direction = direction;
            Scale = scale;
        }

        internal void set(GameTexture texture,
                            int imageIndex,
                            Rectangle destination,
                            CoordinateTypeEnum coordType,
                            float depth,
                            bool centered,
                            Color color,
                            Vector2 direction,
                            float scale)
        {
            Texture = texture;
            Destination = destination;
            Dest = true;
            ImageIndex = imageIndex;
            CoordinateType = coordType;
            Depth = depth;
            Centered = centered;
            Effects = SpriteEffects.None;
            Color = color;
            Direction = direction;
            Scale = scale;
        }

        internal void draw()
        {

        }

        internal Vector2 discretize(Vector2 v)
        {
            return new Vector2((int)v.X, (int)v.Y);
        }

        internal void draw(Vector2 camPosition)
        {
            if (this.CoordinateType == CoordinateTypeEnum.RELATIVE)
            {
                this.Position -= camPosition;
            }
            Vector2 origin = Vector2.Zero;
            if (Centered)
            {
                origin.X = (float)Texture.imageDimensions_[ImageIndex].Width / 2.0f;
                origin.Y = (float)Texture.imageDimensions_[ImageIndex].Height / 2.0f;
            }
            if(Dest)
            {
                GameTexture.spriteBatch_.Draw(
                                this.Texture.texture_,
                                this.Destination,
                                this.Texture.imageDimensions_[ImageIndex],
                                this.Color,
                                this.Rotation,
                                origin,
                                this.Effects,
                                1.0f - this.Depth); // we got the convention backwards
            }
            else
            {
                GameTexture.spriteBatch_.Draw(
                              this.Texture.texture_,
                              discretize(this.Position),
                              this.Texture.imageDimensions_[this.ImageIndex],
                              this.Color,
                              this.Rotation,
                              origin,
                              this.Scale,
                              this.Effects,
                              1.0f - this.Depth); // we got the convention backwards
            }
        }
    }
}
