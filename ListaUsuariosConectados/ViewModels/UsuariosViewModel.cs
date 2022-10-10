using GalaSoft.MvvmLight.Command;
using ListaUsuariosConectados.Models;
using ListaUsuariosConectados.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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

        public bool SoyServidor { get; set; } = true;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Error { get; set; } = "";

        public Usuario? usuario { get; set; }

        public string? IP { get; set; }

        public string? MisIPs { get; set; }

        public string Vista { get; set; } = "Seleccionar";

        public UsuariosViewModel()
        {
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
                Actualizar(nameof(Vista));

            }
            else
            {

            }
        }

        private void Servidor_UsuarioDesconectado(Usuario? obj)
        {
            throw new NotImplementedException();
        }

        private void Servidor_UsuarioConectado(Usuario obj)
        {
            throw new NotImplementedException();
        }

        void Actualizar(string? propiedad=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }
    }
}
