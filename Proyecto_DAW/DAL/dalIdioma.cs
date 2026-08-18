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

        // ===== Idioma =====

        public List<Idioma> ListarIdiomas()
        {
            string query = "SELECT * FROM Idioma ORDER BY nombre";
            return dal.RetornarLista(query, MapearIdioma);
        }

        public List<Idioma> ListarIdiomasDisponibles()
        {
            string query = "SELECT * FROM Idioma WHERE isDisponible = 1 ORDER BY nombre";
            return dal.RetornarLista(query, MapearIdioma);
        }

        public bool ValidarNombreIdioma(string nombre)
        {
            string query = "SELECT COUNT(*) FROM Idioma WHERE nombre = @nombre";
            var parametros = new Dictionary<string, object> { { "@nombre", nombre } };
            int count = Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
            return count > 0;
        }

        // Alta de un idioma nuevo, cargado por cualquier usuario. Arranca sin
        // ninguna fila en Traduccion: al no encontrar traducciones, GestorIdioma
        // resuelve cada control como "[NombreControl]" hasta que se completen.
        // Devuelve el codigo (IDENTITY) recién generado.
        public int AgregarIdioma(string nombre)
        {
            string query = "INSERT INTO Idioma (nombre, isDisponible) " +
                            "VALUES (@nombre, 1); SELECT CAST(SCOPE_IDENTITY() AS int)";
            var parametros = new Dictionary<string, object> { { "@nombre", nombre } };
            return Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
        }

        public void ModificarNombreIdioma(int codigoIdioma, string nombre)
        {
            string query = "UPDATE Idioma SET nombre = @nombre WHERE codigo = @codigo";
            var parametros = new Dictionary<string, object>
            {
                { "@nombre", nombre },
                { "@codigo", codigoIdioma }
            };
            dal.Query(query, parametros);
        }

        public void ToggleDisponibilidad(int codigoIdioma)
        {
            string query = "UPDATE Idioma SET isDisponible = ~isDisponible WHERE codigo = @codigo";
            var parametros = new Dictionary<string, object> { { "@codigo", codigoIdioma } };
            dal.Query(query, parametros);
        }

        private Idioma MapearIdioma(SqlDataReader reader)
        {
            return new Idioma(
                Convert.ToInt32(reader["codigo"]),
                reader["nombre"].ToString(),
                Convert.ToBoolean(reader["isDisponible"])
            );
        }

        // ===== Control =====

        // Da de alta el control la primera vez que aparece registrado desde
        // una pantalla de administración de traducciones. Si ya existe
        // (mismo NombreControl + NombreFormulario) devuelve su codigo actual.
        public int ObtenerOAgregarControl(string nombreControl, string nombreFormulario)
        {
            string queryBuscar = "SELECT codigo FROM Control WHERE nombreControl = @nombreControl AND nombreFormulario = @nombreFormulario";
            var parametrosBuscar = new Dictionary<string, object>
            {
                { "@nombreControl", nombreControl },
                { "@nombreFormulario", nombreFormulario }
            };
            object existente = dal.EjecutarEscalar(queryBuscar, parametrosBuscar);
            if (existente != null && existente != DBNull.Value)
                return Convert.ToInt32(existente);

            string queryInsert = "INSERT INTO Control (nombreControl, nombreFormulario) " +
                                  "VALUES (@nombreControl, @nombreFormulario); SELECT CAST(SCOPE_IDENTITY() AS int)";
            return Convert.ToInt32(dal.EjecutarEscalar(queryInsert, parametrosBuscar));
        }

        public List<ControlIdioma> ListarControles()
        {
            string query = "SELECT * FROM Control ORDER BY nombreFormulario, nombreControl";
            return dal.RetornarLista(query, MapearControl);
        }

        private ControlIdioma MapearControl(SqlDataReader reader)
        {
            return new ControlIdioma(
                Convert.ToInt32(reader["codigo"]),
                reader["nombreControl"].ToString(),
                reader["nombreFormulario"].ToString()
            );
        }

        // ===== Traduccion =====

        // Trae todas las traducciones cargadas para un idioma, ya con el
        // detalle de a qué control/formulario pertenecen. Si el idioma es
        // nuevo (sin filas en Traduccion todavía) devuelve lista vacía.
        public List<Traduccion> ObtenerTraduccionesPorIdioma(int codigoIdioma)
        {
            string query =
                "SELECT c.codigo AS codigoControl, c.nombreControl, c.nombreFormulario, " +
                "       t.codigoIdioma, t.textoTraducido " +
                "FROM Traduccion t " +
                "INNER JOIN Control c ON c.codigo = t.codigoControl " +
                "WHERE t.codigoIdioma = @codigoIdioma";
            var parametros = new Dictionary<string, object> { { "@codigoIdioma", codigoIdioma } };
            return dal.RetornarLista(query, MapearTraduccion, parametros);
        }

        // Devuelve, para un idioma y formulario dados, TODOS los controles
        // registrados junto con su traducción si existe (o null si todavía
        // no fue cargada). Pensado para alimentar una pantalla de ABM donde
        // el usuario completa los "[NombreControl]" pendientes.
        public List<Traduccion> ObtenerEstadoTraduccionesPorFormulario(int codigoIdioma, string nombreFormulario)
        {
            string query =
                "SELECT c.codigo AS codigoControl, c.nombreControl, c.nombreFormulario, " +
                "       @codigoIdioma AS codigoIdioma, t.textoTraducido " +
                "FROM Control c " +
                "LEFT JOIN Traduccion t ON t.codigoControl = c.codigo AND t.codigoIdioma = @codigoIdioma " +
                "WHERE c.nombreFormulario = @nombreFormulario " +
                "ORDER BY c.nombreControl";
            var parametros = new Dictionary<string, object>
            {
                { "@codigoIdioma", codigoIdioma },
                { "@nombreFormulario", nombreFormulario }
            };
            return dal.RetornarLista(query, MapearTraduccion, parametros);
        }

        public void ModificarTraduccion(int codigoControl, int codigoIdioma, string textoTraducido)
        {
            string queryBuscar = "SELECT COUNT(*) FROM Traduccion WHERE codigoControl = @codigoControl AND codigoIdioma = @codigoIdioma";
            var parametrosBuscar = new Dictionary<string, object>
            {
                { "@codigoControl", codigoControl },
                { "@codigoIdioma", codigoIdioma }
            };
            int existe = Convert.ToInt32(dal.EjecutarEscalar(queryBuscar, parametrosBuscar));

            if (existe > 0)
            {
                string queryUpdate = "UPDATE Traduccion SET textoTraducido = @textoTraducido " +
                                      "WHERE codigoControl = @codigoControl AND codigoIdioma = @codigoIdioma";
                var parametros = new Dictionary<string, object>
                {
                    { "@textoTraducido", textoTraducido },
                    { "@codigoControl", codigoControl },
                    { "@codigoIdioma", codigoIdioma }
                };
                dal.Query(queryUpdate, parametros);
            }
            else
            {
                string queryInsert = "INSERT INTO Traduccion (codigoControl, codigoIdioma, textoTraducido) " +
                                      "VALUES (@codigoControl, @codigoIdioma, @textoTraducido)";
                var parametros = new Dictionary<string, object>
                {
                    { "@codigoControl", codigoControl },
                    { "@codigoIdioma", codigoIdioma },
                    { "@textoTraducido", textoTraducido }
                };
                dal.Query(queryInsert, parametros);
            }
        }

        private Traduccion MapearTraduccion(SqlDataReader reader)
        {
            return new Traduccion
            {
                codigoControl = Convert.ToInt32(reader["codigoControl"]),
                nombreControl = reader["nombreControl"].ToString(),
                nombreFormulario = reader["nombreFormulario"].ToString(),
                codigoIdioma = Convert.ToInt32(reader["codigoIdioma"]),
                textoTraducido = reader["textoTraducido"] == DBNull.Value ? null : reader["textoTraducido"].ToString()
            };
        }

        // ===== Usuario.codigoIdioma =====

        public void ActualizarIdiomaUsuario(string dni, int codigoIdioma)
        {
            string query = "UPDATE Usuario SET codigoIdioma = @codigoIdioma WHERE dni = @dni";
            var parametros = new Dictionary<string, object>
            {
                { "@codigoIdioma", codigoIdioma },
                { "@dni", dni }
            };
            dal.Query(query, parametros);
        }
    }
}
