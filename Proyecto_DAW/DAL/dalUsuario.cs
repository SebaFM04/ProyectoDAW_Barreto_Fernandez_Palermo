using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using SERVICIOS;

namespace DAL
{
    public class dalUsuario
    {
        Acceso dal;
        encriptador seguridad;

        public dalUsuario()
        {
            dal = new Acceso();
            seguridad = new encriptador();

        }


        public void Alta(Usuario usuario)
        {
            string query = "INSERT INTO Usuario " +
                         "(dni, nombreUsuario, contraseña, nombre, apellido, rol, email, bloqueo, intentos, lenguaje, activo, domicilio) " +
                         "VALUES (@dni, @nombreUsuario, @contraseña, @nombre, @apellido, @rol, @email, @bloqueo, @intentos, @lenguaje, @activo, @domicilio)";
            EjecutarQueryConEntidad(usuario, query);
        }

        public void Modificar(Usuario usuario)
        {
            string query = "UPDATE Usuario SET nombre = @nombre, contraseña = @contraseña, rol = @rol, email = @email, bloqueo = @bloqueo, intentos = @intentos, " +
                         "lenguaje = @lenguaje, activo = @activo, domicilio = @domicilio, apellido=@apellido, nombreUsuario = @nombreUsuario WHERE dni = @dni";
            // Lista de propiedades usadas en la consulta
            var props = new List<string>
            {
                "nombre","contraseña", "rol", "email", "bloqueo",
                "intentos", "lenguaje", "activo", "domicilio",
                "apellido", "nombreUsuario", "dni"
            };

            EjecutarQueryConEntidad(usuario, query, props);
        }

        private void EjecutarQueryConEntidad(Usuario usuario, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(usuario, propiedadesIncluir);
            // Cifrar domicilio antes de persistir
            if (parametros.ContainsKey("@domicilio") && parametros["@domicilio"] != DBNull.Value)
            {
                string plain = parametros["@domicilio"].ToString();
                parametros["@domicilio"] = string.IsNullOrEmpty(plain)
                    ? (object)DBNull.Value
                    : seguridad.Encrypt(plain);
            }
            dal.Query(query, parametros);
        }

        public bool ValidarDni(string dni)
        {
            string query = "SELECT COUNT(*) FROM Usuario WHERE dni = @dni";
            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni }
            };
            int count = Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
            return count > 0;
        }

        public bool ValidarExistenciaNombreUsuario(string nombreUsuario)
        {
            string query = "SELECT COUNT(*) FROM Usuario WHERE nombreUsuario = @nombreusuario";
            var parametros = new Dictionary<string, object>
            {
                { "@nombreusuario", nombreUsuario }
            };
            int count = Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
            return count > 0;
        }

        public bool PerfilAsignadoAUsuario(string nombrePerfil)
        {

            string query = "SELECT COUNT(*) FROM Usuario WHERE rol = @rol";
            var parametros = new Dictionary<string, object>
            {
                { "@rol", nombrePerfil }
            };
            int count = Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
            return count > 0;
        }


        public bool ValidarEmail(string email, string dni)
        {
            string query = "SELECT COUNT(*) FROM Usuario WHERE email = @email AND dni != @dni";
            var parametros = new Dictionary<string, object>
            {
                { "@email", email },
                { "@dni", dni }
            };
            int count = Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
            return count > 0;
        }

        public bool ValidarContraseña(string usuario, string contraseña)
        {
            string query = "SELECT COUNT(*) FROM Usuario WHERE nombreUsuario = @usuario AND contraseña = @contraseña";
            var parametros = new Dictionary<string, object>
            {
                { "@usuario", usuario },
                { "@contraseña", contraseña }
            };
            int count = Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
            return count > 0;
        }

        //si el usuario ingresa mal la contraseña, se le incrementa + 1 los intentos
        // intentos > 3 --> usuario bloqueado
        public int Intentos(Usuario usuario)
        {
            usuario.intentos++;
            string query;
            List<string> propiedades;

            if (usuario.intentos == 3)
            {
                usuario.bloqueo = true;
                query = "UPDATE Usuario SET intentos = @intentos, bloqueo = @bloqueo WHERE dni = @dni";
                propiedades = new List<string> { "intentos", "bloqueo", "dni" };
            }
            else
            {
                query = "UPDATE Usuario SET intentos = @intentos  WHERE dni = @dni";
                propiedades = new List<string> { "intentos", "dni" };
            }
            // Llamada directa a dal para no pasar por el cifrado de domicilio
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(usuario, propiedades);
            dal.Query(query, parametros);
            return usuario.intentos;
        }

        public Usuario ObtenerUsuarioPorDni(string dni)
        {
            string query = "SELECT * FROM Usuario WHERE dni = @dni";
            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni }
            };
            var usuarios = dal.RetornarLista(query, MapearUsuario, parametros);
            return usuarios.FirstOrDefault();
        }

        public List<Usuario> RetornarUsuarios()
        {
            List<Usuario> usuarios = dal.RetornarLista("SELECT * FROM Usuario", MapearUsuario);
            return usuarios;
        }

        private Usuario MapearUsuario(SqlDataReader reader)
        {
            string domicilioCifrado = reader["domicilio"] == DBNull.Value
               ? null
               : reader["domicilio"].ToString();

            string domicilioPlain = string.IsNullOrEmpty(domicilioCifrado)
                ? string.Empty
                : seguridad.Decrypt(domicilioCifrado);

            return new Usuario(
                reader["dni"].ToString(),
                reader["nombreUsuario"].ToString(),
                reader["contraseña"].ToString(),
                reader["nombre"].ToString(),
                reader["apellido"].ToString(),
                reader["rol"].ToString(),
                reader["email"].ToString(),
                Convert.ToBoolean(reader["bloqueo"]),
                Convert.ToInt32(reader["intentos"]),
                reader["lenguaje"].ToString(),
                Convert.ToBoolean(reader["activo"])
            );
        }
    }
}
