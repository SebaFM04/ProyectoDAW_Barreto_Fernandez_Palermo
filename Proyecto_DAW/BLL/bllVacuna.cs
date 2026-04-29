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

        public void AltaVacuna(int codigoAnimal, string nombreVacuna, DateTime fechaAplicacion, DateTime fechaProximaAplicacion)
        {
            Vacuna vacuna = new Vacuna(codigoAnimal, nombreVacuna, fechaAplicacion, fechaProximaAplicacion);
            dal.Alta(vacuna);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion Vacunas", "Vacuna dada de alta", 2);
        }

        public void Modificar(int codigoVacuna, DateTime fechaAplicacion, DateTime fechaProximaApliacion , string nombreVacuna = null)
        {
            Vacuna Vacuna = BuscarVacunaPorCodigo(codigoVacuna.ToString());
            if (Vacuna == null)
            {
                throw new Exception("No se encontró una vacuna con el código proporcionado.");
            }

            if (nombreVacuna != null) Vacuna.nombreVacuna = nombreVacuna;
            if (fechaAplicacion != null) Vacuna.fechaAplicacion = fechaAplicacion;
            if (fechaProximaApliacion != null) Vacuna.fechaProximaAplicacion = fechaProximaApliacion;
            
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
