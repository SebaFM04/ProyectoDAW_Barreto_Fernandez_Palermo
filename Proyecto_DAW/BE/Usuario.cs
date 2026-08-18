using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Usuario
    {
        public Usuario(string pDni, string pNombreUsuario, string pContraseñaUsuario, string pNombre, string pApellido, string pRol, string pEmailUsuario, bool pBloqueo, int pIntentos, string pLenguaje, bool pActivado, string pDomicilio = "", int pCodigoIdioma = 1)
        {
            dni = pDni;
            nombreUsuario = pNombreUsuario;
            contraseña = pContraseñaUsuario;
            nombre = pNombre;
            apellido = pApellido;
            rol = pRol;
            email = pEmailUsuario;
            bloqueo = pBloqueo;
            intentos = pIntentos;
            lenguaje = pLenguaje;
            activo = pActivado;
            domicilio = pDomicilio;
            codigoIdioma = pCodigoIdioma;
        }

        public string dni { get; set; }
        public string nombreUsuario { get; set; }
        public string contraseña { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string rol { get; set; }
        public string email { get; set; }
        public bool bloqueo { get; set; }
        public int intentos { get; set; }
        public string lenguaje { get; set; }
        public bool activo { get; set; }
        public string domicilio { get; set; }

        // Idioma preferido del usuario (FK -> Idioma.codigo). 1 = Español por defecto.
        public int codigoIdioma { get; set; }
    }
}
