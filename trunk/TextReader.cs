using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ZUNE_WAR
{
    class TextReader
    {
        StreamReader streamReader;  
        public TextReader()
        {
        }
        public TextReader(string filename)
        {
            streamReader = File.OpenText(filename);
            string s = streamReader.ReadLine();
        }

    }
}