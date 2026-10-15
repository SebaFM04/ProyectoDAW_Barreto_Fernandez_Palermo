using DAL;
using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SERVICIOS;

namespace BLL
{
    public class bllIdioma
    {
        dalIdioma dal;
        dalTraduccion dalTraduccion;
        bllBitacora bllBitacora;

        public bllIdioma()
        {
            dal = new dalIdioma();
            dalTraduccion = new dalTraduccion();
            bllBitacora = new bllBitacora();
        }

        public void Alta(string codigo, string nombre)
        {
            codigo = (codigo ?? "").Trim().ToLower();
            nombre = (nombre ?? "").Trim();
            ValidarDatos(codigo, nombre, 0);

            dal.Alta(new Idioma(0, codigo, nombre));
            RegistrarBitacora("Idioma dado de alta: " + codigo);
        }

        public void Modificar(int id, string codigo, string nombre)
        {
            Idioma idioma = BuscarPorId(id);
            codigo = (codigo ?? "").Trim().ToLower();
            nombre = (nombre ?? "").Trim();

            if (idioma.Codigo == codigo && idioma.Nombre == nombre)
                throw new ExcepcionNegocio("bllIdioma_msgSinCambios", "No se realizaron cambios porque los datos son iguales.");

            ValidarDatos(codigo, nombre, id);

            idioma.Codigo = codigo;
            idioma.Nombre = nombre;
            dal.Modificar(idioma);
            RegistrarBitacora("Idioma modificado: " + codigo);
        }

        // Baja lógica: nunca puede quedar el sistema sin idiomas activos
        public void Baja(int id)
        {
            Idioma idioma = BuscarPorId(id);

            if (!idioma.Activo)
                throw new ExcepcionNegocio("bllIdioma_msgYaDadoDeBaja", "El idioma ya está dado de baja.");

            if (idioma.EsPredeterminado)
                throw new ExcepcionNegocio("bllIdioma_msgBajaPredeterminado", "No se puede dar de baja el idioma predeterminado. Marque otro idioma como predeterminado primero.");

            if (dal.ContarActivos() <= 1)
                throw new ExcepcionNegocio("bllIdioma_msgBajaUltimoActivo", "No se puede dar de baja el último idioma activo.");

            dal.CambiarEstado(id, false);
            RegistrarBitacora("Idioma dado de baja: " + idioma.Codigo);
        }

        public void Rehabilitar(int id)
        {
            Idioma idioma = BuscarPorId(id);

            if (idioma.Activo)
                throw new ExcepcionNegocio("bllIdioma_msgYaActivo", "El idioma ya está activo.");

            dal.CambiarEstado(id, true);
            RegistrarBitacora("Idioma rehabilitado: " + idioma.Codigo);
        }

        public void MarcarPredeterminado(int id)
        {
            Idioma idioma = BuscarPorId(id);

            if (!idioma.Activo)
                throw new ExcepcionNegocio("bllIdioma_msgPredeterminadoInactivo", "Solo un idioma activo puede ser predeterminado.");

            if (idioma.EsPredeterminado)
                throw new ExcepcionNegocio("bllIdioma_msgYaPredeterminado", "El idioma ya es el predeterminado.");

            dal.MarcarPredeterminado(id);
            RegistrarBitacora("Idioma predeterminado: " + idioma.Codigo);
        }

        public List<Idioma> RetornarIdiomas(bool soloActivos = false)
        {
            return dal.RetornarIdiomas(soloActivos);
        }

        public Idioma ObtenerPredeterminado()
        {
            return dal.ObtenerPredeterminado();
        }

        // Devuelve el idioma pedido con su diccionario de traducciones.
        // Si no existe o está inactivo, devuelve el predeterminado.
        // Las claves que no estén traducidas se completan con el texto del predeterminado.
        public Idioma ObtenerIdiomaConTraducciones(string codigo)
        {
            Idioma predeterminado = dal.ObtenerPredeterminado();
            Idioma idioma = string.IsNullOrEmpty(codigo) ? null : dal.ObtenerPorCodigo(codigo);

            if (idioma == null || !idioma.Activo)
                idioma = predeterminado;

            if (idioma == null)
                return null;

            Dictionary<string, string> traducciones = predeterminado == null
                ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                : dalTraduccion.ObtenerDiccionario(predeterminado.ID);

            if (predeterminado == null || idioma.ID != predeterminado.ID)
            {
                foreach (var par in dalTraduccion.ObtenerDiccionario(idioma.ID))
                {
                    if (!string.IsNullOrEmpty(par.Value))
                        traducciones[par.Key] = par.Value;
                }
            }

            idioma.Traducciones = traducciones;
            return idioma;
        }

        private Idioma BuscarPorId(int id)
        {
            Idioma idioma = dal.ObtenerPorId(id);
            if (idioma == null)
                throw new ExcepcionNegocio("bllIdioma_msgIdiomaNoEncontrado", "No se encontró el idioma seleccionado.");
            return idioma;
        }

        private void ValidarDatos(string codigo, string nombre, int idExcluido)
        {
            if (!Regex.IsMatch(codigo, @"^[a-z]{2,10}$"))
                throw new ExcepcionNegocio("bllIdioma_msgCodigoInvalido", "El código debe tener entre 2 y 10 letras minúsculas. Ej: es, en, pt");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ExcepcionNegocio("bllIdioma_msgNombreRequerido", "El nombre del idioma es obligatorio.");

            if (dal.ExisteCodigo(codigo, idExcluido))
                throw new ExcepcionNegocio("bllIdioma_msgCodigoExistente", "Ya existe un idioma con ese código.");

            if (dal.ExisteNombre(nombre, idExcluido))
                throw new ExcepcionNegocio("bllIdioma_msgNombreExistente", "Ya existe un idioma con ese nombre.");
        }

        private void RegistrarBitacora(string descripcion)
        {
            Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();
            if (usuario != null)
                bllBitacora.Alta(usuario.nombreUsuario, "Idiomas", descripcion, 2);
        }
    }
}
