using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class bllFamilia
    {
        private dalFamilia dal;
        private bllBitacora bitacora;
 
        public bllFamilia()
        {
            dal = new dalFamilia();
            bitacora = new bllBitacora();
        }
 
        public List<AccesoPermiso> ObtenerTodosLosPermisosSimples()
        {
            return dal.ObtenerTodosLosPermisosSimples();
        }
 
        public List<AccesoPermiso> ObtenerTodasLasFamilias()
        {
            return dal.ObtenerTodasLasFamilias();
        }
 
        public Familia ObtenerFamiliaEspecifica(int idFamilia)
        {
            return dal.ObtenerFamiliaEspecifica(idFamilia);
        }

        #region ABM Familias
        public void CrearFamilia(string nombre, List<AccesoPermiso> hijos)
        {
            nombre = (nombre ?? string.Empty).Trim();
 
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ExcepcionNegocio("bllFamilia_msgNombreRequerido", "Debe ingresar un nombre para la familia.");
 
            if (hijos == null || hijos.Count == 0)
                throw new ExcepcionNegocio("bllFamilia_msgCrearSinPermisos", "No se puede crear una familia sin permisos. Seleccioná al menos un permiso o familia.");
 
            List<Familia> existentes = dal.ObtenerTodasLasFamilias().Cast<Familia>().ToList();
 
            if (existentes.Any(f => string.Equals(f.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
                throw new ExcepcionNegocio("bllFamilia_msgFamiliaExistente", "La familia \"{0}\" ya existe.", nombre);
 
            dal.CrearFamilia(nombre, hijos);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Familia creada", 3);
            //DV para Familia: bllDigitoVerificador.CalcularDVFamilia();
        }

        public void ModificarFamilia(int idFamilia, string nombreNuevo)
        {
            nombreNuevo = (nombreNuevo ?? string.Empty).Trim();
 
            if (string.IsNullOrWhiteSpace(nombreNuevo))
                throw new ExcepcionNegocio("bllFamilia_msgNombreModificarRequerido", "Debe ingresar un nombre para modificar la familia.");
 
            if (dal.ObtenerFamiliaEspecifica(idFamilia) == null)
                throw new ExcepcionNegocio("bllFamilia_msgFamiliaNoEncontrada", "Familia no encontrada.");
 
            dal.ModificarFamilia(idFamilia, nombreNuevo);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Familia modificada", 3);
            //DV para Familia: bllDigitoVerificador.CalcularDVFamilia();
        }

        public void BorrarFamilia(int idFamilia)
        {
            dal.BorrarFamilia(idFamilia);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Familia eliminada", 5);
            //DV para Familia: bllDigitoVerificador.CalcularDVFamilia();
        }
        #endregion
        
        #region Asignaciones
        public void AsignarAccesoAFamilia(int idFamilia, AccesoPermiso acceso)
        {
            Familia familia = dal.ObtenerFamiliaEspecifica(idFamilia);
            if (familia == null)
                throw new ExcepcionNegocio("bllFamilia_msgFamiliaNoEncontrada", "Familia no encontrada.");
 
            if (VerificarPermisoFamiliaRepetida(familia.ObtenerHijos(), acceso))
                throw new ExcepcionNegocio("bllFamilia_msgAccesoYaAsignado", "El permiso o familia ya está asignado a la familia.");
            
            if (acceso is Familia hija)
            {
                if (hija.ID == idFamilia)
                    throw new ExcepcionNegocio("bllFamilia_msgFamiliaSeContiene", "Una familia no puede contenerse a sí misma.");
 
                Familia hijaCompleta = dal.ObtenerFamiliaEspecifica(hija.ID);
                if (hijaCompleta != null && ContieneFamilia(hijaCompleta, idFamilia))
                    throw new ExcepcionNegocio("bllFamilia_msgFamiliaCiclo", "No se puede asignar la familia porque generaría un ciclo.");
            }
 
            dal.AsignarAccesoAFamilia(idFamilia, acceso);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Asignacion de permiso/familia a familia", 5);
            //DV para Familia: bllDigitoVerificador.CalcularDVFamilia();
        }

        public void DesasignarAccesoAFamilia(int idFamilia, AccesoPermiso acceso)
        {
            Familia familia = dal.ObtenerFamiliaEspecifica(idFamilia);
            if (familia == null)
                throw new ExcepcionNegocio("bllFamilia_msgFamiliaNoEncontrada", "Familia no encontrada.");
 
            // Puede quedar vacía solo si está en uso (asignada a un rol o dentro de otra familia): ahí no se puede eliminar
            bool enUso = dal.EstaAsignadaARol(idFamilia) || dal.EstaIncluidaEnOtraFamilia(idFamilia);
            if (familia.ObtenerHijos().Count <= 1 && !enUso)
                throw new ExcepcionNegocio("bllFamilia_msgFamiliaSinPermisos", "La familia debe tener al menos un permiso o familia. Si querés dejarla vacía, eliminá la familia.");
 
            dal.DesasignarAccesoAFamilia(idFamilia, acceso);
            bitacora.Alta(LoginActual(), "Gestion perfiles/familias", "Desasignacion de permiso/familia a familia", 5);
            //DV para Familia: bllDigitoVerificador.CalcularDVFamilia();
        }
        #endregion

        public static bool VerificarPermisoFamiliaRepetida(List<AccesoPermiso> existentes, AccesoPermiso seleccionado)
        {
            if (existentes == null) return false;
 
            foreach (AccesoPermiso actual in existentes)
            {
                // Mismo tipo y mismo ID => es el mismo permiso/familia
                if (actual.Tipo == seleccionado.Tipo && actual.ID == seleccionado.ID)
                    return true;
 
                // Si el actual es una familia, reviso su contenido
                if (actual is Familia fam && VerificarPermisoFamiliaRepetida(fam.ObtenerHijos(), seleccionado))
                    return true;
 
                // Si el que quiero agregar es una familia, ninguno de sus hijos debe estar ya
                if (seleccionado is Familia famSel)
                {
                    foreach (AccesoPermiso hijo in famSel.ObtenerHijos())
                    {
                        if (VerificarPermisoFamiliaRepetida(existentes, hijo))
                            return true;
                    }
                }
            }
 
            return false;
        }

        private bool ContieneFamilia(Familia familia, int idBuscado)
        {
            foreach (AccesoPermiso hijo in familia.ObtenerHijos())
            {
                if (hijo is Familia f && (f.ID == idBuscado || ContieneFamilia(f, idBuscado)))
                    return true;
            }
            return false;
        }
 
        private string LoginActual()
        {
            Usuario u = claseSession.Gestor.RetornarUsuarioSession();
            return u != null ? u.nombreUsuario : "sistema";
        }
    }
}
