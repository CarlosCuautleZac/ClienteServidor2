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

        public bool SoyServidor { get; set; }

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
            throw new NotImplementedException();
        }

        private void GuardarPerfil()
        {
            throw new NotImplementedException();
        }

        private void Iniciar()
        {
            
        }
    }
}
