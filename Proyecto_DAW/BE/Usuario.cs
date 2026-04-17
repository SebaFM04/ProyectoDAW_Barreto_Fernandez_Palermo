using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario
    {
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public bool Bloqueado { get; set; }
        public string Contrasena { get; set; }
        public int Intentos { get; set; }
    }
}
