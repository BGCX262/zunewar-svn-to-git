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
    class Network
    {
        PacketReader packetReader;
        PacketWriter packetWriter;
        NetworkSession session;

        public NetworkSession Session
        {
            get
            {
                return session;
            }
            set
            {
                session = value;
            }
        }

        public PacketReader PacketReader
        {
            get
            {
                return packetReader;
            }
            set
            {
                packetReader = value;
            }
        }

        public PacketWriter PacketWriter
        {
            get
            {
                return packetWriter;
            }
            set
            {
                packetWriter = value;
            }
        }



        public Network()
        {
            packetReader = new PacketReader();
            packetWriter = new PacketWriter();
        }

    }
}