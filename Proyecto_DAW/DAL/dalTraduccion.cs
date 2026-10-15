using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalTraduccion
    {
        Acceso dal;

        public dalTraduccion()
        {
            dal = new Acceso();
        }

        public List<Etiqueta> RetornarEtiquetas()
        {
            return dal.RetornarLista("SELECT * FROM Etiqueta ORDER BY Clave", reader => new Etiqueta(
                Convert.ToInt32(reader["ID"]),
                reader["Clave"].ToString(),
                reader["Descripcion"] == DBNull.Value ? null : reader["Descripcion"].ToString()
            ));
        }

        // Todas las traducciones cargadas de un idioma, clave -> texto
        public Dictionary<string, string> ObtenerDiccionario(int idIdioma)
        {
            string query = "SELECT e.Clave, t.Texto FROM Traduccion t " +
                           "INNER JOIN Etiqueta e ON e.ID = t.ID_Etiqueta " +
                           "WHERE t.ID_Idioma = @ID_Idioma";

            var parametros = new Dictionary<string, object> { { "@ID_Idioma", idIdioma } };

            var pares = dal.RetornarLista(query,
                reader => new KeyValuePair<string, string>(reader["Clave"].ToString(), reader["Texto"].ToString()),
                parametros);

            Dictionary<string, string> diccionario = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var par in pares)
                diccionario[par.Key] = par.Value;
            return diccionario;
        }

        // Todas las etiquetas (filtradas por prefijo de página) con la traducción del idioma elegido
        // y el texto del idioma predeterminado; las que no tienen traducción vienen con Texto vacío.
        public List<Traduccion> RetornarTraducciones(int idIdioma, int idIdiomaPredeterminado, string prefijo = null)
        {
            string query = "SELECT e.ID, e.Clave, t.Texto, tp.Texto AS TextoPredeterminado FROM Etiqueta e " +
                           "LEFT JOIN Traduccion t ON t.ID_Etiqueta = e.ID AND t.ID_Idioma = @ID_Idioma " +
                           "LEFT JOIN Traduccion tp ON tp.ID_Etiqueta = e.ID AND tp.ID_Idioma = @ID_Predeterminado " +
                           (string.IsNullOrEmpty(prefijo) ? "" : "WHERE e.Clave LIKE @Prefijo ") +
                           "ORDER BY e.Clave";

            var parametros = new Dictionary<string, object>
            {
                { "@ID_Idioma", idIdioma },
                { "@ID_Predeterminado", idIdiomaPredeterminado }
            };
            if (!string.IsNullOrEmpty(prefijo))
                parametros.Add("@Prefijo", prefijo + "[_]%");   // [_] porque _ es comodín en LIKE

            return dal.RetornarLista(query, reader => new Traduccion(
                idIdioma,
                Convert.ToInt32(reader["ID"]),
                reader["Clave"].ToString(),
                reader["Texto"] == DBNull.Value ? "" : reader["Texto"].ToString(),
                reader["TextoPredeterminado"] == DBNull.Value ? "" : reader["TextoPredeterminado"].ToString()
            ), parametros);
        }

        public void Guardar(int idIdioma, int idEtiqueta, string texto)
        {
            string query = "IF EXISTS (SELECT 1 FROM Traduccion WHERE ID_Idioma = @ID_Idioma AND ID_Etiqueta = @ID_Etiqueta) " +
                           "UPDATE Traduccion SET Texto = @Texto WHERE ID_Idioma = @ID_Idioma AND ID_Etiqueta = @ID_Etiqueta " +
                           "ELSE INSERT INTO Traduccion (ID_Idioma, ID_Etiqueta, Texto) VALUES (@ID_Idioma, @ID_Etiqueta, @Texto)";

            var parametros = new Dictionary<string, object>
            {
                { "@ID_Idioma", idIdioma },
                { "@ID_Etiqueta", idEtiqueta },
                { "@Texto", texto }
            };

            dal.Query(query, parametros);
        }

        // Deja la traducción como pendiente: el texto del predeterminado entre corchetes
        public void MarcarPendiente(int idIdioma, int idEtiqueta)
        {
            string query = "DECLARE @Texto nvarchar(1000) = (SELECT N'[' + LEFT(t.Texto, 998) + N']' FROM Traduccion t " +
                           "WHERE t.ID_Etiqueta = @ID_Etiqueta AND t.ID_Idioma = (SELECT ID FROM Idioma WHERE EsPredeterminado = 1)); " +
                           "IF @Texto IS NULL RETURN; " +
                           "IF EXISTS (SELECT 1 FROM Traduccion WHERE ID_Idioma = @ID_Idioma AND ID_Etiqueta = @ID_Etiqueta) " +
                           "UPDATE Traduccion SET Texto = @Texto WHERE ID_Idioma = @ID_Idioma AND ID_Etiqueta = @ID_Etiqueta " +
                           "ELSE INSERT INTO Traduccion (ID_Idioma, ID_Etiqueta, Texto) VALUES (@ID_Idioma, @ID_Etiqueta, @Texto)";

            var parametros = new Dictionary<string, object>
            {
                { "@ID_Idioma", idIdioma },
                { "@ID_Etiqueta", idEtiqueta }
            };

            dal.Query(query, parametros);
        }

    }
}
