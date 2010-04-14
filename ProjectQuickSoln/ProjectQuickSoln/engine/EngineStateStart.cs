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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Xml;

namespace QuestAdaptation
{
    /// <summary>
    /// EngineStateStart's purpose is to determine the active controller and
    /// prompt storage device selection.  On PC, the logic runs automatically
    /// (since PC supports only one player) and the screen never needs to be
    /// presented.
    /// </summary>
    class EngineStateStart : EngineStateInterface
    {
        protected const FontEnum TEXT_FONT = FontEnum.Kootenay14;
        protected readonly Color TEXT_COLOR = Color.White;
        protected const float TEXT_ROTATION = 0.0f;
        protected const float TEXT_DEPTH = Constants.DEPTH_MENU_TEXT;
        protected Vector2 TEXT_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                float topEmptySpace = logo_.imageDimensions_[0].Height;
                float y = r.Y + (r.Height - topEmptySpace) / 2 + topEmptySpace;
                return new Vector2(r.X + r.Width / 2.0f, y);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        protected Vector2 LOGO_POSITION
        {
            get
            {
                Rectangle r = engine_.GraphicsDevice.Viewport.TitleSafeArea;
                return new Vector2(r.X + (r.Width - logo_.getImageDimensions()[0].Width) / 2, r.Y);
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        protected const float LOGO_DEPTH = Constants.DEPTH_LOW;

        protected string TEXT_MESSAGE = "Press START to Continue";

        protected Engine engine_;
        protected GameTexture logo_;
        protected bool returnFlag_;

        /// <summary>
        /// Constructor determines whether PC or Xbox 360 and initializes
        /// variables accordingly.
        /// 
        /// For PC, this means creating the controller and trying to get a
        /// storage device.
        /// 
        /// For the Xbox 360, we don't need to do much as we rely on update()
        /// logic to determine active controller and storage device.
        /// </summary>
        /// <param name="engine"></param>
        public EngineStateStart(Engine engine)
        {
            engine_ = engine;
            //logo_ = TextureMap.fetchTexture(@"Sprites\TitleScreen");
            //logo_ = new GameTexture(@"Sprites\TitleScreen", engine.spriteBatch_, engine.GraphicsDevice, engine.Content);
            logo_ = new GameTexture(@"Sprites\TitleScreen");

#if !XBOX
            {
                Settings settings = Settings.getInstance();
                if (Settings.getInstance().IsUsingMouse_)
                {
                    engine_.Controls_ = new PCControllerInput(engine_);
                    settings.CurrentPlayer_ = PlayerIndex.One;
                    TEXT_MESSAGE = "Press " + engine_.Controls_.getControlName(InputsEnum.CONFIRM_BUTTON).ToUpper() + " to Continue";
                }
                else
                {
                    engine_.Controls_ = new X360ControllerInput(engine_, PlayerIndex.One);
                    settings.CurrentPlayer_ = PlayerIndex.One;
                    TEXT_MESSAGE = "Press " + engine_.Controls_.getControlName(InputsEnum.CONFIRM_BUTTON).ToUpper() + " to Continue";
                }
                prepareStorageDevice();
                returnFlag_ = true;
            }
#else
            {
                engine_.Controls_ = null; // reset controls if they are coming back to this
                returnFlag_ = false;
            }
#endif

        }

        /// <summary>
        /// Waits for input from a controller to determine the active controller.
        /// Also monitors flow of controller setup, profile setup, storage setup. 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public EngineStateInterface update(GameTime gameTime)
        {
            if (returnFlag_)
            {
                InputSet.getInstance().setAllToggles();
                EngineManager.ReplaceCurrent(new EngineStateGameplay(engine_));
                return this;
            }

            if (engine_.Controls_ == null && !returnFlag_)
            {
                prepareControls();
            }

            // TODO
            // Load profile information here?

            if (engine_.Controls_ != null && !returnFlag_)
            {
                prepareStorageDevice();
            }

            return this;
        }

        public void draw()
        {
            //engine_.GraphicsDevice.Clear(Color.Black);
            DrawBuffer.getInstance().getUpdateStack().ScreenClearColor_ = Color.Black;

            //logo_.drawImageAbsolute(0, LOGO_POSITION, LOGO_DEPTH);
            DrawStack stack = DrawBuffer.getInstance().getUpdateStack();
            TextureDrawer td = stack.getNext();
            td.set(logo_,
                    0,
                    LOGO_POSITION,
                    CoordinateTypeEnum.ABSOLUTE,
                    LOGO_DEPTH,
                    false,
                    Color.White,
                    0.0f,
                    1.0f);
            stack.push();

            GameFont myFont = FontMap.getInstance().getFont(TEXT_FONT);
            myFont.drawStringCentered(TEXT_MESSAGE,
                                        TEXT_POSITION,
                                        TEXT_COLOR,
                                        TEXT_ROTATION,
                                        TEXT_DEPTH);
        }

        /// <summary>
        /// Waits for any one of four controllers to press Start or A, then sets that
        /// controller as the active player.
        /// </summary>
        private void prepareControls()
        {
            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                GamePadState gps = GamePad.GetState(index);
                if (gps.IsButtonDown(Buttons.Start) || gps.IsButtonDown(Buttons.A))
                {
                    Settings.getInstance().CurrentPlayer_ = index;
                    engine_.Controls_ = new X360ControllerInput(engine_, index);
                    break;
                }
            }
        }

        /// <summary>
        /// If GamerServices is available, opens up the Guide so the user can select
        /// a storage device.  Otherwise, it sets a flag to quit and the storage
        /// will be determined later (PC).
        /// </summary>
        private void prepareStorageDevice()
        {
            Settings settings = Settings.getInstance();
            if (settings.IsGamerServicesAllowed_ && !Guide.IsVisible)
            {
                try
                {
                    //IAsyncResult result =
                    //    Guide.BeginShowStorageDeviceSelector(fetchStorageDevice, "selectStorage");
                }
                catch (GuideAlreadyVisibleException)
                {
                    // No code here, but this catch block is needed
                    // because !Guide.IsVisible doesn't always work
                }
            }
            else
            {
                settings.StorageDevice_ = null;
                returnFlag_ = true;
            }
        }

        /// <summary>
        /// Asynchronous call to run once the user selects a storage device from the
        /// Guide menu.  Determines next state based on user's decision.
        /// </summary>
        /// <param name="result"></param>
        private void fetchStorageDevice(IAsyncResult result)
        {
            StorageDevice storageDevice = Guide.EndShowStorageDeviceSelector(result);

            // User selected a device, so we set it and get ready to quit
            if (storageDevice != null)
            {
                Settings.getInstance().StorageDevice_ = storageDevice;
                returnFlag_ = true;
            }

            // User cancelled, so we remove active controller and wait again
            else
            {
                engine_.Controls_ = null;
                returnFlag_ = false;
            }

            Settings.getInstance().loadSettingsFromFile();
        }

    }
}
