using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SERVICIOS
{
    // Patrón Observer: Sujeto concreto.
    // Hay una instancia por request (HttpContext.Items): las páginas y masters de ese request
    // se suscriben y se desuscriben al terminar, así ningún observador queda vivo entre requests
    // ni se mezclan usuarios. El estado (idioma elegido) se guarda en Session.
    public class GestorIdioma : ISujetoIdioma
    {
        private const string ClaveItems = "GestorIdioma";
        private const string ClaveSession = "IdiomaActual";

        private readonly List<IObservadorIdioma> observadores = new List<IObservadorIdioma>();

        public static GestorIdioma Actual
        {
            get
            {
                HttpContext contexto = HttpContext.Current;
                GestorIdioma gestor = contexto.Items[ClaveItems] as GestorIdioma;
                if (gestor == null)
                {
                    gestor = new GestorIdioma();
                    contexto.Items[ClaveItems] = gestor;
                }
                return gestor;
            }
        }

        // Idioma actual con su diccionario; null si todavía no se eligió ninguno en la sesión
        public Idioma IdiomaActual
        {
            get
            {
                HttpContext contexto = HttpContext.Current;
                return contexto.Session == null ? null : contexto.Session[ClaveSession] as Idioma;
            }
        }

        public bool TieneIdioma
        {
            get { return IdiomaActual != null; }
        }

        public void Suscribir(IObservadorIdioma observador)
        {
            if (observador != null && !observadores.Contains(observador))
                observadores.Add(observador);
        }

        public void Desuscribir(IObservadorIdioma observador)
        {
            observadores.Remove(observador);
        }

        public void Notificar()
        {
            Idioma idioma = IdiomaActual;
            if (idioma == null) return;

            // Copia por si algún observador se desuscribe mientras se recorre
            foreach (IObservadorIdioma observador in observadores.ToList())
                observador.ActualizarIdioma(idioma);
        }

        // Cambia el estado del sujeto y avisa a todos los observadores
        public void CambiarIdioma(Idioma idioma)
        {
            if (idioma == null) return;
            HttpContext.Current.Session[ClaveSession] = idioma;
            Notificar();
        }

        // Para mensajes que se arman desde el code-behind
        public string Traducir(string clave, string porDefecto = null)
        {
            Idioma idioma = IdiomaActual;
            string texto;
            if (idioma != null && idioma.Traducciones != null && idioma.Traducciones.TryGetValue(clave, out texto))
                return texto;
            return porDefecto ?? clave;
        }

        // Mensaje de una excepción en el idioma actual (las de negocio traen clave; el resto, tal cual)
        public string TraducirExcepcion(Exception ex)
        {
            ExcepcionNegocio negocio = ex as ExcepcionNegocio;
            if (negocio == null)
                return ex.Message;

            return ExcepcionNegocio.Formatear(Traducir(negocio.Clave, negocio.TextoBase), negocio.Parametros);
        }
    }
}
