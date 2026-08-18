using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SERVICIOS.MultiIdioma_Observer
{
    // Sujeto concreto del patrón Observer.
    // Singleton "por sesión HTTP": cada usuario navegando el sitio tiene su
    // propio GestorIdioma en Session, para no pisar el idioma de otros
    // usuarios conectados al mismo tiempo (equivalente web del Singleton
    // por proceso que se usa en la versión WinForms).
    public class GestorIdioma : ISujetoIdioma
    {
        private const string SESSION_KEY = "GestorIdioma";

        public static GestorIdioma Instancia
        {
            get
            {
                var gestor = HttpContext.Current.Session[SESSION_KEY] as GestorIdioma;
                if (gestor == null)
                {
                    gestor = new GestorIdioma();
                    HttpContext.Current.Session[SESSION_KEY] = gestor;
                }
                return gestor;
            }
        }

        // Estado
        private int codigoIdiomaActual;
        private Dictionary<string, string> traducciones;
        private readonly List<IObservadorIdioma> observadores;

        private GestorIdioma()
        {
            codigoIdiomaActual = 1; // Español por defecto
            traducciones = new Dictionary<string, string>();
            observadores = new List<IObservadorIdioma>();
        }

        public int CodigoIdiomaActual => codigoIdiomaActual;

        // Traducción
        // Clave = "NombreFormulario|NombreControl". Si el idioma actual todavía
        // no tiene esa traducción cargada (por ejemplo, un idioma recién dado
        // de alta por un usuario), se devuelve "[NombreControl]" para dejar
        // explícito en la UI qué falta traducir.
        public string Traducir(string nombreFormulario, string nombreControl)
        {
            string clave = ClaveDe(nombreFormulario, nombreControl);
            if (traducciones.TryGetValue(clave, out string texto) && !string.IsNullOrEmpty(texto))
                return texto;

            return $"[{nombreControl}]";
        }

        public static string ClaveDe(string nombreFormulario, string nombreControl)
        {
            return $"{nombreFormulario}|{nombreControl}";
        }

        // Alias semántico de Traducir(), pensado para mensajes dinámicos
        // (validaciones, confirmaciones, errores propios) que el code-behind
        // arma en tiempo de ejecución en vez de texto fijo en el markup.
        // Usa exactamente el mismo diccionario y la misma clave compuesta
        // "Formulario|ClaveMensaje" — en Control, esa clave se registra con
        // un nombreControl del estilo "MSG_FALTAN_DATOS" en vez de un ID de
        // control de UI. Si no hay traducción cargada, en vez de "[clave]"
        // devuelve un texto de fallback provisto por el llamador, para no
        // mostrarle placeholders con corchetes a un usuario en medio de una
        // validación.
        public string TraducirMensaje(string nombreFormulario, string claveMensaje, string fallback)
        {
            string clave = ClaveDe(nombreFormulario, claveMensaje);
            if (traducciones.TryGetValue(clave, out string texto) && !string.IsNullOrEmpty(texto))
                return texto;

            return fallback;
        }

        // "Formulario" virtual para mensajes de validación que viven en la
        // BLL, no en una página .aspx concreta (la BLL no conoce ni debe
        // conocer HttpContext/Request para calcular un nombre de página).
        // La misma regla de negocio puede dispararse desde más de un
        // formulario, así que su traducción vive en un espacio aparte,
        // compartido por todas las capas de negocio.
        public const string FORMULARIO_BLL = "BLL";

        public string TraducirMensajeBLL(string claveMensaje, string fallback)
        {
            return TraducirMensaje(FORMULARIO_BLL, claveMensaje, fallback);
        }

        // Atajo estático para usar directo en un throw desde la BLL:
        //   throw new Exception(GestorIdioma.Msg("MSG_DNI_DUPLICADO",
        //       "Ya existe un adoptante con ese DNI."));
        // El texto en español se sigue escribiendo en el propio throw, como
        // fallback, para que el código siga siendo legible sin ir a buscar
        // el texto a otro lado, y para que no se rompa nada si todavía no
        // se cargó la traducción de ese mensaje en la base.
        public static string Msg(string claveMensaje, string textoEspanolFallback)
        {
            return Instancia.TraducirMensajeBLL(claveMensaje, textoEspanolFallback);
        }

        // Gestión de observadores
        public void Suscribir(IObservadorIdioma observador)
        {
            if (observador == null) return;
            if (!observadores.Contains(observador))
                observadores.Add(observador);
        }

        public void Desuscribir(IObservadorIdioma observador)
        {
            if (observador != null)
                observadores.Remove(observador);
        }

        public void Notificar()
        {
            foreach (var obs in new List<IObservadorIdioma>(observadores))
            {
                try { obs.ActualizarIdioma(); }
                catch { /* no interrumpir a los demás observadores suscriptos en este request */ }
            }
        }

        // Cambio de idioma: reemplaza el diccionario de traducciones vigente
        // (idioma sin ninguna traducción cargada todavía = diccionario vacío,
        // y Traducir() resuelve cada clave a "[NombreControl]" automáticamente)
        // y notifica a todos los observadores suscriptos en el request actual.
        public void CambiarIdioma(int codigoIdioma, Dictionary<string, string> nuevasTraducciones)
        {
            codigoIdiomaActual = codigoIdioma;
            traducciones = nuevasTraducciones ?? new Dictionary<string, string>();
            Notificar();
        }
    }
}
