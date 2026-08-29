using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalFamilia
    {
        Acceso dal;

        public dalFamilia()
        {
            dal = new Acceso();
        }

        public List<AccesoPermiso> ObtenerTodosLosPermisosSimples()
        {
            string query = "SELECT ID, Nombre, Clave, Tipo FROM Permiso WHERE Tipo = 'S'";
            return dal.RetornarLista<AccesoPermiso>(query, MapearPatente);
        }

        public List<AccesoPermiso> ObtenerTodasLasFamilias()
        {
            string query = "SELECT ID, Nombre, Clave, Tipo FROM Permiso WHERE Tipo = 'C'";
            List<AccesoPermiso> familias = dal.RetornarLista<AccesoPermiso>(query, MapearFamilia);

            foreach (AccesoPermiso acceso in familias)
            {
                CargarContenidoFamilia((Familia)acceso);
            }
            return familias;
        }

        public Familia ObtenerFamiliaEspecifica(int idFamilia)
        {
            string query = "SELECT ID, Nombre, Clave, Tipo FROM Permiso WHERE ID = @id AND Tipo = 'C'";
            var parametros = new Dictionary<string, object> { { "@id", idFamilia } };

            Familia familia = dal.RetornarLista<AccesoPermiso>(query, MapearFamilia, parametros)
                                 .Cast<Familia>()
                                 .FirstOrDefault();

            if (familia != null) CargarContenidoFamilia(familia);
            return familia;
        }

        public void CargarContenidoFamilia(Familia familia)
        {
            string query = @"SELECT p.ID, p.Nombre, p.Clave, p.Tipo
                             FROM Permiso p
                             INNER JOIN Permiso_Detalle d ON p.ID = d.ID_PermisoHijo
                             WHERE d.ID_PermisoPadre = @idPadre";
            var parametros = new Dictionary<string, object> { { "@idPadre", familia.ID } };

            List<AccesoPermiso> hijos = dal.RetornarLista<AccesoPermiso>(query, MapearAcceso, parametros);

            foreach (AccesoPermiso hijo in hijos)
            {
                if (hijo is Familia subFamilia)
                {
                    CargarContenidoFamilia(subFamilia); // recursión
                }
                familia.Agregar(hijo);
            }
        }

        #region ABM de Familias
        public int CrearFamilia(string nombre, List<AccesoPermiso> hijos)
        {
            string queryInsert = @"INSERT INTO Permiso (Nombre, Clave, Tipo) VALUES (@nombre, 'Es Familia', 'C');
                                   SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var pInsert = new Dictionary<string, object>
            {
                { "@nombre", nombre }
            };

            int idFamilia = Convert.ToInt32(dal.EjecutarEscalar(queryInsert, pInsert));

            foreach (AccesoPermiso hijo in hijos)
            {
                AsignarAccesoAFamilia(idFamilia, hijo);
            }
            return idFamilia;
        }

        public void ModificarFamilia(int idFamilia, string nombreNuevo)
        {
            string query = "UPDATE Permiso SET Nombre = @nombre WHERE ID = @id AND Tipo = 'C'";
            var parametros = new Dictionary<string, object>
            {
                { "@nombre", nombreNuevo },
                { "@id", idFamilia }
            };
            dal.Query(query, parametros);
        }

        public void BorrarFamilia(int idFamilia)
        {
            var pId = new Dictionary<string, object> { { "@id", idFamilia } };

            // 1) No se puede borrar si está asignada a algún Rol
            string qRol = "SELECT COUNT(*) FROM Rol_Permiso WHERE ID_Permiso = @id";
            if (Convert.ToInt32(dal.EjecutarEscalar(qRol, pId)) > 0)
                throw new Exception("No se puede eliminar la familia porque está asignada a uno o más roles.");

            // 2) No se puede borrar si es hija de otra familia
            string qHija = "SELECT COUNT(*) FROM Permiso_Detalle WHERE ID_PermisoHijo = @id";
            if (Convert.ToInt32(dal.EjecutarEscalar(qHija, pId)) > 0)
                throw new Exception("No se puede eliminar la familia porque está incluida en otra familia padre.");

            // 3) Borro sus vínculos con hijos y luego la familia
            dal.Query("DELETE FROM Permiso_Detalle WHERE ID_PermisoPadre = @id", pId);
            dal.Query("DELETE FROM Permiso WHERE ID = @id AND Tipo = 'C'", pId);
        }
        #endregion

        #region Asignaciones de una Familia
        public void AsignarAccesoAFamilia(int idFamilia, AccesoPermiso hijo)
        {
            string query = "INSERT INTO Permiso_Detalle (ID_PermisoPadre, ID_PermisoHijo) VALUES (@padre, @hijo)";
            var parametros = new Dictionary<string, object>
            {
                { "@padre", idFamilia },
                { "@hijo", hijo.ID }
            };
            dal.Query(query, parametros);
        }

        public void DesasignarAccesoAFamilia(int idFamilia, AccesoPermiso hijo)
        {
            string query = "DELETE FROM Permiso_Detalle WHERE ID_PermisoPadre = @padre AND ID_PermisoHijo = @hijo";
            var parametros = new Dictionary<string, object>
            {
                { "@padre", idFamilia },
                { "@hijo", hijo.ID }
            };
            dal.Query(query, parametros);
        }
        #endregion

        #region Mapeos
        private AccesoPermiso MapearAcceso(SqlDataReader reader)
        {
            char tipo = Convert.ToChar(reader["Tipo"]);
            return tipo == 'C' ? MapearFamilia(reader) : MapearPatente(reader);
        }

        private AccesoPermiso MapearPatente(SqlDataReader reader)
        {
            return new Permiso(
                Convert.ToInt32(reader["ID"]),
                reader["Nombre"].ToString(),
                reader["Clave"].ToString()
            );
        }

        private AccesoPermiso MapearFamilia(SqlDataReader reader)
        {
            return new Familia(
                Convert.ToInt32(reader["ID"]),
                reader["Nombre"].ToString(),
                reader["Clave"].ToString()
            );
        }
        #endregion
    }
}
