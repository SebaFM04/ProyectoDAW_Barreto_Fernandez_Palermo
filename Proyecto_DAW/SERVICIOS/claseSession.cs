using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BE;


namespace SERVICIOS
{
    public class claseSession
    {
        public Usuario usuarioSession;
        private static claseSession instance;

        public static claseSession Gestor
        {
            get
            {
                if (instance == null)
                {
                    instance = new claseSession();
                }
                return instance;
            }
        }

        public Usuario RetornarUsuarioSession()
        {
            if (HttpContext.Current.Session["UsuarioActual"] != null)
            {
                return (Usuario)HttpContext.Current.Session["UsuarioActual"];
            }
            return null;
        }

        public bool Session()
        {
            return HttpContext.Current.Session["UsuarioActual"] != null;
        }

        public void SetUsuario(Usuario usuario)
        {
            HttpContext.Current.Session["UsuarioActual"] = usuario;
            HttpContext.Current.Session["Rol"] = usuario.rol;
        }

        public string RetornarRol()
        {
            return HttpContext.Current.Session["Rol"].ToString();
        }

        public void UnsetUsuario()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}
