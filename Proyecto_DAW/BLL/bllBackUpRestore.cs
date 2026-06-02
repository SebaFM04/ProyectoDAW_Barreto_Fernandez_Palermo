using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BLL
{
    public class bllBackUpRestore
    {
        dalBackUpRestore dal;
        bllBitacora bllBitacora;

        public bllBackUpRestore()
        {
            dal = new dalBackUpRestore();
            bllBitacora = new bllBitacora();
        }

        // Carpeta de backups propia de SQL Server (tiene permisos garantizados)
        public string ObtenerCarpetaBackups()
        {
            return dal.ObtenerCarpetaBackup();
        }

        // Lista los .bak de esa carpeta (para el dropdown si lo usás)
        public List<string> ObtenerBackups()
        {
            return dal.ListarBackups(ObtenerCarpetaBackups());
        }

        // Backup sin parámetro: usa la carpeta de SQL Server
        public string Backup()
        {
            string resultado = dal.Backup(ObtenerCarpetaBackups());
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario,
                             "Backup", "Backup realizado", 1);
            return resultado;
        }

        public void RealizarRestore(string ruta)
        {
            dal.RealizarRestore(ruta);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario,
                             "Restore", "Restore Realizado", 1);
        }
    }
}
