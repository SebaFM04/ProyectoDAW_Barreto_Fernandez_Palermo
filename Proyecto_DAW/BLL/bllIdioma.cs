using BE;
using DAL;
using SERVICIOS;
using SERVICIOS.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class bllIdioma
    {
        dalIdioma dal;

        public bllIdioma()
        {
            dal = new dalIdioma();
        }

        public List<Idioma> ListarIdiomas()
        {
            return dal.ListarIdiomas();
        }

        public List<Idioma> ListarIdiomasDisponibles()
        {
            return dal.ListarIdiomasDisponibles();
        }

        // Alta de un idioma nuevo por parte de cualquier usuario. Arranca sin
        // traducciones: hasta que alguien las cargue, GestorIdioma resuelve
        // cada control como "[NombreControl]".
        public int AgregarIdioma(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre del idioma no puede estar vacío.");

            if (dal.ValidarNombreIdioma(nombre.Trim()))
                throw new Exception($"El idioma '{nombre}' ya existe.");

            return dal.AgregarIdioma(nombre.Trim());
        }

        public void ModificarNombreIdioma(int codigoIdioma, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("El nombre del idioma no puede estar vacío.");

            dal.ModificarNombreIdioma(codigoIdioma, nombre.Trim());
        }

        public void ToggleDisponibilidad(int codigoIdioma)
        {
            dal.ToggleDisponibilidad(codigoIdioma);
        }

        // Activa un idioma como el actual del GestorIdioma (Sujeto del
        // Observer) para el usuario de esta sesión, y notifica a todos los
        // observadores suscriptos (MasterPage / páginas) en este request.
        public void CambiarIdioma(int codigoIdioma)
        {
            List<Traduccion> detalle = dal.ObtenerTraduccionesPorIdioma(codigoIdioma);
            Dictionary<string, string> traducciones = detalle.ToDictionary(
                t => GestorIdioma.ClaveDe(t.nombreFormulario, t.nombreControl),
                t => t.textoTraducido);

            GestorIdioma.Instancia.CambiarIdioma(codigoIdioma, traducciones);

            // Persistir preferencia del usuario logueado, si hay sesión activa
            if (claseSession.Gestor.Session())
            {
                string dni = claseSession.Gestor.RetornarUsuarioSession().dni;
                dal.ActualizarIdiomaUsuario(dni, codigoIdioma);
            }
        }

        // Inicializa el GestorIdioma al arrancar la sesión (Login o primer
        // acceso anónimo), sin depender de que haya sesión de usuario todavía.
        public void InicializarIdioma(int codigoIdioma)
        {
            List<Traduccion> detalle = dal.ObtenerTraduccionesPorIdioma(codigoIdioma);
            Dictionary<string, string> traducciones = detalle.ToDictionary(
                t => GestorIdioma.ClaveDe(t.nombreFormulario, t.nombreControl),
                t => t.textoTraducido);

            GestorIdioma.Instancia.CambiarIdioma(codigoIdioma, traducciones);
        }

        // Estado completo de traducciones de un formulario para un idioma,
        // incluyendo los controles que todavía no tienen texto cargado
        // (textoTraducido == null), pensado para una pantalla de ABM.
        public List<Traduccion> ObtenerEstadoTraduccionesPorFormulario(int codigoIdioma, string nombreFormulario)
        {
            return dal.ObtenerEstadoTraduccionesPorFormulario(codigoIdioma, nombreFormulario);
        }

        public int ObtenerOAgregarControl(string nombreControl, string nombreFormulario)
        {
            return dal.ObtenerOAgregarControl(nombreControl, nombreFormulario);
        }

        public void ModificarTraduccion(int codigoControl, int codigoIdioma, string textoTraducido)
        {
            dal.ModificarTraduccion(codigoControl, codigoIdioma, textoTraducido);

            // Si el idioma que se está editando es el activo en esta sesión,
            // recargamos el GestorIdioma para que el cambio se vea sin relogin.
            if (GestorIdioma.Instancia.CodigoIdiomaActual == codigoIdioma)
            {
                CambiarIdioma(codigoIdioma);
            }
        }

        // Traduce un mensaje dinámico (validación, confirmación, error
        // propio) armado en el code-behind. "claveMensaje" es un
        // identificador estable tipo "MSG_FALTAN_DATOS", registrado en
        // Control con ese nombreControl. "textoEspanolActual" se usa como
        // fallback si el idioma activo todavía no tiene ese mensaje
        // traducido (evita mostrarle un placeholder "[MSG_...]" al usuario
        // en medio de una validación).
        public string TraducirMensaje(string nombreFormulario, string claveMensaje, string textoEspanolActual)
        {
            return GestorIdioma.Instancia.TraducirMensaje(nombreFormulario, claveMensaje, textoEspanolActual);
        }

        // API interna consultada por el selector de idioma vía AJAX
        // (PageMethods, ver ObtenerTraducciones en MasterPage.master.cs /
        // MasterPageLogin.master.cs). El dato en sí sigue viniendo 100% de
        // la tabla Traduccion — acá solo se arma como texto con formato
        // JSON para que el navegador pueda leerlo; no hay ninguna fuente
        // de datos alternativa a la base.
        // Resultado: {"lblCodigo":"Code:","btnAlta":"Add", ...}
        // Sin dependencias de JavaScriptSerializer/Json.NET a propósito: no
        // todos los proyectos Website tienen esas referencias disponibles.
        public static string ObtenerTraduccionesApi(string nombreFormulario)
        {
            if (string.IsNullOrWhiteSpace(nombreFormulario))
                return "{}";

            int codigoIdioma = GestorIdioma.Instancia.CodigoIdiomaActual;

            var servicioIdioma = new bllIdioma();
            List<Traduccion> detalle = servicioIdioma.ObtenerEstadoTraduccionesPorFormulario(codigoIdioma, nombreFormulario);

            var sb = new StringBuilder();
            sb.Append("{");
            bool primero = true;
            foreach (Traduccion item in detalle)
            {
                if (string.IsNullOrEmpty(item.textoTraducido))
                    continue; // sin traducción cargada para este idioma: el cliente conserva el texto que ya tiene

                if (!primero) sb.Append(",");
                primero = false;

                sb.Append("\"").Append(SerializarParaApi(item.nombreControl)).Append("\":");
                sb.Append("\"").Append(SerializarParaApi(item.textoTraducido)).Append("\"");
            }
            sb.Append("}");

            return sb.ToString();
        }

        private static string SerializarParaApi(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;

            var sb = new StringBuilder();
            foreach (char c in texto)
            {
                switch (c)
                {
                    case '\"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\t': sb.Append("\\t"); break;
                    default: sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
    }
}
