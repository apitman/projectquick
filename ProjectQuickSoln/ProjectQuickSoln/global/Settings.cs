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
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;
using System.Xml;
using System.IO;

namespace QuestAdaptation
{
    /// <summary>
    /// Singleton for control and game settings.
    /// </summary>
    public class Settings
    {
        protected const string DEFAULT_SETTINGS = @"XML\defaultsettings";
        protected const string SETTINGS_FOLDER = "settings";
        protected const string SETTINGS_FILE = "settings.xml";

        protected static Settings instance_;

        protected MovementType movementType_;

        internal PlayerIndex CurrentPlayer_ { get; set; }

        internal bool IsUsingMouse_ { get; set; }

        internal bool IsInDebugMode_ { get; set; }

        internal bool IsGamerServicesAllowed_ { get; set; }

        protected bool isSoundAllowed_;
        internal bool IsSoundAllowed_
        {
            get
            {
                return isSoundAllowed_;
            }

            set
            {
                isSoundAllowed_ = value;
                if (value)
                    SoundEngine.getInstance().changeAllVolume(1.0f);
                else
                    SoundEngine.getInstance().changeAllVolume(0.0f);
            }
        }

        internal Engine EngineHandle_ { get; set; }

        internal protected Resolution resolution_;
        internal Resolution Resolution_
        {
            get
            {
                return resolution_;
            }

            set
            {
                resolution_ = value;
                switch (value)
                {
                    case Resolution.auto:
                        EngineHandle_.initializeScreen();
                        break;
                    case Resolution.s640x480:
                        EngineHandle_.setScreenSize(640, 480);
                        break;
                    case Resolution.s800x600:
                        EngineHandle_.setScreenSize(800, 600);
                        break;
                    case Resolution.s1024x768:
                        EngineHandle_.setScreenSize(1024, 768);
                        break;
                    case Resolution.s1152x864:
                        EngineHandle_.setScreenSize(1152, 864);
                        break;
                    case Resolution.h1280x720:
                        EngineHandle_.setScreenSize(1280, 720);
                        break;
                }
            }
        }

        private StorageDevice storageDevice_;
        internal StorageDevice StorageDevice_
        {
            get
            {
                return storageDevice_;
            }
            set
            {
                ContainerManager.cleanupContainer();
                storageDevice_ = value;
            }
        }

        internal static void initialize(Engine engine_)
        {
            instance_ = new Settings();
            instance_.EngineHandle_ = engine_;
            instance_.movementType_ = MovementType.ABSOLUTE;
            instance_.IsInDebugMode_ = false;
            instance_.IsSoundAllowed_ = true;
            instance_.Resolution_ = Resolution.auto;
        }

        private Settings()
        {
            // Singleton
        }

        public static Settings getInstance()
        {
            return instance_;
        }

        /// <summary>
        /// Change movement type between relative and absolute.
        /// </summary>
        public void swapMovementType()
        {
            if (movementType_ == MovementType.ABSOLUTE)
            {
                movementType_ = MovementType.RELATIVE;
            }
            else
            {
                movementType_ = MovementType.ABSOLUTE;
            }
        }

        /// <summary>
        /// Get whether movement should be relative to character
        /// direction or absolute.
        /// </summary>
        /// <returns>Type of movement</returns>
        public MovementType getMovementType()
        {
            return movementType_;
        }

        /// <summary>
        /// Pulls settings from an XmlDocument
        /// </summary>
        /// <param name="doc">XmlDocument containing the appropriate commando-settings tag</param>
        protected void pullSettings(XmlDocument doc)
        {
            XmlNode root = doc.ChildNodes[1]; // index 0 is XML declaration
            if (root.Name != "commando-settings")
            {
                throw new XmlException("commando-settings missing from settings file");
            }

            XmlNodeList settings = root.ChildNodes;
            for (int i = 0; i < settings.Count; i++)
            {
                XmlNode cur = settings[i];
                switch (cur.Name)
                {
                    case "resolution":
                        Resolution_ = (Resolution)Convert.ToInt32(cur.InnerText);
                        break;
                    case "movement":
                        movementType_ = (MovementType)Convert.ToInt32(cur.InnerText);
                        break;
                    case "sound":
                        IsSoundAllowed_ = Convert.ToBoolean(cur.InnerText);
                        break;
                    case "debug":
                        IsInDebugMode_ = Convert.ToBoolean(cur.InnerText);
                        break;
                }
            }
        }

