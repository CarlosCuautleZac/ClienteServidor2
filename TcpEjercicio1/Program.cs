using System.Net;
using System.Net.Sockets;
using System.Text;

IPEndPoint ipe = new(IPAddress.Any, 2019);
TcpListener listener = new(ipe);
listener.Start();

List<TcpClient> clients = new List<TcpClient>();

while (true)
{
    TcpClient clienteConectado = listener.AcceptTcpClient();
    Console.WriteLine("Se ha conectado un cliente: " + clienteConectado.Client.RemoteEndPoint.ToString());

    clients.Add(clienteConectado);

    //Iniciamos un hilo para que reciba mensajes del cliente nuevo
    Thread hilo = new Thread(new ParameterizedThreadStart(RecibirDatos));
    hilo.Start(clienteConectado);
}

void RecibirDatos(object? tcpclient)
{
    if (tcpclient != null)
    {
        TcpClient cliente = (TcpClient)tcpclient;

        NetworkStream stream = cliente.GetStream();


        while (cliente.Connected)
        {
            if (cliente.Available > 0)
            {
                byte[] buffer = new byte[cliente.Available];
                stream.Read(buffer, 0, buffer.Length);

                string mensaje = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(mensaje);


                //RETRANSMITIR (RELAY)
                foreach (var c in clients)
                {
                    if (c != cliente)
                    {
                        EnviarMensjae(c, buffer);
                    }
                }
            }

            Thread.Sleep(1000);
        }

    }

}

void EnviarMensjae(TcpClient client, byte[] buffer)
{
    if (client.Connected)
    {
        var stream = client.GetStream();
        stream.Write(buffer, 0, buffer.Length);
    }
}
