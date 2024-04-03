using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knoknok
{
    public static class Info
    {
        private static byte id;
        private static readonly int port = 11000;

        public static byte ID => id;
        public static int Port => port;

        public static void Start()
        {
            //get id
            Random rnd = new Random();
            id = (byte)rnd.Next(1, 255);
        }
    }
}
