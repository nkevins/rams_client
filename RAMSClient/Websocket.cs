using Newtonsoft.Json;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMSClient
{
    public class Websocket : IObserver
    {
        private Socket _socket;
        private Aircraft _aircraft;

        public Websocket(Aircraft aircraft)
        {
            _aircraft = aircraft;
            _socket = IO.Socket("https://rams-nkevins.c9users.io");
            //_socket = IO.Socket("http://192.168.88.5:3000/");
        }

        public void DataUpdated()
        {
            _socket.Emit("update", JsonConvert.SerializeObject(_aircraft));
        }
    }
}
