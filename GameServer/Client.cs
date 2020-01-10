using System;
using System.Net.Sockets;

namespace GameServer
{
    public class Client
    {
        public int connectionId;
        public string ip;
        public TcpClient socket;
        public NetworkStream stream;
        private static int bufferSize = 4096;
        private byte[] readBuffer;

        public void Start()
        {
            socket.SendBufferSize = bufferSize;
            socket.ReceiveBufferSize = bufferSize;
            stream = socket.GetStream();
            readBuffer = new byte[bufferSize];
            stream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, ReceiveDataCallback, null);
        }

        private void ReceiveDataCallback(IAsyncResult result)
        {
            try
            {
                int readbytes = stream.EndRead(result);
                if (readbytes <= 0) 
                {
                    return;
                }
                byte[] newBytes = new byte[readbytes];

                // copy array of bytes in to new array
                Buffer.BlockCopy(readBuffer, 0, newBytes, 0, readbytes);

                // begin the next read
                stream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, ReceiveDataCallback, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}