using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace ZUNE_WAR
{
    class Bullet : Microsoft.Xna.Framework.Game
    {
        Texture2D bulletImage;
        SpriteBatch spriteBatch;
        Vector2 position;
        Vector2 velocity;
        
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        public float PositionX
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }
        public float positionY
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }
        public int Width
        {
            get
            {
                return this.bulletImage.Width;
            }
        }
        public int Height
        {
            get
            {
                return this.bulletImage.Height;
            }
        }

        public Bullet()
        {
            position = new Vector2(100.0f, 150.0f);
            velocity = new Vector2(0.0f, 0.0f);
        }



















    }
}