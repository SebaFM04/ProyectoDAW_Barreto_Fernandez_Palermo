using System;
using BE;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalDigitoVerificador
    {
        Acceso dal;

        public dalDigitoVerificador()
        {
            dal = new Acceso();
        }

        public void Update(DigitoVerificador d)
        {
            string query = "UPDATE DigitoVerificador SET horizontal = @horizontal, vertical = @vertical WHERE nombreTabla = @nombreTabla";

            var parametros = new Dictionary<string, object>
            {
                { "@nombreTabla", d.nombreTabla },
                { "@horizontal", d.horizontal },
                { "@vertical", d.vertical }
            };

            dal.Query(query, parametros);
        }

        public List<string> CompararDigitos(List<DigitoVerificador> listaCalculados)
        {
            List<string> tablasConInconsistencias = new List<string>();

            foreach (var dvCalculado in listaCalculados)
            {
                string query = @"SELECT nombreTabla, horizontal, vertical 
                         FROM DigitoVerificador 
                         WHERE nombreTabla = @nombreTabla";
                var parametros = new Dictionary<string, object>
                {
                    { "@nombreTabla", dvCalculado.nombreTabla }
                };

                List<DigitoVerificador> resultado = dal.RetornarLista(query, MapearDigito, parametros);

                if (resultado.Count == 0)
                {
                    tablasConInconsistencias.Add($"Tabla: {dvCalculado.nombreTabla} | No se encuentra el registro en BD");
                    continue;
                }

                var almacenado = resultado[0];

                if (almacenado.horizontal != dvCalculado.horizontal ||
                    almacenado.vertical != dvCalculado.vertical)
                {
                    string queryAuditoria = @"SELECT TOP 1 nombreTabla, operacion, registroId 
                                      FROM AuditoriaTablas 
                                      WHERE nombreTabla = @nombreTabla 
                                      ORDER BY fecha DESC";
                    var paramAuditoria = new Dictionary<string, object>
                    {
                        { "@nombreTabla", dvCalculado.nombreTabla }
                    };

                    List<string> auditoriaResultado = dal.RetornarLista(queryAuditoria, r =>
                        $"Tabla: {r["nombreTabla"]} | Operación: {r["operacion"]} | Registro: {r["registroId"]}",
                        paramAuditoria);

                    string info = auditoriaResultado.Count > 0
                        ? auditoriaResultado[0]
                        : $"Tabla: {dvCalculado.nombreTabla} | Sin información de auditoría";

                    tablasConInconsistencias.Add(info);
                }
            }

            return tablasConInconsistencias;
        }

        public List<DigitoVerificador> RetornarDigitos()
        {
            List<DigitoVerificador> digito = dal.RetornarLista("SELECT * FROM DigitoVerificador", MapearDigito);
            return digito;
        }

        private DigitoVerificador MapearDigito(SqlDataReader reader)
        {
            return new DigitoVerificador(
                reader["nombreTabla"].ToString(),
                reader["horizontal"].ToString(),
                reader["vertical"].ToString()
            );
        }
    }
}
