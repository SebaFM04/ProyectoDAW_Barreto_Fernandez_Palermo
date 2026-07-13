using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BE;
using SERVICIOS;

namespace BLL
{
    public class bllDigitoVerificador
    {
        dalDigitoVerificador dal;
        dalAnimal dalAnimal;
        dalUsuario dalUsuario;
        dalVacuna dalVacuna;
        dalIntermediaVacunaAnimal dalIntermediaVacunaAnimal;
        encriptador seguridad;

        public bllDigitoVerificador()
        {
            dal = new dalDigitoVerificador();
            dalAnimal = new dalAnimal();
            dalUsuario = new dalUsuario();
            dalVacuna = new dalVacuna();
            dalIntermediaVacunaAnimal = new dalIntermediaVacunaAnimal();
            seguridad = new encriptador();
        }

        public void LimpiarAuditoria()
        {
            dal.LimpiarAuditoria();
        }

        public bool Deteccion()
        {
            List<DigitoVerificador> dvCalculados = Calcular();

            return dal.CompararDigitos(dvCalculados).Count > 0;
        }

        private List<DigitoVerificador> Calcular()
        {
            return new List<DigitoVerificador>
            {
                DVAnimal(),
                DVIntermediaVacunaAnimal(),
                DVUsuario(),
                DVVacuna()
            };
        }

        public List<string> MostrarInconsistencias()
        {
            List<DigitoVerificador> dvCalculados = Calcular();

            List<string> tablasConInconsistencias = dal.CompararDigitos(dvCalculados);

            return tablasConInconsistencias.Distinct().ToList();
        }


        public DigitoVerificador DVAnimal()
        {
            var animal = dalAnimal.RetornarAnimal();

            string horizontal = string.Concat(animal.Select(x =>
                x.codigoAnimal + x.especie + x.raza +
                x.nombre + x.tamaño + x.sexo + x.estadoAdopcion + x.vivo));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(animal.Select(x => x.codigoAnimal)) +
                             string.Concat(animal.Select(x => x.especie)) +
                             string.Concat(animal.Select(x => x.nombre)) +
                             string.Concat(animal.Select(x => x.tamaño)) +
                             string.Concat(animal.Select(x => x.sexo)) +
                             string.Concat(animal.Select(x => x.estadoAdopcion)) +
                             string.Concat(animal.Select(x => x.vivo));
            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("Animal", horizontalHash, verticalHash);
        }

        public void CalcularDVAnimal()
        {
            DigitoVerificador d = DVAnimal();
            dal.Update(d);
        }

        public DigitoVerificador DVVacuna()
        {
            var vacuna = dalVacuna.RetornarVacunas();

            string horizontal = string.Concat(vacuna.Select(x =>
                x.codigoVacuna + x.nombreVacuna + x.activo ));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(vacuna.Select(x => x.codigoVacuna)) +
                             string.Concat(vacuna.Select(x => x.nombreVacuna)) +
                             string.Concat(vacuna.Select(x => x.activo));

            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("Vacuna", horizontalHash, verticalHash);
        }

        public void CalcularDVVacuna()
        {
            DigitoVerificador d = DVVacuna();
            dal.Update(d);
        }

        public DigitoVerificador DVUsuario()
        {
            var usuario = dalUsuario.RetornarUsuarios();

            // domicilio se incluye en plain text: RetornarUsuarios() ya desencripta.
            // El DV es consistente porque siempre se calcula sobre el mismo valor (plain).
            // Si alguien altera el cifrado en BD, al desencriptar obtendrá un valor distinto
            // y el hash calculado no coincidirá con el almacenado -> inconsistencia detectada.
            string horizontal = string.Concat(usuario.Select(x =>
               x.dni + x.nombreUsuario + x.contraseña + x.nombre + x.apellido +
               x.rol + x.email + x.bloqueo + x.intentos + x.lenguaje + x.activo + x.domicilio));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(usuario.Select(x => x.dni)) +
                             string.Concat(usuario.Select(x => x.nombreUsuario)) +
                             string.Concat(usuario.Select(x => x.contraseña)) +
                             string.Concat(usuario.Select(x => x.nombre)) +
                             string.Concat(usuario.Select(x => x.apellido)) +
                             string.Concat(usuario.Select(x => x.rol)) +
                             string.Concat(usuario.Select(x => x.email)) +
                             string.Concat(usuario.Select(x => x.bloqueo)) +
                             string.Concat(usuario.Select(x => x.intentos)) +
                             string.Concat(usuario.Select(x => x.lenguaje)) +
                             string.Concat(usuario.Select(x => x.activo)) +
                             string.Concat(usuario.Select(x => x.domicilio));

            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("Usuario", horizontalHash, verticalHash);
        }

        public void CalcularDVUsuario()
        {
            DigitoVerificador d = DVUsuario();
            dal.Update(d);
        }

        public DigitoVerificador DVIntermediaVacunaAnimal()
        {
            var vacunaIntermedia = dalIntermediaVacunaAnimal.RetornarIntermediaVacunaAnimal();

            string horizontal = string.Concat(vacunaIntermedia.Select(x =>
                x.codigo + x.codigoVacuna + x.codigoAnimal +
                x.nombreVacuna + x.fechaAplicacion + x.fechaProximaAplicacion));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(vacunaIntermedia.Select(x => x.codigo)) +
                             string.Concat(vacunaIntermedia.Select(x => x.codigoVacuna)) +
                             string.Concat(vacunaIntermedia.Select(x => x.codigoAnimal)) +
                             string.Concat(vacunaIntermedia.Select(x => x.nombreVacuna)) +
                             string.Concat(vacunaIntermedia.Select(x => x.fechaAplicacion)) +
                             string.Concat(vacunaIntermedia.Select(x => x.fechaProximaAplicacion));

            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("Intermedia vacuna-animal", horizontalHash, verticalHash);
        }

        public void CalcularDVIntermediaVacunaAnimal()
        {
            DigitoVerificador d = DVIntermediaVacunaAnimal();
            dal.Update(d);
        }
    }
}
