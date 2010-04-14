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
    public class GlobalHelper
    {
        protected static GlobalHelper instance_ = null;

        protected Engine engine_;
        protected Camera currentCamera_;

        private GlobalHelper()
        {
            
        }

        public static GlobalHelper getInstance()
        {
            if (instance_ == null)
            {
                instance_ = new GlobalHelper();
            }
            return instance_;
        }

        public void setEngine(Engine engine)
        {
            engine_ = engine;
        }

        public Engine getEngine()
        {
            return engine_;
        }

        public void setCurrentCamera(Camera cam)
        {
            currentCamera_ = cam;
        }

        public Camera getCurrentCamera()
        {
            return currentCamera_;
        }

        public static T loadContent<T>(string assetPath)
        {
            return instance_.engine_.Content.Load<T>(assetPath);
        }
    }
}
