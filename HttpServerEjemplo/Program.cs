using System.Net;
using System.Text;

HttpListener servidor = new();
servidor.Prefixes.Add("http://*:10000/prueba/");
servidor.Start();

var context = servidor.GetContext();

if (context.Request.Url.LocalPath == "/")
{
    string saludo = "hola";
    byte[] buffer = Encoding.UTF8.GetBytes(saludo);
    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    context.Response.StatusCode = 200;
    context.Response.Close();
}