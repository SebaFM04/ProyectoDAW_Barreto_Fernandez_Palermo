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
    public class bllCertificadoAdopcion
    {
        dalCertificadoAdopcion dal;
        Acceso acceso;
        bllAnimal bllAnimal;
        bllAdoptante bllAdoptante;
        bllBitacora bllBitacora;
        bllDigitoVerificador bllDigitoVerificador;
        bllFichaDeIngreso bllFichaDeIngreso;

        public bllCertificadoAdopcion()
        {
            dal = new dalCertificadoAdopcion();
            acceso = new Acceso();
            bllAnimal = new bllAnimal();
            bllAdoptante = new bllAdoptante();
            bllBitacora = new bllBitacora();
            bllDigitoVerificador = new bllDigitoVerificador();
            bllFichaDeIngreso = new bllFichaDeIngreso();
        }

        public void CancelarAdopcion(string codigoCertificado)
        {
            CertificadoAdopcion certificado = dal.ObtenerPorCodigo(codigoCertificado);
            if (certificado == null) throw new Exception("No se encontró el certificado indicado.");
            if (!certificado.activo) throw new Exception("Este certificado ya fue cancelado anteriormente.");

            AccionSql accionCancelarCertificado = dal.ConstruirAccionCancelar(codigoCertificado);
            AccionSql accionAnimalDisponible = bllAnimal.ConstruirAccionCambiarEstadoAdopcion(certificado.codigoAnimal.ToString(), "En Adopcion");
            AccionSql accionReingreso = bllFichaDeIngreso.ConstruirAccionReingreso(certificado.codigoAnimal, "Devolución de adopción");

            List<AccionSql> acciones = new List<AccionSql> { accionCancelarCertificado, accionAnimalDisponible, accionReingreso };
            acceso.EjecutarTransaccion(acciones);

            bllDigitoVerificador.CalcularDVAnimal();
            bllDigitoVerificador.CalcularDVCertificadoAdopcion(); 
            bllDigitoVerificador.CalcularDVHistorialIngreso();
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario,
                "Gestion adopciones", "Adopción cancelada", 2);
        }

        public void RegistrarAdopcion(string dni, int codigoAnimal)
        {
            Animal animal = bllAnimal.BuscarAnimalPorCodigo(codigoAnimal.ToString());
            if (animal == null) throw new Exception("No se encontró el animal seleccionado.");
            if (animal.estadoAdopcion == "Adoptado") throw new Exception("Este animal ya fue adoptado.");

            Adoptante adoptante = bllAdoptante.BuscarAdoptantePorDNI(dni);
            if (adoptante == null) throw new Exception("No se encontró el adoptante seleccionado.");
            if (!adoptante.activo) throw new Exception("El adoptante seleccionado no está activo.");

            string codigo = dal.GenerarCodigoCertificadoUnico();

            CertificadoAdopcion certificado = new CertificadoAdopcion(
                codigo, dni, codigoAnimal, animal.especie, animal.raza, animal.nombre,
                adoptante.nombre, adoptante.apellido, DateTime.Now
            );

            // Arma las dos acciones, sin ejecutarlas todavía
            AccionSql accionAltaCertificado = dal.ConstruirAccionAlta(certificado);
            AccionSql accionMarcarAdoptado = bllAnimal.ConstruirAccionMarcarAdoptado(codigoAnimal.ToString());

            List<AccionSql> acciones = new List<AccionSql> { accionAltaCertificado, accionMarcarAdoptado };

            // Ejecuta las dos juntas: o pasan las dos, o no pasa ninguna
            acceso.EjecutarTransaccion(acciones);

            // Recién acá, con los cambios ya confirmados en la base, hacemos los efectos secundarios
            bllDigitoVerificador.CalcularDVAnimal();
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario,
                "Gestion adopciones", "Adopción registrada", 2);
            bllDigitoVerificador.CalcularDVCertificadoAdopcion();
        }

        public List<CertificadoAdopcion> RetornarCertificados()
        {
            return dal.RetornarCertificados();
        }

        public List<CertificadoAdopcion> ObtenerCertificadosPorAnimal(int codigoAnimal)
        {
            return dal.ObtenerPorCodigoAnimal(codigoAnimal);
        }

        public List<CertificadoAdopcion> ObtenerCertificadosPorAdoptante(string dni)
        {
            return dal.ObtenerPorDni(dni);
        }
    }
}
