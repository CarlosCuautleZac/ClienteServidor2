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
    public class UserService
    {
        //Un server siempre lleva un listener y un tcpclient

        TcpListener? server;
        //List<TcpClient> clients = new List<TcpClient>();
        Dictionary<TcpClient, Usuario?> clients = new Dictionary<TcpClient, Usuario?>();

        public void Iniciar()
        {
            if (server == null)
            {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 55555);
                server = new TcpListener(ipe);//Bind, separa el puerto    

                Thread hilo1 = new Thread(new ThreadStart(Escuchar));
                hilo1.IsBackground = true;
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
                    clients.Add(clientenuevo, null);


                    Thread HiloRecibir = new Thread(new ParameterizedThreadStart(Recibir));
                    HiloRecibir.Start(clientenuevo);

                    //Enviarle la lista de todos los clientes ya conectados
                    var todosconectados = clients.Values.ToList();
                    var json = JsonConvert.SerializeObject(todosconectados);
                    byte[] buffer = Encoding.UTF8.GetBytes(json);
                    Enviar(clientenuevo, buffer);


                }
            }
        }

        public event Action<Usuario>? UsuarioConectado;
        public event Action<Usuario?>? UsuarioDesconectado;

        void Enviar(TcpClient cliente, byte[] buffer)
        {
            for (int i = 0; i < clients.Count(); i++)
            {
                TcpClient c = clients.Keys.ElementAt(i);

                try
                {
                    if (c != cliente)
                    {
                        if (c.Connected)//Esta conectado
                        {
                            var stream = c.GetStream();
                            stream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            //como saber quien se desconecto
                            //Avisar al viewmodel que lo borre 

                            UsuarioDesconectado?.Invoke(clients[c]);
                            clients.Remove(c);
                            i--;

                        }


                    }
                }
                catch (Exception)
                {
                    clients.Remove(c);
                    i--;
                    throw;
                }
            }
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

                        //Relaying




                        //clients.ForEach(x =>
                        //{
                        //    if (x != clienteconectado)
                        //    {
                        //        Enviar(x, buffer);
                        //    }
                        //});

                        try
                        {
                           Enviar(clienteconectado, buffer);
                           var usuario = JsonConvert.DeserializeObject<List<Usuario>>(
                           Encoding.UTF8.GetString(buffer));

                            if (usuario != null)
                            {
                                foreach (var item in usuario)
                                {
                                    clients[clienteconectado] = item;
                                    UsuarioConectado?.Invoke(item);
                                }

                            }
                        }
                        catch (Exception)
                        {

                        }





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

