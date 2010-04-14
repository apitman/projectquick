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

namespace QuestAdaptation
{
    public class Constants
    {
        public const float DEPTH_GROUND = 0.1f;
        public const float DEPTH_LOW = 0.2f;
        public const float DEPTH_HIGH = 0.3f;
        public const float DEPTH_LASER = 0.31f;
        public const float DEPTH_MENU_TEXT = 0.5f;
        public const float DEPTH_HUD = 0.9f;
        public const float DEPTH_HUD_TEXT = 0.91f;
        public const float DEPTH_DEBUG_LINES = 1.0f;
        public const float DEPTH_OUT_OF_FOCUS_OVERLAY = 0.99f;
        public const float DEPTH_OUT_OF_FOCUS_TEXT = 1.0f;

        public const float DEPTH_DIALOGUE_PAGE = .95f;
        public const float DEPTH_DIALOGUE_TEXT = .951f;

        public const int MIN_NUM_TILES_X = 0;
        public const int MIN_NUM_TILES_Y = 0;
        public const int MAX_NUM_TILES_X = 100;
        public const int MAX_NUM_TILES_Y = 100;
    }
}
