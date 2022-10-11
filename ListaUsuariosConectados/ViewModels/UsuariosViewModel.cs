using GalaSoft.MvvmLight.Command;
using ListaUsuariosConectados.Models;
using ListaUsuariosConectados.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ListaUsuariosConectados.ViewModels
{
    public class UsuariosViewModel : INotifyPropertyChanged
    {
        public ICommand IniciarCommand { get; set; }
        public ICommand GuardarPerfilCommand { get; set; }
        public ICommand EditarPerfilCommand { get; set; }

        public ObservableCollection<Usuario> Usuarios { get; set; } = new ObservableCollection<Usuario>();

        public UserService? Servidor;
        public ClientService? ClientService;
        Dispatcher dispatcher;

        public bool SoyServidor { get; set; } = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Error { get; set; } = "";

        public Usuario? usuario { get; set; }

        public string? IP { get; set; }

        public string? MisIPs { get; set; }

        public string Vista { get; set; } = "Seleccionar";

        public UsuariosViewModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            IniciarCommand = new RelayCommand(Iniciar);
            GuardarPerfilCommand = new RelayCommand(GuardarPerfil);
            EditarPerfilCommand = new RelayCommand(EditarPerfil);
        }

        private void EditarPerfil()
        {

        }

        private void GuardarPerfil()
        {

        }

        private void Iniciar()
        {
            if (SoyServidor)
            {
                Servidor = new();
                Servidor.Iniciar();
                Servidor.UsuarioConectado += Servidor_UsuarioConectado;
                Servidor.UsuarioDesconectado += Servidor_UsuarioDesconectado;
                Vista = "Usuarios";
                MisIPs = GetIPs();
                Actualizar();


            }
            else
            {
                Error = "";
                if (IPAddress.TryParse(IP, out IPAddress? direccionip))
                {
                    ClientService = new(direccionip.ToString(), new Usuario
                    {
                        Id = Guid.NewGuid(),
                        Nombre = Dns.GetHostName(),
                        Descripcion = "",
                        Fotofrafia = "https://t3.ftcdn.net/jpg/03/46/83/96/360_F_346839683_6nAPzbhpSkIpb8pmAwufkC7c5eD7wYws.jpg"
                    });

                    ClientService.UsuarioRecibido += ClientService_UsuarioRecibido;
                    Vista = "Usuarios";
                    MisIPs = GetIPs();
                    Actualizar();
                }
                else
                {
                    Error = "Escriba una direccion IP valida";
                    Actualizar(nameof(Error));
                }
            }
        }

        private void ClientService_UsuarioRecibido(Usuario obj)
        {
            throw new NotImplementedException();
        }

        string GetIPs()
        {
            string ips = "";

            foreach (var red in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (red.OperationalStatus == OperationalStatus.Up && (red.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    red.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                {
                    foreach (var ip in red.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            ips += ip.Address.ToString() + Environment.NewLine;
                    }
                }
            }

            return ips;
        }



        private void Servidor_UsuarioDesconectado(Usuario? obj)
        {
            throw new NotImplementedException();
        }

        private void Servidor_UsuarioConectado(Usuario obj)
        {
            dispatcher.Invoke(() =>
            {
                Usuarios.Add(obj);
            });
            
        }


        void Actualizar(string? propiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }
    }
}
