using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.bllRol;

namespace BLL
{
    public class bllRol
    {
        private dalRol dal;
        private bllBitacora bitacora;

        public bllRol()
        {
            dal = new dalRol();
            bitacora = new bllBitacora();
        }

        public List<Rol> ObtenerRoles()
        {
            return dal.ObtenerRoles();
        }

        public List<AccesoPermiso> ObtenerAccesosPorRol(int idRol)
        {
            return dal.ObtenerAccesosPorRol(idRol);
        }

        #region ABM Roles
        public void CrearRol(string nombre, List<AccesoPermiso> accesos)
        {
            nombre = (nombre ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ExcepcionNegocio("bllRol_msgNombreRequerido", "Debe ingresar un nombre para el perfil.");

            if (accesos == null || accesos.Count == 0)
                throw new ExcepcionNegocio("bllRol_msgCrearSinPermisos", "No se puede crear un perfil sin permisos. Seleccioná al menos un permiso o familia.");

            if (dal.ObtenerRol(nombre) != null)
                throw new ExcepcionNegocio("bllRol_msgPerfilExistente", "El perfil \"{0}\" ya existe.", nombre);

            dal.CrearRol(nombre, accesos);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Perfil creado", 3);
            //DV para Rol: bllDigitoVerificador.CalcularDVRol();
        }

        public void ModificarRol(int idRol, string nombreNuevo)
        {
            nombreNuevo = (nombreNuevo ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombreNuevo))
                throw new ExcepcionNegocio("bllRol_msgNombreModificarRequerido", "Debe ingresar un nombre para modificar el perfil.");

            Rol rol = dal.ObtenerRolPorID(idRol);
            if (rol == null)
                throw new ExcepcionNegocio("bllRol_msgPerfilNoEncontrado", "Perfil no encontrado.");

            rol.Nombre = nombreNuevo;
            dal.ModificarRol(rol);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Perfil modificado", 3);
            //DV para Rol: bllDigitoVerificador.CalcularDVRol();
        }

        public void BorrarRol(int idRol)
        {
            dal.BorrarRol(idRol);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Perfil eliminado", 5);
            //DV para Rol: bllDigitoVerificador.CalcularDVRol();
        }
        #endregion

        #region Asignaciones 
        public void AsignarAccesoAlRol(int idRol, AccesoPermiso acceso)
        {
            List<AccesoPermiso> actuales = dal.ObtenerAccesosPorRol(idRol);

            if (bllFamilia.VerificarPermisoFamiliaRepetida(actuales, acceso))
                throw new ExcepcionNegocio("bllRol_msgAccesoYaAsignado", "El permiso o familia ya está asignado al perfil.");

            dal.AsignarAccesoAlRol(idRol, acceso);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Asignacion de permiso/familia a perfil", 5);
            //DV para Rol: bllDigitoVerificador.CalcularDVRol();
        }

        public void DesasignarAccesoAlRol(int idRol, AccesoPermiso acceso)
        {
            List<AccesoPermiso> actuales = dal.ObtenerAccesosPorRol(idRol);

            // Puede quedar vacío solo si tiene usuarios asignados (en ese caso no se puede eliminar)
            if (actuales.Count <= 1 && dal.ContarUsuarios(idRol) == 0)
                throw new ExcepcionNegocio("bllRol_msgPerfilSinPermisos", "El perfil debe tener al menos un permiso o familia. Si querés dejarlo vacío, eliminá el perfil.");

            dal.DesasignarAccesoAlRol(idRol, acceso);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Desasignacion de permiso/familia a perfil", 5);
            //DV para Rol: bllDigitoVerificador.CalcularDVRol();
        }
        #endregion

        private string LoginActual()
        {
            Usuario u = claseSession.Gestor.RetornarUsuarioSession();
            return u != null ? u.nombreUsuario : "sistema";
        }
    }
}
