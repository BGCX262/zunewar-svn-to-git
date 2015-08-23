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
    class AnimationSprite
    {
        public Vector2 position = Vector2.Zero;
        public Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        Texture2D texture;
        string currentAnimation;
        bool updateAnimation;

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        public string CurrentAnimation
        {
            get
            {
                return currentAnimation;
            }
            set
            {
                if (!animations.ContainsKey(value))
                    throw new Exception("Invalid animation specified");
                if (currentAnimation == null || !currentAnimation.Equals(value))
                {
                    currentAnimation = value;
                    animations[currentAnimation].Reset();
                }
            }
        }

        public void StartAnimation()
        {
            updateAnimation = true;
        }
        public void StopAnimation()
        {
            updateAnimation = false;
        }

        public void Update(GameTime gameTime)
        {
            if (currentAnimation == null)
            {
                if (animations.Keys.Count == 0)
                    return;
                string[] keys = new string[animations.Keys.Count];

                animations.Keys.CopyTo(keys, 0);
                currentAnimation = keys[0];
            }
            if (updateAnimation)
            {
                animations[currentAnimation].Update(gameTime);
            }
        }

    }
}