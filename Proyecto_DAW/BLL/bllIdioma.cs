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
    }
}
