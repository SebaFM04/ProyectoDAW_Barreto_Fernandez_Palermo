using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string Backup(string backupPath)
        {
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Backup", "Backup realizado", 1);
            return dal.Backup(backupPath);
        }

        public void RealizarRestore(string ruta)
        {
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Restore", "Restore Realizado", 1);
            dal.RealizarRestore(ruta);
        }
    }
}
