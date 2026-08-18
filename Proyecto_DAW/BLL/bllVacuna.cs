using DAL;
using BE;
using System;
using SERVICIOS.MultiIdioma_Observer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERVICIOS;

namespace BLL
{
    public class bllVacuna
    {
        dalVacuna dal;
        bllBitacora bllBitacora;
        bllDigitoVerificador bllDigitoVerificador;

        public bllVacuna()
        {
            dal = new dalVacuna();
            bllBitacora = new bllBitacora();
            bllDigitoVerificador = new bllDigitoVerificador();
        }

        public void AltaVacuna(string codigoVacuna, string nombreVacuna)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(codigoVacuna, @"^[A-Z]{6}[0-9]{1}$"))
                throw new Exception(GestorIdioma.Msg("MSG_VACUNA_CODIGO_FORMATO_INVALIDO", "El código debe estar compuesto por 6 letras mayúsculas y un número al final. Ej: ABCDEF1"));

            if (dal.ValidarVacuna(codigoVacuna))
                throw new Exception(GestorIdioma.Msg("MSG_VACUNA_CODIGO_DUPLICADO", "Ya existe una vacuna con ese código."));

            if (dal.ValidarNombreVacuna(nombreVacuna))
                throw new Exception(GestorIdioma.Msg("MSG_VACUNA_NOMBRE_DUPLICADO", "Ya existe una vacuna con ese nombre."));

            Vacuna vacuna = new Vacuna(codigoVacuna, nombreVacuna);
            dal.Alta(vacuna);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion Vacunas", "Vacuna dada de alta", 2);
            RecalcularDigitoVacuna();
        }

        private void RecalcularDigitoVacuna()
        {
            bllDigitoVerificador.CalcularDVVacuna();
        }

        public void Modificar(string codigoVacuna, string nombreVacuna = null, bool? activo = null)
        {
            Vacuna vacuna = BuscarVacunaPorCodigo(codigoVacuna);
            if (vacuna == null)
                throw new Exception(GestorIdioma.Msg("MSG_VACUNA_NO_ENCONTRADA", "No se encontró una vacuna con el código proporcionado."));

            // Verificar si hay cambios
            bool mismoNombre = nombreVacuna == null || nombreVacuna == vacuna.nombreVacuna;
            bool mismoEstado = activo == null || activo.Value == vacuna.activo;

            if (mismoNombre && mismoEstado)
                throw new Exception(GestorIdioma.Msg("MSG_VACUNA_SIN_CAMBIOS", "No se realizaron cambios porque los datos son iguales."));

            if (nombreVacuna != null && nombreVacuna != vacuna.nombreVacuna)
            {
                if (dal.ValidarNombreVacuna(nombreVacuna))
                    throw new Exception(GestorIdioma.Msg("MSG_VACUNA_NOMBRE_DUPLICADO", "Ya existe una vacuna con ese nombre."));
            }

            if (nombreVacuna != null) vacuna.nombreVacuna = nombreVacuna;
            if (activo != null) vacuna.activo = activo.Value;

            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion Vacunas", "Vacuna modificada", 2);
            dal.Modificar(vacuna);
            RecalcularDigitoVacuna();
        }

        public void Baja(string codigoVacuna)
        {
            Vacuna vacuna = BuscarVacunaPorCodigo(codigoVacuna);
            if (vacuna == null)
                throw new Exception(GestorIdioma.Msg("MSG_VACUNA_NO_ENCONTRADA", "No se encontró una vacuna con el código proporcionado."));

            vacuna.activo = false;  // ← solo cambiás el estado

            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion Vacunas", "Vacuna dada de baja", 2);
            dal.Modificar(vacuna);
            RecalcularDigitoVacuna();
        }

        public bool ValidarExistenciaVacuna(string codigo)
        {
            return BuscarVacunaPorCodigo(codigo) != null;
        }

        public Vacuna BuscarVacunaPorCodigo(string codigo)
        {
            return dal.ObtenerVacuna(codigo);
        }

        public List<Vacuna> RetornarVacunas()
        {
            return dal.RetornarVacunas();
        }
    }
}
