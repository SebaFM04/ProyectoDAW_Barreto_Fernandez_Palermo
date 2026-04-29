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
    public class bllVacuna
    {
        dalVacuna dal;
        bllBitacora bllBitacora;

        public bllVacuna()
        {
            dal = new dalVacuna();
            bllBitacora = new bllBitacora();
        }

        public void AltaVacuna(string codigoVacuna, string nombreVacuna)
        {
            Vacuna vacuna = new Vacuna(codigoVacuna,nombreVacuna);
            dal.Alta(vacuna);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion Vacunas", "Vacuna dada de alta", 2);
        }

        public void Modificar(int codigoVacuna, string nombreVacuna = null)
        {
            Vacuna Vacuna = BuscarVacunaPorCodigo(codigoVacuna.ToString());
            if (Vacuna == null)
            {
                throw new Exception("No se encontró una vacuna con el código proporcionado.");
            }

            if (nombreVacuna != null) Vacuna.nombreVacuna = nombreVacuna;
            
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion Vacunas", "Vacuna modificada", 2);

            dal.Modificar(Vacuna);
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
