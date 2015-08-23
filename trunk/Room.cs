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

namespace ZUNE_WAR
{
    class Room
    {
        int northWall;
        int southWall;
        int eastWall;
        int westWall;

        public Room()
        {
            northWall = southWall = eastWall = westWall = 0;
        }

        public Room(int east, int north, int west, int south)
        {
            eastWall = east;
            northWall = north;
            westWall = west;
            southWall = south;
        }

        public int NorthWall
        {
            get
            {
                return northWall;
            }
            set
            {
                northWall = value;
            }
        }
        public int SouthWall
        {
            get
            {
                return southWall;
            }
            set
            {
                southWall = value;
            }
        }
        public int EastWall
        {
            get
            {
                return eastWall;
            }

            set
            {
                eastWall = value;
            }
        }
        public int WestWall
        {
            get
            {
                return westWall;
            }
            set
            {
                westWall = value;
            }
        }
    }
}