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
    class Animation
    {
        Rectangle[] frames;
        float frameLength = 1f / 5f;
        float timer = 0f;
        int currentFrame = 0;

        public Animation(int width, int height, int numFrames, int xOffset, int yOffset)
        {
            frames = new Rectangle[numFrames];
            int frameWidth = width / numFrames;
            for (int i = 0; i < numFrames; i++)
            {
                frames[i] = new Rectangle(xOffset + (frameWidth * i), yOffset, frameWidth, height);
            }
        }
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameLength)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }
        public void Reset()
        {
            currentFrame = 0;
            timer = 0f;
        }



        public int FramesPerSecond
        {
            get
            {
                return (int)(1f / frameLength);
            }
            set
            {
                frameLength = 1f / (float)value;
            }
        }

        public Rectangle CurrentFrame
        {
            get
            {
                return frames[currentFrame];
            }
        }




    }
}