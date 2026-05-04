using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class bllIntermediaVacunaAnimal
    {
        dalIntermediaVacunaAnimal dal;
        bllBitacora bllBitacora;

        public bllIntermediaVacunaAnimal()
        {
            dal = new dalIntermediaVacunaAnimal();
            bllBitacora = new bllBitacora();
        }

        public void Alta(string codigoVacuna, int codigoAnimal, string nombreVacuna, DateTime fechaAplicacion, DateTime fechaProximaAplicacion)
        {
            List<IntermediaVacunaAnimal> asignaciones = dal.RetornarIntermediaVacunaAnimal();

            bool tieneVigente = asignaciones.Any(a =>
                a.codigoAnimal == codigoAnimal &&
                a.codigoVacuna == codigoVacuna &&
                a.fechaProximaAplicacion >= fechaAplicacion 
            );

            if (tieneVigente)
                throw new Exception("La fecha de aplicación debe ser mayor a la fecha de próxima aplicación de la vacuna ya registrada.");

            IntermediaVacunaAnimal intermedia = new IntermediaVacunaAnimal(codigoVacuna, codigoAnimal, nombreVacuna, fechaAplicacion, fechaProximaAplicacion);
            dal.Alta(intermedia);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion vacuna-animal", "vacuna asignada a animal", 3);
        }

        public void Modificar(int codigo, DateTime fechaAplicacion, DateTime fechaProximaApliacion)
        {
            IntermediaVacunaAnimal intermedia = BuscarVacunaPorCodigo(codigo.ToString());
            if (intermedia == null)
            {
                throw new Exception("No se encontró una intermedia con el código proporcionado.");
            }

            if (fechaAplicacion != null) intermedia.fechaAplicacion = fechaAplicacion;
            if (fechaProximaApliacion != null) intermedia.fechaProximaAplicacion = fechaProximaApliacion;

            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion vacuna-animal", "intermedia modificada", 3);

            dal.Modificar(intermedia);
        }

        public bool ValidarExistenciaVacuna(string codigo)
        {
            return BuscarVacunaPorCodigo(codigo) != null;
        }

        public IntermediaVacunaAnimal BuscarVacunaPorCodigo(string codigo)
        {
            return dal.ObtenerItermediaVacunaAnimal(codigo);
        }

        public List<IntermediaVacunaAnimal> RetornarVacunas()
        {
            return dal.RetornarIntermediaVacunaAnimal();
        }
    }
}
