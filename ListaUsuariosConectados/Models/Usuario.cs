using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaUsuariosConectados.Models
{
    
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Fotofrafia { get; set; } = "";
        public string Descripcion { get; set; } = "";
    }
}
