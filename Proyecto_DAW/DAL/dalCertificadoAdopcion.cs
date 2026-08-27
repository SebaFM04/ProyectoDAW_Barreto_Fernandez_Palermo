using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalCertificadoAdopcion
    {
        Acceso dal;

        public dalCertificadoAdopcion()
        {
            dal = new Acceso();
        }

        public string GenerarCodigoCertificadoUnico()
        {
            Random random = new Random();
            string codigo;
            bool existe;
            do
            {
                int numero = random.Next(10000, 99999);
                codigo = "CERT-" + numero;
                string query = "SELECT COUNT(*) FROM CertificadoAdopcion WHERE codigo = @codigo";
                var parametros = new Dictionary<string, object> { { "@codigo", codigo } };
                object resultado = dal.EjecutarEscalar(query, parametros);
                existe = Convert.ToInt32(resultado) > 0;
            }
            while (existe);
            return codigo;
        }

        public AccionSql ConstruirAccionAlta(CertificadoAdopcion certificado)
        {
            string query = "INSERT INTO CertificadoAdopcion " +
                "(codigo, dni, codigoAnimal, especie, raza, nombreAnimal, nombreAdoptante, apellidoAdoptante, fecha, activo) " +
                "VALUES (@codigo, @dni, @codigoAnimal, @especie, @raza, @nombreAnimal, @nombreAdoptante, @apellidoAdoptante, @fecha, @activo)";
            var parametros = ParametroHelper.CrearParametros(certificado);
            return new AccionSql { Query = query, Parametros = parametros };
        }

        public AccionSql ConstruirAccionCancelar(string codigo)
        {
            string query = "UPDATE CertificadoAdopcion SET activo = 0 WHERE codigo = @codigo";
            var parametros = new Dictionary<string, object> { { "@codigo", codigo } };
            return new AccionSql { Query = query, Parametros = parametros };
        }

        public CertificadoAdopcion ObtenerPorCodigo(string codigo)
        {
            string query = "SELECT * FROM CertificadoAdopcion WHERE codigo = @codigo";
            var parametros = new Dictionary<string, object> { { "@codigo", codigo } };
            var lista = dal.RetornarLista(query, MapearCertificado, parametros);
            return lista.FirstOrDefault();
        }

        public List<CertificadoAdopcion> ObtenerPorDni(string dni)
        {
            string query = "SELECT * FROM CertificadoAdopcion WHERE dni = @dni ORDER BY fecha DESC";
            var parametros = new Dictionary<string, object> { { "@dni", dni } };
            return dal.RetornarLista(query, MapearCertificado, parametros);
        }

        public void Alta(CertificadoAdopcion certificado)
        {
            string query = "INSERT INTO CertificadoAdopcion " +
                "(codigo, dni, codigoAnimal, especie, raza, nombreAnimal, nombreAdoptante, apellidoAdoptante, fecha) " +
                "VALUES (@codigo, @dni, @codigoAnimal, @especie, @raza, @nombreAnimal, @nombreAdoptante, @apellidoAdoptante, @fecha)";
            EjecutarQueryConEntidad(certificado, query);
        }

        public List<CertificadoAdopcion> RetornarCertificados()
        {
            return dal.RetornarLista("SELECT * FROM CertificadoAdopcion ORDER BY fecha DESC", MapearCertificado);
        }

        public List<CertificadoAdopcion> ObtenerPorCodigoAnimal(int codigoAnimal)
        {
            string query = "SELECT * FROM CertificadoAdopcion WHERE codigoAnimal = @codigoAnimal ORDER BY fecha DESC";
            var parametros = new Dictionary<string, object> { { "@codigoAnimal", codigoAnimal } };
            return dal.RetornarLista(query, MapearCertificado, parametros);
        }

        private void EjecutarQueryConEntidad(CertificadoAdopcion certificado, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(certificado, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        private CertificadoAdopcion MapearCertificado(SqlDataReader reader)
        {
            return new CertificadoAdopcion(
                reader["codigo"].ToString(),
                reader["dni"].ToString(),
                Convert.ToInt32(reader["codigoAnimal"]),
                reader["especie"].ToString(),
                reader["raza"].ToString(),
                reader["nombreAnimal"].ToString(),
                reader["nombreAdoptante"].ToString(),
                reader["apellidoAdoptante"].ToString(),
                Convert.ToDateTime(reader["fecha"]),
                Convert.ToBoolean(reader["activo"])
            );
        }
    }
}
