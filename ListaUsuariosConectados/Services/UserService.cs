using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ListaUsuariosConectados.Services
{
    internal class UserService
    {
        //Un server siempre lleva un listener y un tcpclient

        TcpListener server;
        List<TcpClient> clients = new List<TcpClient>();

        public UserService()
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 55555);
            server = new TcpListener(ipe);          
        }

        public void Iniciar()
        {
            if(!server.Server.Connected)
            {
                Thread hilo1 = new Thread(new ThreadStart(Escuchar));
                hilo1.Start();
            }
        }

        void Escuchar()
        {
            while (server.Server.IsBound)
            {
                var clientenuevo = server.AcceptTcpClient();
                clients.Add(clientenuevo);
            }
        }

        public void Detener()
        {
            server.Stop();
        }
    }
}

