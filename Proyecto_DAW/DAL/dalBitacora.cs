using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class dalBitacora
    {
        Acceso dal;

        public dalBitacora()
        {
            dal = new Acceso();
        }

        public string GenerarCodigoBitacora(DateTime fecha)
        {
            string fechaFormateada = fecha.ToString("ddMMyy");

            string query = "SELECT ISNULL(MAX(CAST(SUBSTRING(codigo, 7, 4) AS INT)), 0) FROM BitacoraEventos " +
                             "WHERE fecha = @fecha";

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@fecha", fecha.Date }
            };

            object resultado = dal.EjecutarEscalar(query, parametros);

            int ultimoConsecutivo = resultado != null && resultado != DBNull.Value ?
                                  Convert.ToInt32(resultado) : 0;

            int nuevoConsecutivo = ultimoConsecutivo + 1;

            return fechaFormateada + nuevoConsecutivo.ToString("D4");
        }

        public List<Evento> Filtros(Dictionary<string, string> filtros)
        {
            string query = "SELECT * FROM BitacoraEventos";
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            List<string> condiciones = new List<string>();

            foreach (var filtro in filtros)
            {
                string nombreParametro = "@" + filtro.Key;

                switch (filtro.Key)
                {
                    case "criticidad":
                        condiciones.Add($"{filtro.Key} = {nombreParametro}");
                        parametros.Add(nombreParametro, int.Parse(filtro.Value));
                        break;

                    case "modulo":
                        condiciones.Add($"{filtro.Key} = {nombreParametro}");
                        parametros.Add(nombreParametro, filtro.Value.ToString());
                        break;

                    case "evento":
                        condiciones.Add($"{filtro.Key} = {nombreParametro}");
                        parametros.Add(nombreParametro, filtro.Value.ToString());
                        break;

                    case "login":
                        condiciones.Add($"{filtro.Key} = {nombreParametro}");
                        parametros.Add(nombreParametro, filtro.Value.ToString());
                        break;

                    case "fechaInicio":
                        condiciones.Add("fecha >= @fechaInicio");
                        parametros.Add("@fechaInicio", DateTime.Parse(filtro.Value));
                        break;

                    case "fechaFin":
                        condiciones.Add("fecha <= @fechaFin");
                        parametros.Add("@fechaFin", DateTime.Parse(filtro.Value));
                        break;

                    default:
                        condiciones.Add($"{filtro.Key} = {nombreParametro}");
                        parametros.Add(nombreParametro, filtro.Value);
                        break;
                }
            }

            if (condiciones.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", condiciones);
            }

            return dal.RetornarLista(query, MapearEvento, parametros);
        }

        public void Alta(Evento evento)
        {
            evento.codigo = GenerarCodigoBitacora(evento.fecha);
            string query = "INSERT INTO BitacoraEventos " +
                         "(codigo, login, fecha, hora, modulo, evento, criticidad) " +
                         "VALUES (@codigo, @login, @fecha, @hora, @modulo, @evento, @criticidad)";
            EjecutarQueryConEntidad(evento, query);
        }

        private void EjecutarQueryConEntidad(Evento evento, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(evento, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        public List<Evento> RetornarEventos()
        {
            List<Evento> evento = dal.RetornarLista("SELECT * FROM BitacoraEventos", MapearEvento);
            return evento;
        }

        private Evento MapearEvento(SqlDataReader reader)
        {
            TimeSpan hora;
            object horaValue = reader["hora"];
            if (horaValue is TimeSpan)
            {
                hora = (TimeSpan)horaValue;
            }
            else if (horaValue is DateTime)
            {
                hora = ((DateTime)horaValue).TimeOfDay;
            }
            else
            {
                hora = TimeSpan.Parse(horaValue.ToString());
            }
            return new Evento(
                reader["codigo"].ToString(),
                reader["login"].ToString(),
                Convert.ToDateTime(reader["fecha"]),
                hora,
                reader["modulo"].ToString(),
                reader["evento"].ToString(),
                Convert.ToInt32(reader["criticidad"])
            );
        }
    }
}
