using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalRol
    {
        Acceso dal;
        dalFamilia datosFamilia;

        public dalRol()
        {
            dal = new Acceso();
            datosFamilia = new dalFamilia();
        }

        public List<Rol> ObtenerRoles()
        {
            return dal.RetornarLista<Rol>("SELECT ID, Nombre FROM Rol", MapearRol);
        }

        public Rol ObtenerRol(string nombre)
        {
            string query = "SELECT ID, Nombre FROM Rol WHERE Nombre = @nombre";
            var parametros = new Dictionary<string, object> { { "@nombre", nombre } };
            return dal.RetornarLista<Rol>(query, MapearRol, parametros).FirstOrDefault();
        }

        public Rol ObtenerRolPorID(int id)
        {
            string query = "SELECT ID, Nombre FROM Rol WHERE ID = @id";
            var parametros = new Dictionary<string, object> { { "@id", id } };
            return dal.RetornarLista<Rol>(query, MapearRol, parametros).FirstOrDefault();
        }

        #region ABM de Roles
        public int CrearRol(string nombre, List<AccesoPermiso> accesos)
        {
            string queryInsert = @"INSERT INTO Rol (Nombre) VALUES (@nombre);
                                   SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var pInsert = new Dictionary<string, object> { { "@nombre", nombre } };
            int idRol = Convert.ToInt32(dal.EjecutarEscalar(queryInsert, pInsert));

            foreach (AccesoPermiso acceso in accesos)
            {
                AsignarAccesoAlRol(idRol, acceso);
            }
            return idRol;
        }

        public void ModificarRol(Rol rol)
        {
            string query = "UPDATE Rol SET Nombre = @nombre WHERE ID = @id";
            var parametros = new Dictionary<string, object>
            {
                { "@nombre", rol.Nombre },
                { "@id", rol.ID }
            };
            dal.Query(query, parametros);
        }

        public void BorrarRol(int idRol)
        {
            var pId = new Dictionary<string, object> { { "@id", idRol } };

            // No se puede borrar si algún usuario tiene asignado este rol
            string qUsuarios = "SELECT COUNT(*) FROM Usuario WHERE rol = @id";
            if (Convert.ToInt32(dal.EjecutarEscalar(qUsuarios, pId)) > 0)
                throw new Exception("No se puede eliminar el perfil porque está asignado a uno o más usuarios.");

            // Primero borro las asignaciones de permisos, después el rol
            dal.Query("DELETE FROM Rol_Permiso WHERE ID_Rol = @id", pId);
            dal.Query("DELETE FROM Rol WHERE ID = @id", pId);
        }
        #endregion

        public List<AccesoPermiso> ObtenerAccesosPorRol(int idRol)
        {
            string query = @"SELECT p.ID, p.Nombre, p.Clave, p.Tipo
                             FROM Permiso p
                             INNER JOIN Rol_Permiso rp ON p.ID = rp.ID_Permiso
                             WHERE rp.ID_Rol = @idRol";
            var parametros = new Dictionary<string, object> { { "@idRol", idRol } };

            List<AccesoPermiso> accesos = dal.RetornarLista<AccesoPermiso>(query, MapearAcceso, parametros);

            foreach (AccesoPermiso acceso in accesos)
            {
                if (acceso is Familia familia)
                {
                    datosFamilia.CargarContenidoFamilia(familia);
                }
            }
            return accesos;
        }

        #region Asignaciones Rol <-> Permiso
        public void AsignarAccesoAlRol(int idRol, AccesoPermiso acceso)
        {
            string query = "INSERT INTO Rol_Permiso (ID_Rol, ID_Permiso) VALUES (@idRol, @idPermiso)";
            var parametros = new Dictionary<string, object>
            {
                { "@idRol", idRol },
                { "@idPermiso", acceso.ID }
            };
            dal.Query(query, parametros);
        }

        public void DesasignarAccesoAlRol(int idRol, AccesoPermiso acceso)
        {
            string query = "DELETE FROM Rol_Permiso WHERE ID_Rol = @idRol AND ID_Permiso = @idPermiso";
            var parametros = new Dictionary<string, object>
            {
                { "@idRol", idRol },
                { "@idPermiso", acceso.ID }
            };
            dal.Query(query, parametros);
        }
        #endregion

        #region Mapeos
        private Rol MapearRol(SqlDataReader reader)
        {
            return new Rol(
                Convert.ToInt32(reader["ID"]),
                reader["Nombre"].ToString()
            );
        }

        private AccesoPermiso MapearAcceso(SqlDataReader reader)
        {
            char tipo = Convert.ToChar(reader["Tipo"]);
            if (tipo == 'C')
            {
                return new Familia(
                    Convert.ToInt32(reader["ID"]),
                    reader["Nombre"].ToString(),
                    reader["Clave"].ToString()
                );
            }
            return new Permiso(
                Convert.ToInt32(reader["ID"]),
                reader["Nombre"].ToString(),
                reader["Clave"].ToString()
            );
        }
        #endregion
    }
}
