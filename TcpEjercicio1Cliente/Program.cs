using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Escribe la ip del servidor");
var ip = Console.ReadLine();

Console.WriteLine("Escriba el nombre de usuario");
var username = Console.ReadLine();

IPEndPoint ipe = new(IPAddress.Parse(ip), 2019);
TcpClient tcpClient = new();

//Conectarme
while (true)
{
    tcpClient.Connect(ipe);
    Thread.Sleep(2000);
    if (tcpClient.Connected)
    {
        Thread t = new(new ThreadStart(Recibir));
        t.Start();
        break;

    }
}


void Recibir()
{
    var stream = tcpClient.GetStream();
    while (tcpClient.Connected)
    {
        if (tcpClient.Available > 0)
        {
            byte[] buffer = new byte[tcpClient.Available];
            stream.Read(buffer, 0, buffer.Length);
            string mensaje = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(mensaje);
            
        }
        Thread.Sleep(1000);
    }
}

//Enviar mensajes

string? mensaje = "";
var stream = tcpClient.GetStream();
do
{
    mensaje = Console.ReadLine();
    byte[] bytes = Encoding.UTF8.GetBytes($"{username}: {mensaje}");
    stream.Write(bytes, 0, bytes.Length);


} while (!string.IsNullOrWhiteSpace(mensaje));

stream.Close();
tcpClient.Close();

Console.WriteLine("El servidor ha cerrado la conexion");