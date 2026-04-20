using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVICIOS;
using BE;
using DAL;

namespace BLL
{
    public class bllUsuario
    {
        encriptador seguridad;
        dalUsuario dal;

        public bllUsuario()
        {
            seguridad = new encriptador();
            dal = new dalUsuario();
        }

        //public void Alta(string dni, string nombre, string apellido, string rol, string email)
        //{
        //    try
        //    {
        //        string nombreUsuario = dni + nombre;
        //        string contraseña = HashearContraseña(dni + apellido); // lógica de negocio: contraseña inicial hasheada
        //        Usuario nuevoUsuario = new Usuario(dni, nombreUsuario, contraseña, nombre, apellido, rol, email, false, 0, "es", true);
        //        dal.Alta(nuevoUsuario);
        //        //bllBitacoraEventos.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario dado de alta", 1);
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}

        public bool VerificarContraseñaNoSeaDNIyApellido(string contraseña)
        {
            bool coincide = false;
            string contraseñaVieja = HashearContraseña(sessionManager.Gestor.RetornarUsuarioSession().dni + sessionManager.Gestor.RetornarUsuarioSession().apellido);
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

        public int AumentarIntentos(Usuario nombreUsuario)
        {
            return dal.AumentarIntentos(nombreUsuario);
        }

        //public void Modificar(string dni, string rol, string email)
        //{
        //    try
        //    {
        //        Usuario nuevoUsuario = BuscarUsuarioPorDNI(dni);
        //        nuevoUsuario.rol = rol;
        //        nuevoUsuario.email = email;
        //        dal.Modificar(nuevoUsuario);
        //        sessionManager.Gestor.SetPerfil(rol);
        //        //bllBitacoraEventos.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario modificado", 1);
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}

        

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

        //public void Desbloquear(string dni)
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
        //        else if (usuario.bloqueo)
        //        {
        //            usuario.contraseña = HashearContraseña(usuario.dni + usuario.apellido);
        //            usuario.bloqueo = false;
        //            usuario.intentos = 0;
        //            dal.Modificar(usuario);
        //            string desbloqueado = TraductorHelper.TraducirMensaje("FdalGestionUsuario", "MSG_USUARIO_DESBLOQUEADO", "Usuario desbloqueado exitosamente");
        //            //bllBitacoraEventos.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario bloqueado", 1);
        //            MessageBox.Show(desbloqueado);
        //        }
        //        else
        //        {
        //            string desbloqueado = TraductorHelper.TraducirMensaje("FdalGestionUsuario", "MSG_USUARIO_NO_DESBLOQUEADO", "El usuario ya se encuentra desbloqueado");
        //            //bllBitacoraEventos.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario desbloqueado", 1);
        //            MessageBox.Show(desbloqueado);
        //            return;
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}

        public bool UsuarioActivo(Usuario usuario)
        {
            return usuario.activo;
        }

        public bool UsuarioBloqueado(Usuario usuario)
        {
            if (!usuario.bloqueo)
                return false;

            //bllBitacoraEventos.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Usuario bloqueado", 1);
            return true;
        }

        public bool ValidarEmail(string email, string dni)
        {
            return dal.ValidarEmail(email, dni);
        }

        public void ModificarContraseña(Usuario usuario, string contraseñaNueva)
        {
            usuario.contraseña = HashearContraseña(contraseñaNueva);
            dal.Modificar(usuario);
            sessionManager.Gestor.SetUsuario(usuario);
            //bllBitacoraEventos.Alta(sessionManager.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion usuarios", "Modificar contraseña usuario", 1);
        }

        public List<Usuario> RetornarUsuarios()
        {
            return dal.RetornarUsuarios();
        }
    }
}
