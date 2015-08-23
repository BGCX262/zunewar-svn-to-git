using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;

namespace ZUNE_WAR
{
    class Map
    {
       public Room[,] map;

        public Map()
        {
            map = new Room[4, 5];
        }
        public Map(string fn)
        {
            map = new Room[4, 5];
            FillMap(fn);
        }

        public void FillMap(string filename)
        {
            StreamReader sr = new StreamReader(Path.Combine(StorageContainer.TitleLocation, "lvl1.txt"));
           
            Random random = new Random();
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        int a = sr.Read();
                        int b = sr.Read();
                        int c = sr.Read();
                        int d = sr.Read();
                        map[x, y] = new Room();

                        map[x, y].EastWall = a;
                        map[x, y].NorthWall = b;
                        map[x, y].WestWall = c;
                        map[x, y].SouthWall = d;
                    }
                }
                sr.Close();

        }





    }
}