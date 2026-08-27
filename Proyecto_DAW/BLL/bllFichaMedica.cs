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
    public class bllFichaMedica
    {
        dalFichaMedica dal;
        bllAnimal bllAnimal;
        bllFichaDeIngreso bllFichaDeIngreso;
        bllDigitoVerificador bllDigitoVerificador;
        bllBitacora bllBitacoraEvento;

        public bllFichaMedica()
        {
            dal = new dalFichaMedica();
            bllAnimal = new bllAnimal();
            bllFichaDeIngreso = new bllFichaDeIngreso();
            bllDigitoVerificador = new bllDigitoVerificador();
            bllBitacoraEvento = new bllBitacora();
        }

        public bool YaEstaCastrado(int codigoAnimal)
        {
            FichaMedica ultima = dal.ObtenerUltimaFichaPorAnimal(codigoAnimal);
            return ultima != null && ultima.castrado;
        }

        public void AltaFichaMedica(int codigoAnimal, bool castrado, string dieta, string medicamento, string observaciones)
        {
            // Regla 1: el animal debe tener ficha de ingreso
            if (!bllFichaDeIngreso.TieneFicha(codigoAnimal))
                throw new Exception("El animal debe tener una ficha de ingreso antes de poder crear una ficha médica.");

            // Regla 2: el animal no debe estar adoptado
            if (bllAnimal.VerificarAnimalAdoptado(codigoAnimal.ToString()))
                throw new Exception("No se puede crear una ficha médica para un animal adoptado.");

            // Regla 3: castrado es de una sola vía (true -> no puede volver a false)
            bool yaEstabaCastrado = YaEstaCastrado(codigoAnimal);
            if (yaEstabaCastrado && !castrado)
                throw new Exception("El animal ya fue castrado anteriormente; este dato no se puede revertir.");

            int codigo = dal.GenerarCodigoFichaMedicaUnico();
            FichaMedica ficha = new FichaMedica(codigo, codigoAnimal, DateTime.Now, castrado, dieta, medicamento, observaciones);
            dal.Alta(ficha);
            bllBitacoraEvento.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Ficha médica", "Ficha médica dada de alta", 3);
            bllDigitoVerificador.CalcularDVFichaMedica();
        }

        public List<FichaMedica> ObtenerFichasPorAnimal(int codigoAnimal)
        {
            return dal.RetornarFichasPorAnimal(codigoAnimal);
        }
    }
}
