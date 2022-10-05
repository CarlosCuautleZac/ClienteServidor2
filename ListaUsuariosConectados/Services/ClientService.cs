using ListaUsuariosConectados.Models;
using Newtonsoft.Json;
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
    public class ClientService
    {
        TcpClient cliente = new();

        public ClientService(string ip, Usuario usuario)
        {
            IPEndPoint ipe = new(IPAddress.Parse(ip), 55555);
            cliente.Connect(ipe);
            Thread.Sleep(1000);
            Enviar(usuario);
            Thread hilorecibir = new(new ThreadStart(Recibir));
            hilorecibir.Start();    
        }

        public void Enviar(Usuario u)
        {
            try
            {
                var json = JsonConvert.SerializeObject(u);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                var stream = cliente.GetStream();
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                cliente.Close();    
            }

        }

        public event Action<Usuario>? UsuarioRecibido;


        void Recibir()//Recibir
        {
            var stream = cliente.GetStream();

            while (cliente.Connected)
            {
                if (cliente.Available > 0)
                {
                    byte[] buffer = new byte[cliente.Available];
                    stream.Read(buffer, 0, buffer.Length);
                    string json = Encoding.UTF8.GetString(buffer);
                    var usuario = JsonConvert.DeserializeObject<Usuario>(json);
                    if(usuario != null)
                    {
                        UsuarioRecibido?.Invoke(usuario);
                    }
                }

                Thread.Sleep(1000);
            }
        }


        public void Cerrar()
        {
            cliente.Close();
        }
    }
}
