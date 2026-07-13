using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalBackUpRestore
    {
        Acceso dal;

        public dalBackUpRestore()
        {
            dal = new Acceso();
        }

        // Devuelve la carpeta de backups propia de SQL Server
        public string ObtenerCarpetaBackup()
        {
            return dal.ObtenerCarpetaBackupSQL();
        }

        public string Backup(string backupPath)
        {
            AsegurarCarpeta(backupPath);

            string fileName = $"backUp_dawRefugio_{DateTime.Now:ddMMyy-HHmm}.bak";
            string rutaCompleta = Path.Combine(backupPath, fileName);
            string query = $@"
                BACKUP DATABASE [dawRefugio]
                TO DISK = '{rutaCompleta}'
                WITH FORMAT, INIT, NAME = 'Backup_{fileName}';
            ";
            dal.Query(query);
            return rutaCompleta;
        }

        public void RealizarRestore(string ruta)
        {
            try
            {
                dal.RestaurarBaseDatos(ruta);
            }
            catch (Exception ex) { throw new Exception("Error:" + ex.Message); }
        }

        public List<string> ListarBackups(string carpeta)
        {
            AsegurarCarpeta(carpeta);
            return Directory.GetFiles(carpeta, "*.bak").ToList();
        }

        // Crea la carpeta si no existe. Si no se puede (permisos), lanza un
        // mensaje claro en vez de dejar que explote un UnauthorizedAccessException crudo.
        private void AsegurarCarpeta(string carpeta)
        {
            try
            {
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception(
                    $"No se tienen permisos para acceder a la carpeta de backups configurada en SQL Server:\n{carpeta}\n\n" +
                    "Verificá que el BackupDirectory de SQL Server no apunte a una ruta protegida como 'Program Files'. " +
                    "Se recomienda usar una carpeta como C:\\SQLBackups."
                );
            }
        }
    }
}
