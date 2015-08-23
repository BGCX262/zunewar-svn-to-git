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
    class Tank : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D tankImage;
        SpriteBatch spriteBatch;
        Vector2 position;
        Vector2 globalPosition;
        Vector2 velocity;
        Vector2 mapScreen;
        public List<Bullet> bullets;
        public List<Bullet> bulletsOther;
        double rot;
        Int32 score;

        public double Rotation
        {
            get
            {
                return rot;
            }
            set
            {
                rot = value;
            }
        }

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
        public Vector2 GlobalPosition
        {
            get
            {
                return globalPosition;
            }
            set
            {
                globalPosition = value;
            }
        }
        public Vector2 MapScreen
        {
            get
            {
                return mapScreen;
            }
            set
            {
                mapScreen = value;
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
        public float GlobalPositionX
        {
            get
            {
                return globalPosition.X;
            }
            set
            {
                globalPosition.X = value;
            }
        }
        public float GlobalPositionY
        {
            get
            {
                return globalPosition.Y;
            }
            set
            {
                globalPosition.Y = value;
            }
        }
        public float MapScreenX
        {
            get
            {
                return mapScreen.X;
            }
            set
            {
                mapScreen.X = value;
            }
        }
        public float MapScreenY
        {
            get
            {
                return mapScreen.Y;
            }
            set
            {
                mapScreen.Y = value;
            }
        }

        public int Width
        {
            get
            {
                return this.tankImage.Width;
            }
        }
        public int Height
        {
            get
            {
                return this.tankImage.Height;
            }
        }
        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                score = value;
            }
        }

        public Tank(Game game)
            : base(game)
        {
            position = new Vector2(100.0f, 150.0f);
            globalPosition = new Vector2(100.0f, 150.0f);
            bullets = new List<Bullet>();
            bulletsOther = new List<Bullet>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            


            base.Update(gameTime);
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tankImage = Game.Content.Load<Texture2D>("tank1");
                       
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tankImage, new Rectangle((int)PositionX, (int)positionY, 32, 32), null, Color.White, (float)rot, new Vector2((float)(Width / 2), (float)(Height/ 2)), SpriteEffects.None, 0);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
        public void DrawGlobal(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tankImage, new Rectangle((int)globalPosition.X / 4, (int)(globalPosition.Y / 3.75), (int)32, (int)32), null, Color.White, (float)rot, new Vector2((float)(Width / 2), (float)(Height / 2)), SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }











    }
}