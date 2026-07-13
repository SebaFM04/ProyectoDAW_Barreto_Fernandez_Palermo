using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class bllUsuario
    {
        encriptador seguridad;
        dalUsuario dal;
        bllBitacora bllBitacora;
        bllDigitoVerificador bllDigitoVerificador;
        public bllUsuario()
        {
            seguridad = new encriptador();
            dal = new dalUsuario();
            bllBitacora = new bllBitacora();
            bllDigitoVerificador = new bllDigitoVerificador();
        }
        private void RecalcularDigitoUsuario()
        {
            bllDigitoVerificador.CalcularDVUsuario();
        }
        public void Alta(string dni, string nombre, string apellido, string rol, string email, string contraseña, string domicilio = "")
        {
            if (dal.ValidarDni(dni))
                throw new Exception("Ya existe un usuario con ese DNI.");

            if (!EsValida(contraseña))
                throw new Exception("Contraseña NO cumple con las reglas del sistema. Tener al menos 8 caracteres.\r\nIncluir letras minúsculas y mayúsculas.\r\nContener al menos un carácter especial.\r\n");

            string nombreUsuario = dni + nombre;
            string contraseñaHasheada = HashearContraseña(contraseña);
            Usuario nuevoUsuario = new Usuario(dni, nombreUsuario, contraseñaHasheada, nombre, apellido, rol, email, false, 0, "es", true, domicilio);
            dal.Alta(nuevoUsuario);
            RecalcularDigitoUsuario();
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario dado de alta", 1);
        }

        public bool VerificarContraseñaNoSeaDNIyApellido(string contraseña)
        {
            bool coincide = false;
            string contraseñaVieja = HashearContraseña(claseSession.Gestor.RetornarUsuarioSession().dni + claseSession.Gestor.RetornarUsuarioSession().apellido);
            if (contraseña == contraseñaVieja)
            {
                coincide = true;
            }
            return coincide;
        }

        public bool ValidarExistenciaNombreUsuario(string nombreUsuario)
        {
            return dal.ValidarExistenciaNombreUsuario(nombreUsuario);
        }

        public string HashearContraseña(string contraseñaUsuario)
        {
            return seguridad.GetSHA256(contraseñaUsuario);
        }

        public void ReiniciarIntentos(Usuario usuario)
        {
            usuario.intentos = 0;
            dal.Modificar(usuario);
        }


        public bool ValidarContraseñaActual(string usuario, string contraseña)
        {
            string contraseñaHasheada = HashearContraseña(contraseña);
            return dal.ValidarContraseña(usuario, contraseñaHasheada);
        }

        public bool ValidarDNI(string dni)
        {
            return dal.ValidarDni(dni);
        }

        public int Intentos(Usuario nombreUsuario)
        {
            return dal.Intentos(nombreUsuario);
        }

        public void Modificar(string dni, string rol, string email, string apellido, string nombreUsuario, string nombre, bool activo)
        {
            Usuario usuario = BuscarUsuarioPorDNI(dni);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");
            usuario.nombre = nombre;
            usuario.nombreUsuario = nombreUsuario;
            usuario.apellido = apellido;
            usuario.rol = rol;
            usuario.email = email;
            usuario.activo = activo;
            dal.Modificar(usuario);
            RecalcularDigitoUsuario();
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario modificado", 1);
        }


        ///// REBUNDATE AHORA
        //public void ActivarDesactivar(string dni)
        //{
        //    try
        //    {
        //        Usuario usuario = BuscarUsuarioPorDNI(dni);
        //        if (usuario == null)
        //        {
        //            string mensaje = TraductorHelper.TraducirMensaje("FdalGestionUsuario", "MSG_USUARIO_NO_ENCONTRADO", "Usuario no encontrado");
        //            MessageBox.Show(mensaje);
        //            return;
        //        }
        //        //Invierte el valor actual del campo activo
        //        usuario.activo = !usuario.activo;
        //        string activado = TraductorHelper.TraducirMensaje("FdalGestionUsuario", "MSG_USUARIO_ACTIVADO", "Se ha activado al usuario con éxito");
        //        string noActivado = TraductorHelper.TraducirMensaje("FdalGestionUsuario", "MSG_USUARIO_NO_ACTIVADO", "Se ha desactivado al usuario con éxito");
        //        string mensaje;
        //        if (usuario.activo)
        //        {
        //            mensaje = activado;
        //            bllBitacoraEventos.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario activado", 1);
        //        }
        //        else
        //        {
        //            mensaje = noActivado;
        //            bllBitacoraEventos.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario desactivado", 1);
        //        }
        //        dal.Modificar(usuario);
        //        MessageBox.Show(mensaje);
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}

        public Usuario BuscarUsuarioPorDNI(string dni)
        {
            return dal.ObtenerUsuarioPorDni(dni);
        }

        public bool Desbloquear(string dni)
        {
            Usuario usuario = BuscarUsuarioPorDNI(dni);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (!usuario.bloqueo)
                return false;

            usuario.bloqueo = false;
            usuario.intentos = 0;
            dal.Modificar(usuario);
            RecalcularDigitoUsuario();
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario desbloqueado", 1);
            return true;
        }

        public bool UsuarioActivo(Usuario usuario)
        {
            return usuario.activo;
        }

        public bool UsuarioBloqueado(Usuario usuario)
        {
            if (!usuario.bloqueo)
                return false;

            return true;
        }

        public bool ValidarEmail(string email, string dni)
        {
            return dal.ValidarEmail(email, dni);
        }

        public void ModificarContraseña(Usuario usuario, string contraseñaNueva)
        {

            if (EsValida(contraseñaNueva))
            {

                usuario.contraseña = HashearContraseña(contraseñaNueva);
                dal.Modificar(usuario);
                RecalcularDigitoUsuario();
                claseSession.Gestor.SetUsuario(usuario);
                bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Modificar contraseña usuario", 1);
            }
            else
            {
                throw new Exception("Contraseña NO cumple con las reglas del sistema. Tener al menos 8 caracteres.\r\nIncluir letras minúsculas y mayúsculas.\r\nContener al menos un carácter especial.\r\n");
            }
        }

        public List<Usuario> RetornarUsuarios()
        {
            return dal.RetornarUsuarios();
        }

        private static readonly Regex PasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$", RegexOptions.Compiled);

        public static bool EsValida(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            return PasswordRegex.IsMatch(password);
        }

        public void CambiarContraseñaAdmin(string dni, string nuevaContraseña)
        {
            Usuario usuario = BuscarUsuarioPorDNI(dni);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (!EsValida(nuevaContraseña))
                throw new Exception("Contraseña NO cumple con las reglas del sistema. Tener al menos 8 caracteres.\r\nIncluir letras minúsculas y mayúsculas.\r\nContener al menos un carácter especial.\r\n");

            usuario.contraseña = HashearContraseña(nuevaContraseña);
            dal.Modificar(usuario);
            RecalcularDigitoUsuario();
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Contraseña modificada por admin", 1);
        }

    }
}
