using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    bllBackUpRestore bllBackupRestore;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        bllBackupRestore = new bllBackUpRestore();

        string backupPath = Path.Combine(
        Path.GetPathRoot(Environment.SystemDirectory),
        "BackUp"
    );

        if (!Directory.Exists(backupPath))
        {
            Directory.CreateDirectory(backupPath);
            var security = Directory.GetAccessControl(backupPath);
            security.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(
                "NT SERVICE\\MSSQLSERVER",
                System.Security.AccessControl.FileSystemRights.FullControl,
                System.Security.AccessControl.AccessControlType.Allow
            ));
            Directory.SetAccessControl(backupPath, security);
        }

        Session["backupPath"] = backupPath;
    }

    protected void btnBackUp_Click(object sender, EventArgs e)
    {
        try
        {
            string backupPath = Session["backupPath"].ToString();
            string resultado = bllBackupRestore.Backup(backupPath);

            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "login-alert login-alert-success";
            lblMensajeError.Text = "Back up realizado con éxito en la ruta: " + resultado;
        }
        catch (Exception ex)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "login-alert login-alert-error";
            lblMensajeError.Text = ex.Message;
        }
    }

    protected void btnRestore_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fuRestore.HasFile)
            {
                pnlAlerta.Visible = true;
                pnlAlerta.CssClass = "login-alert login-alert-error";
                lblMensajeError.Text = "Seleccione un archivo .bak";
                return;
            }

            if (!fuRestore.FileName.EndsWith(".bak"))
            {
                pnlAlerta.Visible = true;
                pnlAlerta.CssClass = "login-alert login-alert-error";
                lblMensajeError.Text = "El archivo debe ser .bak";
                return;
            }

            string backupPath = Session["backupPath"].ToString();
            string ruta = Path.Combine(backupPath, fuRestore.FileName);
            fuRestore.SaveAs(ruta);

            bllBackupRestore.RealizarRestore(ruta);

            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "login-alert login-alert-success";
            lblMensajeError.Text = "Restore realizado con éxito";
        }
        catch (Exception ex)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "login-alert login-alert-error";
            lblMensajeError.Text = ex.Message;
        }
    }
}