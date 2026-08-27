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
        dalAdoptante dalAdoptante;
        dalFichaDeIngreso dalFichaDeIngreso;
        dalHistorialIngreso dalHistorialIngreso;
        dalFichaMedica dalFichaMedica;
        dalCertificadoAdopcion dalCertificadoAdopcion;

        public bllDigitoVerificador()
        {
            dal = new dalDigitoVerificador();
            dalAnimal = new dalAnimal();
            dalUsuario = new dalUsuario();
            dalVacuna = new dalVacuna();
            dalIntermediaVacunaAnimal = new dalIntermediaVacunaAnimal();
            seguridad = new encriptador();
            dalAdoptante = new dalAdoptante();
            dalFichaDeIngreso = new dalFichaDeIngreso();
            dalHistorialIngreso = new dalHistorialIngreso();
            dalFichaMedica = new dalFichaMedica();
            dalCertificadoAdopcion = new dalCertificadoAdopcion();
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
                DVVacuna(),
                DVAdoptante(),
                DVFichaDeIngreso(),
                DVHistorialIngreso(),
                DVFichaMedica(),
                DVCertificadoAdopcion()
            };
        }

        public List<string> MostrarInconsistencias()
        {
            List<DigitoVerificador> dvCalculados = Calcular();

            List<string> tablasConInconsistencias = dal.CompararDigitos(dvCalculados);

            return tablasConInconsistencias.Distinct().ToList();
        }

        public DigitoVerificador DVAdoptante()
        {
            var adoptante = dalAdoptante.RetornarAdoptantes();

            string horizontal = string.Concat(adoptante.Select(x =>
                x.dni + x.nombre + x.apellido + x.telefono + x.edad + x.domicilio + x.mascotas + x.activo));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(adoptante.Select(x => x.dni)) +
                             string.Concat(adoptante.Select(x => x.nombre)) +
                             string.Concat(adoptante.Select(x => x.apellido)) +
                             string.Concat(adoptante.Select(x => x.telefono)) +
                             string.Concat(adoptante.Select(x => x.edad)) +
                             string.Concat(adoptante.Select(x => x.domicilio)) +
                             string.Concat(adoptante.Select(x => x.mascotas)) +
                             string.Concat(adoptante.Select(x => x.activo));

            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("Adoptante", horizontalHash, verticalHash);
        }

        public void CalcularDVAdoptante()
        {
            DigitoVerificador d = DVAdoptante();
            dal.Update(d);
        }

        public DigitoVerificador DVFichaDeIngreso()
        {
            var fichas = dalFichaDeIngreso.RetornarTodas();

            string horizontal = string.Concat(fichas.Select(x =>
                x.codigoFicha + x.codigoAnimal.ToString() + x.fecha));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(fichas.Select(x => x.codigoFicha)) +
                             string.Concat(fichas.Select(x => x.codigoAnimal)) +
                             string.Concat(fichas.Select(x => x.fecha));

            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("FichaDeIngreso", horizontalHash, verticalHash);
        }

        public void CalcularDVFichaDeIngreso()
        {
            DigitoVerificador d = DVFichaDeIngreso();
            dal.Update(d);
        }

        public DigitoVerificador DVHistorialIngreso()
        {
            var historial = dalHistorialIngreso.RetornarTodos();

            string horizontal = string.Concat(historial.Select(x =>
                x.codigoHistorial.ToString() + x.codigoFicha.ToString() + x.fecha.ToString() + x.motivo));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(historial.Select(x => x.codigoHistorial)) +
                             string.Concat(historial.Select(x => x.codigoFicha)) +
                             string.Concat(historial.Select(x => x.fecha)) +
                             string.Concat(historial.Select(x => x.motivo));

            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("HistorialIngreso", horizontalHash, verticalHash);
        }

        public void CalcularDVHistorialIngreso()
        {
            DigitoVerificador d = DVHistorialIngreso();
            dal.Update(d);
        }

        public DigitoVerificador DVFichaMedica()
        {
            var fichas = dalFichaMedica.RetornarTodas();

            string horizontal = string.Concat(fichas.Select(x =>
    x.codigo.ToString() + x.codigoAnimal.ToString() + x.fecha.ToString() + x.castrado.ToString() + x.dieta + x.medicamento + x.observaciones));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(fichas.Select(x => x.codigo)) +
                             string.Concat(fichas.Select(x => x.codigoAnimal)) +
                             string.Concat(fichas.Select(x => x.fecha)) +
                             string.Concat(fichas.Select(x => x.castrado)) +
                             string.Concat(fichas.Select(x => x.dieta)) +
                             string.Concat(fichas.Select(x => x.medicamento)) +
                             string.Concat(fichas.Select(x => x.observaciones));

            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("FichaMedica", horizontalHash, verticalHash);
        }

        public void CalcularDVFichaMedica()
        {
            DigitoVerificador d = DVFichaMedica();
            dal.Update(d);
        }

        public DigitoVerificador DVCertificadoAdopcion()
        {
            var certificados = dalCertificadoAdopcion.RetornarCertificados();

            string horizontal = string.Concat(certificados.Select(x =>
                x.codigo + x.dni + x.codigoAnimal + x.especie + x.raza +
                x.nombreAnimal + x.nombreAdoptante + x.apellidoAdoptante + x.fecha + x.activo));
            string horizontalHash = seguridad.GetSHA256(horizontal);

            string vertical = string.Concat(certificados.Select(x => x.codigo)) +
                             string.Concat(certificados.Select(x => x.dni)) +
                             string.Concat(certificados.Select(x => x.codigoAnimal)) +
                             string.Concat(certificados.Select(x => x.especie)) +
                             string.Concat(certificados.Select(x => x.raza)) +
                             string.Concat(certificados.Select(x => x.nombreAnimal)) +
                             string.Concat(certificados.Select(x => x.nombreAdoptante)) +
                             string.Concat(certificados.Select(x => x.apellidoAdoptante)) +
                             string.Concat(certificados.Select(x => x.fecha)) +
                             string.Concat(certificados.Select(x => x.activo));

            string verticalHash = seguridad.GetSHA256(vertical);
            return new DigitoVerificador("CertificadoAdopcion", horizontalHash, verticalHash);
        }

        public void CalcularDVCertificadoAdopcion()
        {
            DigitoVerificador d = DVCertificadoAdopcion();
            dal.Update(d);
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
