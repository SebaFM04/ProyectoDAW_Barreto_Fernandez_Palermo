using DAL;
using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVICIOS;

namespace BLL
{
    public class bllTraduccion
    {
        dalTraduccion dal;
        dalIdioma dalIdioma;
        bllBitacora bllBitacora;

        public bllTraduccion()
        {
            dal = new dalTraduccion();
            dalIdioma = new dalIdioma();
            bllBitacora = new bllBitacora();
        }

        public List<Traduccion> RetornarTraducciones(int idIdioma, string prefijo = null)
        {
            Idioma predeterminado = dalIdioma.ObtenerPredeterminado();
            int idPredeterminado = predeterminado == null ? idIdioma : predeterminado.ID;
            return dal.RetornarTraducciones(idIdioma, idPredeterminado, prefijo);
        }

        // Prefijos de página (lo que está antes del primer "_" de cada clave), para filtrar la grilla
        public List<string> RetornarPrefijos()
        {
            return dal.RetornarEtiquetas()
                      .Select(e => e.Clave.Contains("_") ? e.Clave.Substring(0, e.Clave.IndexOf('_')) : e.Clave)
                      .Distinct()
                      .OrderBy(p => p)
                      .ToList();
        }

        // Guarda solo las filas cuyo texto cambió. Devuelve cuántas se guardaron.
        // Una traducción vacía queda como pendiente: "[texto del predeterminado]".
        public int GuardarTraducciones(int idIdioma, List<Traduccion> modificadas)
        {
            Idioma idioma = dalIdioma.ObtenerPorId(idIdioma);
            if (idioma == null)
                throw new ExcepcionNegocio("bllTraduccion_msgIdiomaNoEncontrado", "No se encontró el idioma seleccionado.");

            if (modificadas == null || modificadas.Count == 0)
                throw new ExcepcionNegocio("bllTraduccion_msgSinCambios", "No se realizaron cambios en las traducciones.");

            foreach (Traduccion t in modificadas)
            {
                string texto = (t.Texto ?? "").Trim();
                if (texto.Length > 1000)
                    throw new ExcepcionNegocio("bllTraduccion_msgTextoMuyLargo", "La traducción de {0} supera los 1000 caracteres.", t.Clave);

                if (texto == "" && idioma.EsPredeterminado)
                    throw new ExcepcionNegocio("bllTraduccion_msgPredeterminadoVacio", "El idioma predeterminado no puede tener textos vacíos ({0}).", t.Clave);
            }

            foreach (Traduccion t in modificadas)
            {
                string texto = (t.Texto ?? "").Trim();

                if (texto == "")
                    dal.MarcarPendiente(idIdioma, t.IdEtiqueta);
                else
                    dal.Guardar(idIdioma, t.IdEtiqueta, texto);
            }

            RegistrarBitacora("Traducciones modificadas: " + modificadas.Count);
            return modificadas.Count;
        }

        // Vuelve a dejar la traducción como pendiente ("[texto del predeterminado]")
        public void MarcarPendiente(int idIdioma, int idEtiqueta)
        {
            Idioma idioma = dalIdioma.ObtenerPorId(idIdioma);
            if (idioma == null)
                throw new ExcepcionNegocio("bllTraduccion_msgIdiomaNoEncontrado", "No se encontró el idioma seleccionado.");

            if (idioma.EsPredeterminado)
                throw new ExcepcionNegocio("bllTraduccion_msgEliminarPredeterminado", "No se puede eliminar una traducción del idioma predeterminado; es el texto de respaldo de los demás idiomas.");

            dal.MarcarPendiente(idIdioma, idEtiqueta);
            RegistrarBitacora("Traducción marcada como pendiente");
        }

        private void RegistrarBitacora(string descripcion)
        {
            Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();
            if (usuario != null)
                bllBitacora.Alta(usuario.nombreUsuario, "Idiomas", descripcion, 2);
        }
    }
}
