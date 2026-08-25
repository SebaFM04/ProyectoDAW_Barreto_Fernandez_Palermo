using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class bllFichaDeIngreso
    {
        dalFichaDeIngreso dalFicha;
        dalHistorialIngreso dalHistorial;

        public bllFichaDeIngreso()
        {
            dalFicha = new dalFichaDeIngreso();
            dalHistorial = new dalHistorialIngreso();
        }

        public FichaDeIngreso ObtenerFichaPorAnimal(int codigoAnimal)
        {
            return dalFicha.ObtenerFichaPorCodigoAnimal(codigoAnimal);
        }

        public bool TieneFicha(int codigoAnimal)
        {
            return ObtenerFichaPorAnimal(codigoAnimal) != null;
        }

        // Primera vez que el animal ingresa
        public void CrearFichaConPrimerIngreso(int codigoAnimal, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new Exception("Debe indicar el motivo del ingreso.");

            int codigoFicha = dalFicha.GenerarCodigoFichaUnico();
            FichaDeIngreso ficha = new FichaDeIngreso(codigoFicha, codigoAnimal, DateTime.Now);
            dalFicha.Alta(ficha);

            RegistrarEnHistorial(codigoFicha, motivo);
        }

        // El animal vuelve al refugio (ya tiene ficha creada)
        public void RegistrarReingreso(int codigoAnimal, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new Exception("Debe indicar el motivo del reingreso.");

            FichaDeIngreso ficha = ObtenerFichaPorAnimal(codigoAnimal);
            if (ficha == null)
                throw new Exception("Este animal no tiene una ficha de ingreso creada.");

            RegistrarEnHistorial(ficha.codigoFicha, motivo);
        }

        private void RegistrarEnHistorial(int codigoFicha, string motivo)
        {
            int codigoHistorial = dalHistorial.GenerarCodigoHistorialUnico();
            HistorialIngreso registro = new HistorialIngreso(codigoHistorial, codigoFicha, DateTime.Now, motivo);
            dalHistorial.Alta(registro);
        }

        public List<HistorialIngreso> ObtenerHistorial(int codigoFicha)
        {
            return dalHistorial.RetornarHistorialPorFicha(codigoFicha);
        }
    }
}
