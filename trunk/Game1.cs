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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Network network = new Network();
        Input input = new Input();
        Map map = new Map("lvl1.txt");
        bool WorldView = false;
        bool buttonADown = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int maxGamers = 2;
        int maxLocalGamers = 1;
        Rectangle boundRect = new Rectangle(0, 0, 1, 1);

        Texture2D enemyTexture;
        Texture2D boundingBox;
        Vector2 enemyPosition = Vector2.Zero;
        Vector2 enemyVelocity = Vector2.Zero;

        Texture2D backgroundGrey;
        Texture2D map1;

        bool hostFireShot = false;
        Vector2 hostBulletPosition = Vector2.Zero;
        Vector2 hostBulletVelocity = Vector2.Zero;

        Texture2D bulletImage;
        SpriteFont font1;

        Texture2D light_horizontal;
        Texture2D light_vertical;

        int shotCount = 0;  // 3 shots per second
        float timer = 0.0f;
        float interval = 1000.0f;
        float singleShotInterval = 200.0f;
        float singleShotTimer = 0.0f;
        int totalShotsFired = 0;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Components.Add(new GamerServicesComponent(this));
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            enemyTexture = Content.Load<Texture2D>("tank2");
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            bulletImage = Content.Load<Texture2D>("bullet");
            boundingBox = Content.Load<Texture2D>("boundingBox");
            backgroundGrey = Content.Load<Texture2D>("background_grey");
            map1 = Content.Load<Texture2D>("map1");
            light_horizontal = Content.Load<Texture2D>("light_horiz");
            light_vertical = Content.Load<Texture2D>("light_vertical");
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if(input.IsButtonDown(Buttons.Back))
                this.Exit();

            if (network.Session == null)
            {
                if (input.IsButtonDown(Buttons.DPadRight))
                {
                    CreateNetworkSession();
                }
                if (input.IsButtonDown(Buttons.DPadLeft))  // selected to join game
                {
                    JoinNetworkSession();
                }
            }
            else
            {
                foreach (LocalNetworkGamer gamer in network.Session.LocalGamers)
                {
                    Tank t = gamer.Tag as Tank;
                    UpdateLocalGamer(gamer, t, gameTime);
                }

                network.Session.Update();
                foreach (LocalNetworkGamer gamer in network.Session.LocalGamers)
                {
                    ServerReadInputFromClients(gamer);

                }

            }
            base.Update(gameTime);
        }

        void ServerReadInputFromClients(LocalNetworkGamer gamer)
        {
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;
                gamer.ReceiveData(network.PacketReader, out sender);

                if (!sender.IsLocal)
                {
                    Tank t = sender.Tag as Tank;
                    bool shotFired = network.PacketReader.ReadBoolean();
                    if (shotFired)
                    {
                        Bullet b = new Bullet();
                        b.Position = network.PacketReader.ReadVector2();
                        b.Velocity = network.PacketReader.ReadVector2();
                        t.bullets.Add(b);
                    }

                    t.Position = network.PacketReader.ReadVector2();
                    t.Rotation = network.PacketReader.ReadDouble();
                    t.Score = network.PacketReader.ReadInt32();
                    t.MapScreenX = network.PacketReader.ReadInt32();
                    t.MapScreenY = network.PacketReader.ReadInt32();
                }
            }
        }

        void ClientReadGameStateFromServer(LocalNetworkGamer gamer)
        {
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;
                gamer.ReceiveData(network.PacketReader, out sender);
                foreach (NetworkGamer remoteGamer in network.Session.AllGamers)
                {
                    Tank t = remoteGamer.Tag as Tank;
                    t.Position = network.PacketReader.ReadVector2();
                    t.Rotation = network.PacketReader.ReadDouble();
                    bool a = network.PacketReader.ReadBoolean();
                    if (a)
                    {
                        Bullet b = new Bullet();
                        b.Position = network.PacketReader.ReadVector2();
                        b.Velocity = network.PacketReader.ReadVector2();
                        t.bullets.Add(b);
                    }
                    Bullet c = new Bullet();
                }
            }
        }

        void UpdateLocalGamer(LocalNetworkGamer gamer, Tank t, GameTime gameTime)
        {
            hostFireShot = false;
            timer = timer + (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            singleShotTimer = singleShotTimer + (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                shotCount = 0;
                timer = 0;
            }
            bool shotFired = false;
            Bullet bul = null;
            t.Velocity = Vector2.Zero;
            if (input.IsButtonDown(Buttons.A))
            {
                buttonADown = true;
            }
            if (!input.IsButtonDown(Buttons.A))
            {
                buttonADown = false;
            }


            // east
            if (input.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                // north east
                if (input.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    t.Velocity = new Vector2(3.0f, -3.0f);
                    t.Rotation = -(Math.PI / 4);
                }
                // south east
                else if (input.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    t.Velocity = new Vector2(3.0f, 3.0f);
                    t.Rotation = (Math.PI / 4);
                }
                // east
                else
                {
                    t.Velocity = new Vector2(3.0f, 0.0f);
                    t.Rotation = 0;
                }
            }
            // west
            if (input.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                // north west
                if (input.IsButtonDown(Buttons.LeftThumbstickUp))
                {
                    t.Velocity = new Vector2(-3.0f, -3.0f);
                    t.Rotation = -(3 * Math.PI / 4);

                }
                // south west
                else if (input.IsButtonDown(Buttons.LeftThumbstickDown))
                {
                    t.Velocity = new Vector2(-3.0f, 3.0f);
                    t.Rotation = (3 * Math.PI / 4);
                }
                // west
                else
                {
                    t.Velocity = new Vector2(-3.0f, 0.0f);
                    t.Rotation = Math.PI;
                }
            }
            if (input.IsButtonDown(Buttons.LeftThumbstickDown) && (!input.IsButtonDown(Buttons.LeftThumbstickLeft)) && (!input.IsButtonDown(Buttons.LeftThumbstickRight)))
            {
                t.Velocity = new Vector2(0.0f, 3.0f);
                t.Rotation = Math.PI / 2;
            }
            if (input.IsButtonDown(Buttons.LeftThumbstickUp) && (!input.IsButtonDown(Buttons.LeftThumbstickLeft)) && (!input.IsButtonDown(Buttons.LeftThumbstickRight)))
            {
                t.Velocity = new Vector2(0.0f, -3.0f);
                t.Rotation = -Math.PI / 2;
            }

            if (input.IsButtonDown(Buttons.B))
            {
                if (singleShotTimer > singleShotInterval && shotCount <= 2)
                {
                    shotFired = true;
                    double a = Math.Cos(t.Rotation);
                    double c = Math.Sin(t.Rotation);
                    Bullet b = new Bullet();
                    b.Position = t.Position;
                    b.Velocity = new Vector2((float)(3 * a), (float)(3 * c));
                    b.Position += 5 * b.Velocity;
                    t.bullets.Add(b);
                    bul = b;
                    singleShotTimer = 0;
                    shotCount++;
                    totalShotsFired++;
                    hostFireShot = true;
                    hostBulletPosition = b.Position;
                    hostBulletVelocity = b.Velocity;
                }
            }
            foreach (Bullet b in t.bullets)
            {
                b.Position += b.Velocity;
            }


            foreach (NetworkGamer g in network.Session.AllGamers)
            {
                if (!g.IsLocal)
                {
                    Tank t2 = g.Tag as Tank;
                    foreach (Bullet b in t2.bullets)
                    {
                        b.Position += b.Velocity;
                    }
                }
            }
            Vector2 newPos = t.Position + t.Velocity;


            bool changePosX = true;
            bool changePosY = true;

            if (t.PositionX <= 26)
            {
                if (map.map[(int)t.MapScreenX, (int)t.MapScreenY].WestWall == 49)
                {
                    changePosX = false;
                    t.PositionX = 27;
                }
            }
            if (t.PositionX >= 240 - 26)
            {
                if (map.map[(int)t.MapScreenX, (int)t.MapScreenY].EastWall == 49)
                {
                    changePosX = false;
                    t.PositionX = 240 - 27;
                }
            }
            if (t.positionY <= 26)
            {
                if (map.map[(int)t.MapScreenX, (int)t.MapScreenY].NorthWall == 49)
                {
                    changePosY = false;
                    t.positionY = 27;
                }
            }
            if (t.positionY >= 240 - 27)
            {
                if (map.map[(int)t.MapScreenX, (int)t.MapScreenY].SouthWall == 49)
                {
                    changePosY = false;
                    t.positionY = 240 - 28;
                }
            }
            if (changePosX)
                t.PositionX = t.PositionX + t.Velocity.X;
            if (changePosY)
                t.positionY = t.positionY + t.Velocity.Y;



            if ((t.Position.X > (240 - 16)))  // on the right edge of the screen
            {
                t.PositionX = 16;
                t.GlobalPositionX = t.GlobalPositionX + 16;
                t.MapScreenX++;
            }
            if ((t.Position.X < (16)))  // on the left edge of the screen
            {
                t.PositionX = 240 - 16;
                t.GlobalPositionX = t.GlobalPositionX - 16;
                t.MapScreenX--;
            }
            if ((t.Position.Y > (240 - 16)))  // on the bottom edge of the screen
            {
                t.positionY = 16;
                t.GlobalPositionY = t.GlobalPositionY + 16;
                t.MapScreenY++;
            }
            if ((t.positionY < 16))
            {
                t.positionY = 240 - 16;
                t.GlobalPositionY = t.GlobalPositionY - 16;
                t.MapScreenY--;
            }


            t.GlobalPosition = t.GlobalPosition + t.Velocity;



            Bullet bulletToDelete = new Bullet();
            bool bulletHit = false;
            foreach (NetworkGamer gamer2 in network.Session.AllGamers)
            {
                if (!gamer2.IsLocal)
                {
                    Tank gamerTank = gamer2.Tag as Tank;
                    Rectangle tankRect = new Rectangle((int)gamerTank.PositionX, (int)gamerTank.positionY, 32, 32);

                    boundRect = new Rectangle((int)gamerTank.PositionX - 16, (int)gamerTank.positionY - 16, 32, 32);

                    foreach (Bullet b in t.bullets)
                    {
                        Rectangle bulletRect = new Rectangle((int)b.PositionX, (int)b.positionY, 5, 5);
                        if (tankRect.Intersects(bulletRect) && !bulletHit)
                        {
                            b.PositionX = b.positionY = 400;
                            bulletToDelete = b;
                            t.Score++;
                            bulletHit = true;
                        }
                    }
                }
            }


            t.bullets.Remove(bulletToDelete);






            if (buttonADown)
            {
                WorldView = true;
            }
            else
            {
                WorldView = false;
            }

            network.PacketWriter.Write(shotFired);
            if (shotFired)
            {
                network.PacketWriter.Write(bul.Position);
                network.PacketWriter.Write(bul.Velocity);
            }
            network.PacketWriter.Write(t.Position);
            network.PacketWriter.Write(t.Rotation);
            network.PacketWriter.Write(t.Score);
            network.PacketWriter.Write(t.MapScreenX);
            network.PacketWriter.Write(t.MapScreenY);
            gamer.SendData(network.PacketWriter, SendDataOptions.InOrder);



        }

        void JoinNetworkSession()
        {

            // search for all sessions
            using (AvailableNetworkSessionCollection availableSessions = NetworkSession.Find(NetworkSessionType.SystemLink, maxLocalGamers, null))
            {
                if (availableSessions.Count == 0)
                {
                    DrawMessage("No network sessions found");
                    return;
                }
                network.Session = NetworkSession.Join(availableSessions[0]);
                HookNetworkSessionEvents();
            }
        }

        void CreateNetworkSession()
        {
            network.Session = NetworkSession.Create(NetworkSessionType.SystemLink, maxLocalGamers, maxGamers);
            HookNetworkSessionEvents();
        }
        void HookNetworkSessionEvents()
        {
            network.Session.GamerJoined += GamerJoinedEventHandler;
            network.Session.SessionEnded += SessionEndedEventHandler;
        }
        protected override void Draw(GameTime gameTime)
        {
            if (network.Session == null)
            {
                //no game started yet, in lobby
                graphics.GraphicsDevice.Clear(Color.WhiteSmoke);
                DrawMessage("R - CreateSession\n" + "L - JoinSession");
            }
            else
            {
                Tank localTank = network.Session.LocalGamers[0].Tag as Tank;
                graphics.GraphicsDevice.Clear(Color.WhiteSmoke);
                if (!WorldView)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(map1, new Rectangle(0, 0, 240, 240), new Rectangle((int)localTank.MapScreenX * 240, (int)localTank.MapScreenY * 240, 240, 240), Color.White);
                   // spriteBatch.DrawString(font1, "GP: " + localTank.GlobalPosition.ToString(), new Vector2(10, 100), Color.AntiqueWhite);
                   // spriteBatch.DrawString(font1, "LP: " + localTank.Position.ToString(), new Vector2(10, 140), Color.AntiqueWhite);
                    if (map.map[(int)localTank.MapScreenX, (int)localTank.MapScreenY].EastWall == 49)
                    {
                        spriteBatch.Draw(light_horizontal, new Rectangle(230, 0, 10, 240), new Rectangle(0, 0, 10, 240), Color.White);
                    }
                    if (map.map[(int)localTank.MapScreenX, (int)localTank.MapScreenY].NorthWall == 49)
                    {
                        spriteBatch.Draw(light_vertical, new Rectangle(0, 0, 240, 10), new Rectangle(0, 0, 240, 10), Color.White);
                    }
                    if (map.map[(int)localTank.MapScreenX, (int)localTank.MapScreenY].SouthWall == 49)
                    {
                        spriteBatch.Draw(light_vertical, new Rectangle(0, 230, 240, 10), new Rectangle(0, 0, 240, 10), Color.White);
                    }
                    if (map.map[(int)localTank.MapScreenX, (int)localTank.MapScreenY].WestWall == 49)
                    {
                        spriteBatch.Draw(light_horizontal, new Rectangle(0, 0, 10, 240), new Rectangle(0, 0, 10, 240), Color.White);
                    }
                    
                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(map1, new Rectangle(0, 0, 240, 320), null, Color.White);
                    for (int y = 0; y < 5; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            if (map.map[x, y].WestWall == 49)
                            {
                                //64 pixels tall
                                spriteBatch.Draw(light_horizontal, new Rectangle(x * 64, y * 64, 2, 64), new Rectangle(0, 0, 10, 240), Color.White);
                            }
                            if (map.map[x, y].EastWall == 49)
                            {
                                spriteBatch.Draw(light_horizontal, new Rectangle(x * 64 + 62, y * 64, 2, 64), new Rectangle(0, 0, 10, 240), Color.White);
                            }
                            if (map.map[x, y].NorthWall == 49)
                            {
                                spriteBatch.Draw(light_vertical, new Rectangle(x * 64, y * 64, 64, 2), new Rectangle(0, 0, 240, 10), Color.White);
                            }
                            if (map.map[x, y].SouthWall == 49)
                            {
                                spriteBatch.Draw(light_vertical, new Rectangle(x * 64, y * 64 + 62, 64, 2), new Rectangle(0, 0, 240, 10), Color.White);
                            }
                        }
                    }


                    localTank.DrawGlobal(gameTime);
                    spriteBatch.End();
                }
                DrawNetworkedPlayers(gameTime);
                spriteBatch.Begin();
                foreach (NetworkGamer networkGamer in network.Session.AllGamers)
                {
                    Tank t = networkGamer.Tag as Tank;
                    foreach (Bullet b in t.bullets)
                    {
                        spriteBatch.Draw(bulletImage, b.Position, Color.White);
                    }
                }
               // spriteBatch.Draw(boundingBox, boundRect, Color.White);
                spriteBatch.End();
            }
        }


        void DrawMessage(string message)
        {
            spriteBatch.Begin();
            graphics.GraphicsDevice.Clear(Color.WhiteSmoke);


            spriteBatch.DrawString(font1, message, new Vector2(10, 10), Color.Black);

            spriteBatch.End();
        }

        void DrawNetworkedPlayers(GameTime gameTime)
        {
            spriteBatch.Begin();

            Tank localTank = network.Session.LocalGamers[0].Tag as Tank;
            foreach (NetworkGamer gamer in network.Session.AllGamers)
            {
                Tank t = gamer.Tag as Tank;
                if (!WorldView)
                {   if((t.MapScreenX == localTank.MapScreenX) && (t.MapScreenY == localTank.MapScreenY))
 
                        t.Draw(gameTime);
                }
                else
                {
                        t.DrawGlobal(gameTime);
                }


            }
            // spriteBatch.DrawString(font1, "Score: " + score, new Vector2(10, 10), Color.Red);
            spriteBatch.End();



            base.Draw(gameTime);
        }

        void GamerJoinedEventHandler(object sender, GamerJoinedEventArgs e)
        {
            //new gamer, add tank object                           
            e.Gamer.Tag = new Tank(this);
            Components.Add(e.Gamer.Tag as Tank);
        }

        void SessionEndedEventHandler(object sender, NetworkSessionEndedEventArgs e)
        {
            network.Session.Dispose();
            network.Session = null;
        }



    }
}
