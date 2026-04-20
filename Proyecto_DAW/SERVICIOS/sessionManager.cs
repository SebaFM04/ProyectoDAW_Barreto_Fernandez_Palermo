using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace SERVICIOS
{
    public class sessionManager
    {
        public Usuario usuarioSession;
        private static sessionManager instance;

        private string idioma = "es";

        public string Idioma
        {
            get => idioma;
            set => idioma = value;
        }

        public static sessionManager Gestor
        {
            get
            {
                if (instance == null)
                {
                    instance = new sessionManager();
                }
                return instance;
            }
        }

        private HashSet<string> permisosUsuario;

        public void SetPermisosUsuario(HashSet<string> permisos)
        {
            permisosUsuario = permisos;
        }

        public HashSet<string> RetornarPermisosUsuario()
        {
            return permisosUsuario;
        }

        public Usuario RetornarUsuarioSession()
        {
            return usuarioSession;
        }

        public bool Session()
        {
            return usuarioSession != null ? true : false;
        }

        public void SetPerfil(string perfil)
        {
            usuarioSession.rol = perfil;
        }

        public void SetUsuario(Usuario usuario)
        {
            usuarioSession = usuario;
        }

        public void UnsetUsuario()
        {
            Gestor.usuarioSession = null;
        }
    }
}