        /// <summary>
        /// Pushes current settings to an XmlDocument
        /// </summary>
        /// <returns>An XmlDocument containing the current settings</returns>
        protected XmlDocument pushSettings()
        {
            XmlDocument doc = new XmlDocument();

            XmlNode declaration = doc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            doc.AppendChild(declaration);

            XmlElement root = doc.CreateElement("commando-settings");
            doc.AppendChild(root);

            XmlElement res = doc.CreateElement("resolution");
            res.InnerText = Convert.ToString((int)resolution_);
            XmlElement movement = doc.CreateElement("movement");
            movement.InnerText = Convert.ToString((int)movementType_);
            XmlElement sound = doc.CreateElement("sound");
            sound.InnerText = Convert.ToString(IsSoundAllowed_);
            XmlElement debug = doc.CreateElement("debug");
            debug.InnerText = Convert.ToString(IsInDebugMode_);
            root.AppendChild(res);
            root.AppendChild(movement);
            root.AppendChild(sound);
            root.AppendChild(debug);

            return doc;
        }

        /// <summary>
        /// Copies the current settings into a local file
        /// </summary>
        internal void saveSettingsToFile()
        {
            XmlDocument doc = pushSettings();
        
            // No storage device, so we'll store/load the settings in a directory
            //  with the executable
            if (storageDevice_ == null)
            {
                const string folderpath = @".\" + SETTINGS_FOLDER;
                Directory.CreateDirectory(folderpath);
                const string filepath = folderpath + @"\" + SETTINGS_FILE;
                doc.Save(filepath);
            }

            // We have a storage device, so we'll store the settings in the associated
            //  container
            else
            {
                ContainerManager.cleanupContainer();
                StorageContainer sc = ContainerManager.getOpenContainer();
                string folderpath = Path.Combine(sc.Path, SETTINGS_FOLDER);
                Directory.CreateDirectory(folderpath);
                string filepath = Path.Combine(folderpath, SETTINGS_FILE);
                doc.Save(filepath);
            }
        
        }

        internal void loadSettingsFromFile()
        {
            try
            {
                // No storage device, so we'll store/load the settings in a directory
                //  with the executable
                if (storageDevice_ == null)
                {
                    const string filepath = @".\" + SETTINGS_FOLDER + @"\" + SETTINGS_FILE;
                    using (ManagedXml manager = new ManagedXml(EngineHandle_))
                    {
                        XmlDocument doc = manager.loadFromFile(filepath);
                        pullSettings(doc);
                    }
                }

                // We have a storage device, so we'll store the settings in the associated
                //  container
                else
                {
                    StorageContainer sc = ContainerManager.getOpenContainer();
                    string folderpath = Path.Combine(sc.Path, SETTINGS_FOLDER);
                    string filepath = Path.Combine(folderpath, SETTINGS_FILE);
                    using (ManagedXml manager = new ManagedXml(EngineHandle_))
                    {
                        XmlDocument doc = manager.loadFromFile(filepath);
                        pullSettings(doc);
                    }
                }
            }
            catch
            {
                using (ManagedXml manager = new ManagedXml(EngineHandle_))
                {
                    XmlDocument doc = manager.load(DEFAULT_SETTINGS);
                    pullSettings(doc);
                }
            }
        }
    }

    public enum MovementType
    {
        RELATIVE,
        ABSOLUTE
    }

    public enum Resolution
    {
        auto,
        s640x480,
        s800x600,
        s1024x768,
        s1152x864,
        h1280x720,
        LENGTH
    }
}
