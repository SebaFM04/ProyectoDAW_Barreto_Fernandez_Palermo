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


        public string Backup(string backupPath)
        {
            Directory.CreateDirectory(backupPath);
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
    }
}
