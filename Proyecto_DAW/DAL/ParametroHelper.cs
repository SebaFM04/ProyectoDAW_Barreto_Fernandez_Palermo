using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class ParametroHelper
    {
        /// <summary>
        /// Crea un diccionario de parámetros a partir de las propiedades de una entidad.
        /// Si se especifica una lista de nombres de propiedades, solo esas se incluyen.
        /// </summary>
        /// <param name="entity">La entidad de la cual extraer propiedades.</param>
        /// <param name="propiedadesIncluir">Lista opcional de nombres de propiedades a incluir (en minúscula).</param>
        public static Dictionary<string, object> CrearParametros(object entity, List<string> propiedadesIncluir = null)
        {
            var parametros = new Dictionary<string, object>();

            foreach (var prop in entity.GetType().GetProperties())
            {
                string propName = prop.Name;

                // Si se pasó una lista de propiedades a incluir, y esta propiedad no está en la lista, la salteamos
                if (propiedadesIncluir != null && !propiedadesIncluir.Contains(propName))
                    continue;

                var nombreParametro = "@" + propName;
                var valor = prop.GetValue(entity) ?? DBNull.Value;

                parametros[nombreParametro] = valor;
            }

            return parametros;

        }
    }
}
