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
    public class bllAdoptante
    {
        dalAdoptante dal;
        bllBitacora bllBitacoraEvento;
        public bllAdoptante()
        {
            dal = new dalAdoptante();
            bllBitacoraEvento = new bllBitacora();
        }

        public void Alta(string dni, string nombre, string apellido, string telefono, int edad, string domicilio, bool mascotas)
        {
            Adoptante nuevoAdoptante = new Adoptante(dni, nombre, apellido, telefono, edad, domicilio, mascotas, true);
            dal.Alta(nuevoAdoptante);
            bllBitacoraEvento.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion adoptantes", "Adoptante dado de alta", 2);
        }

        public bool ValidarDNI(string dni)
        {
            return dal.ValidarDni(dni);
        }

        public bool VerificarAdoptanteVivo(bool vivo)
        {
            return vivo;
        }

        public void Modificar(string dni, string nombre, string apellido, string telefono, int edad, string domicilio, bool mascotas)
        {
                Adoptante adoptante = BuscarAdoptantePorDNI(dni);
                adoptante.nombre = nombre;
                adoptante.apellido = apellido;
                adoptante.telefono = telefono;
                adoptante.edad = edad;
                adoptante.domicilio = domicilio;
                adoptante.mascotas = mascotas;
                dal.Modificar(adoptante);
                bllBitacoraEvento.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion adoptantes", "Adoptante modificado", 3);
           
        }

        public void ActivarDesactivar(string dni)
        {
            Adoptante adoptante = BuscarAdoptantePorDNI(dni);
            if (adoptante == null)
            {
                throw new Exception("Adoptante no encontrado");
            }

            //Invierte el valor actual del campo activo
            adoptante.activo = !adoptante.activo;
            dal.Modificar(adoptante);

            bllBitacoraEvento.Alta(
                claseSession.Gestor.RetornarUsuarioSession().nombreUsuario,
                "Gestion adoptantes",
                adoptante.activo ? "Adoptante activado" : "Adoptante desactivado",
                3
            );
        }

        public Adoptante BuscarAdoptantePorDNI(string dni)
        {
            return dal.ObtenerAdoptantePorDni(dni);
        }

        public List<Adoptante> RetornarAdoptantes()
        {
            List<Adoptante> aux = new List<Adoptante>();
            foreach (Adoptante c in dal.RetornarAdoptantes())
            {
                aux.Add(new Adoptante(c.dni, c.nombre, c.apellido, c.telefono, c.edad, c.domicilio, c.mascotas, c.activo));
            }
            return aux;
        }
    }
}
