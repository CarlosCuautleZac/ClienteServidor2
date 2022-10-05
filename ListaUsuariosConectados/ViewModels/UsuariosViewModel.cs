using ListaUsuariosConectados.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaUsuariosConectados.ViewModels
{
    public class UsuariosViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Usuario> Usuarios { get; set; } = new ObservableCollection<Usuario>();

        public bool SoyServidor { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Error { get; set; } = string.Empty;

        public Usuario? usuario { get; set; }

    }
}
