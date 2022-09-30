using ListaUsuariosConectados.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        TcpListener? server;
        List<TcpClient> clients = new List<TcpClient>();

        public void Iniciar()
        {
            if (server == null)
            {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 55555);
                server = new TcpListener(ipe);//Bind, separa el puerto    

                Thread hilo1 = new Thread(new ThreadStart(Escuchar));
                hilo1.Start();
            }
        }

        void Escuchar()
        {
            if (server != null)
            {
                server.Start();
                while (server.Server.IsBound)
                {
                    var clientenuevo = server.AcceptTcpClient();
                    clients.Add(clientenuevo);
                    Thread HiloRecibir = new Thread(new ParameterizedThreadStart(Recibir));
                    HiloRecibir.Start(clientenuevo);
                }
            }
        }

        public event Action<Usuario>? UsuarioConectado;

        void Enviar(TcpClient cliente, byte[] buffer)
        {

        }

        void Recibir(object? tcpclient)
        {
            if (tcpclient != null)
            {
                TcpClient clienteconectado = (TcpClient)tcpclient;

                var stream = clienteconectado.GetStream();

                while (clienteconectado.Connected)
                {
                    if (clienteconectado.Available > 0)//Si hay algun byte disponible, lo leo
                    {
                        byte[] buffer = new byte[clienteconectado.Available];
                        stream.Read(buffer, 0, buffer.Length);

                        clients.ForEach(x =>
                        {
                            if (x != clienteconectado)
                            {
                                Enviar(x, buffer);
                            }
                        });

                        var usuario = JsonConvert.DeserializeObject<Usuario>(
                           Encoding.UTF8.GetString(buffer)
                            );

                        if (usuario != null)
                            UsuarioConectado?.Invoke(usuario);

                    }

                    Thread.Sleep(1000);
                }
            }


        }

        public void Detener()
        {
            if (server != null)
            {
                server.Stop();
                server = null;
            }

        }
    }
}

