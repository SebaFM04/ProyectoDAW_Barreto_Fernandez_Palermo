using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalIdioma
    {
        Acceso dal;

        public dalIdioma()
        {
            dal = new Acceso();
        }

        // Crea el idioma y, en la misma transacción, le copia todos los textos del predeterminado
        // entre corchetes ("[Guardar cambios]") para que se vea qué falta traducir.
        public void Alta(Idioma idioma)
        {
            var parametros = new Dictionary<string, object>
            {
                { "@Codigo", idioma.Codigo },
                { "@Nombre", idioma.Nombre },
                { "@Activo", idioma.Activo }
            };

            List<AccionSql> acciones = new List<AccionSql>
            {
                new AccionSql
                {
                    Query = "INSERT INTO Idioma (Codigo, Nombre, Activo, EsPredeterminado) VALUES (@Codigo, @Nombre, @Activo, 0)",
                    Parametros = parametros
                },
                new AccionSql
                {
                    Query = "INSERT INTO Traduccion (ID_Idioma, ID_Etiqueta, Texto) " +
                            "SELECT i.ID, t.ID_Etiqueta, N'[' + LEFT(t.Texto, 998) + N']' " +
                            "FROM Idioma i CROSS JOIN Traduccion t " +
                            "WHERE i.Codigo = @Codigo AND t.ID_Idioma = (SELECT ID FROM Idioma WHERE EsPredeterminado = 1)",
                    Parametros = new Dictionary<string, object> { { "@Codigo", idioma.Codigo } }
                }
            };

            dal.EjecutarTransaccion(acciones);
        }

        public void Modificar(Idioma idioma)
        {
            string query = "UPDATE Idioma SET Codigo = @Codigo, Nombre = @Nombre WHERE ID = @ID";

            var parametros = new Dictionary<string, object>
            {
                { "@ID", idioma.ID },
                { "@Codigo", idioma.Codigo },
                { "@Nombre", idioma.Nombre }
            };

            dal.Query(query, parametros);
        }

        public void CambiarEstado(int id, bool activo)
        {
            string query = "UPDATE Idioma SET Activo = @Activo WHERE ID = @ID";

            var parametros = new Dictionary<string, object>
            {
                { "@ID", id },
                { "@Activo", activo }
            };

            dal.Query(query, parametros);
        }

        // Saca la marca al predeterminado actual y se la pone al nuevo, todo en una transacción
        public void MarcarPredeterminado(int id)
        {
            List<AccionSql> acciones = new List<AccionSql>
            {
                new AccionSql { Query = "UPDATE Idioma SET EsPredeterminado = 0 WHERE EsPredeterminado = 1" },
                new AccionSql
                {
                    Query = "UPDATE Idioma SET EsPredeterminado = 1 WHERE ID = @ID",
                    Parametros = new Dictionary<string, object> { { "@ID", id } }
                }
            };

            dal.EjecutarTransaccion(acciones);
        }

        public List<Idioma> RetornarIdiomas(bool soloActivos = false)
        {
            string query = "SELECT * FROM Idioma" + (soloActivos ? " WHERE Activo = 1" : "") + " ORDER BY Nombre";
            return dal.RetornarLista(query, MapearIdioma);
        }

        public Idioma ObtenerPorId(int id)
        {
            var parametros = new Dictionary<string, object> { { "@ID", id } };
            return dal.RetornarLista("SELECT * FROM Idioma WHERE ID = @ID", MapearIdioma, parametros).FirstOrDefault();
        }

        public Idioma ObtenerPorCodigo(string codigo)
        {
            var parametros = new Dictionary<string, object> { { "@Codigo", codigo } };
            return dal.RetornarLista("SELECT * FROM Idioma WHERE Codigo = @Codigo", MapearIdioma, parametros).FirstOrDefault();
        }

        public Idioma ObtenerPredeterminado()
        {
            return dal.RetornarLista("SELECT * FROM Idioma WHERE EsPredeterminado = 1", MapearIdioma).FirstOrDefault();
        }

        // Excluye un ID para poder validar en las modificaciones
        public bool ExisteCodigo(string codigo, int idExcluido = 0)
        {
            string query = "SELECT COUNT(*) FROM Idioma WHERE Codigo = @Codigo AND ID <> @ID";
            var parametros = new Dictionary<string, object>
            {
                { "@Codigo", codigo },
                { "@ID", idExcluido }
            };
            return Convert.ToInt32(dal.EjecutarEscalar(query, parametros)) > 0;
        }

        public bool ExisteNombre(string nombre, int idExcluido = 0)
        {
            string query = "SELECT COUNT(*) FROM Idioma WHERE Nombre = @Nombre AND ID <> @ID";
            var parametros = new Dictionary<string, object>
            {
                { "@Nombre", nombre },
                { "@ID", idExcluido }
            };
            return Convert.ToInt32(dal.EjecutarEscalar(query, parametros)) > 0;
        }

        public int ContarActivos()
        {
            return Convert.ToInt32(dal.EjecutarEscalar("SELECT COUNT(*) FROM Idioma WHERE Activo = 1"));
        }

        private Idioma MapearIdioma(SqlDataReader reader)
        {
            return new Idioma(
                Convert.ToInt32(reader["ID"]),
                reader["Codigo"].ToString(),
                reader["Nombre"].ToString(),
                Convert.ToBoolean(reader["Activo"]),
                Convert.ToBoolean(reader["EsPredeterminado"])
            );
        }
    }
}
