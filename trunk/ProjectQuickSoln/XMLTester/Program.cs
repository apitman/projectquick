using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework;

using ProjectQuickGameLibrary;

namespace XMLTester
{
    /***
     * XMLTester is useful for testing the serialization of XML files.  Basically, create
     * any type of object from the GameLibrary and populate it how you like.  Then, run this
     * program on it and it will produce an XML file of that object.  From reading that, you
     * can determine how to write XML files which can be automatically deserialized into real
     * objects.
     **/
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                /*
                AnimationSet testData = new AnimationSet();
                testData.anims = new List<Animation>();
                Animation walk = new Animation("walk", 2, 1, true, 0, "idle");
                walk.frames[0] = new Frame();
                walk.frames[0].box = new Rectangle(0, 0, 23, 32);
                walk.frames[0].duration = 2;
                walk.frames[0].trigger = "";
                walk.frames[1] = new Frame();
                walk.frames[1].box = new Rectangle(23, 0, 23, 32);
                walk.frames[1].duration = 2;
                walk.frames[1].trigger = "";
                Animation run = new Animation("run", 2, 1, true, 0, "idle");
                run.frames[0] = new Frame();
                run.frames[0].box = new Rectangle(0, 33, 23, 32);
                run.frames[0].duration = 2;
                run.frames[0].trigger = "";
                run.frames[1] = new Frame();
                run.frames[1].box = new Rectangle(23, 33, 23, 32);
                run.frames[1].duration = 2;
                run.frames[1].trigger = "";
                testData.anims.Add(walk);
                testData.anims.Add(run);
                 * */

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create("test.xml", settings))
                {
                    //IntermediateSerializer.Serialize(writer, testData, null);
                }
            }
            catch (System.IO.FileNotFoundException fex)
            {
                System.Console.WriteLine(fex.StackTrace);
            }
        }
    }
}
