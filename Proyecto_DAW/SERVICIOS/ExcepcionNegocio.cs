using System;

namespace SERVICIOS
{
    // Error de una regla de negocio con clave de traducción (ej. bllRol_msgPerfilSinPermisos).
    // Message queda en español (el texto base), así cualquier lugar que muestre ex.Message sigue
    // funcionando; la GUI lo traduce con GestorIdioma.TraducirExcepcion.
    public class ExcepcionNegocio : Exception
    {
        public string Clave { get; private set; }
        public string TextoBase { get; private set; }
        public object[] Parametros { get; private set; }

        public ExcepcionNegocio(string clave, string textoBase, params object[] parametros)
            : base(Formatear(textoBase, parametros))
        {
            Clave = clave;
            TextoBase = textoBase;
            Parametros = parametros ?? new object[0];
        }

        public static string Formatear(string texto, object[] parametros)
        {
            if (parametros == null || parametros.Length == 0) return texto;
            try
            {
                return string.Format(texto, parametros);
            }
            catch (FormatException)
            {
                return texto;   // traducción mal cargada (llaves desbalanceadas): no se rompe la página
            }
        }
    }
}
