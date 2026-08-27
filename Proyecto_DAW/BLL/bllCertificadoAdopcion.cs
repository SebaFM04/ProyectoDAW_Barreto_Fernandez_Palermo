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
        bllAnimal bllAnimal;
        bllAdoptante bllAdoptante;
        bllBitacora bllBitacora;

        public bllCertificadoAdopcion()
        {
            dal = new dalCertificadoAdopcion();
            bllAnimal = new bllAnimal();
            bllAdoptante = new bllAdoptante();
            bllBitacora = new bllBitacora();
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

            dal.Alta(certificado);
            bllAnimal.MarcarComoAdoptado(codigoAnimal.ToString());

            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario,
                "Gestion adopciones", "Adopción registrada", 2);
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
