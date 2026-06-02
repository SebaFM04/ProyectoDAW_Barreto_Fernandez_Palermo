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

        if (!IsPostBack)
        {
            CargarBackups();
        }
    }

    // Muestra en el dropdown los .bak que hay en la carpeta del servidor
    private void CargarBackups()
    {
        ddlBackups.Items.Clear();
        foreach (string archivo in bllBackupRestore.ObtenerBackups())
        {
            ddlBackups.Items.Add(new ListItem(Path.GetFileName(archivo), archivo));
        }
    }

    protected void btnBackUp_Click(object sender, EventArgs e)
    {
        try
        {
            string resultado = bllBackupRestore.Backup();
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "login-alert login-alert-success";
            lblMensajeError.Text = "Back up realizado con éxito en: " + resultado;

            CargarBackups();   // refresca la lista para que aparezca el nuevo .bak
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
            if (ddlBackups.SelectedItem == null || ddlBackups.SelectedValue == "")
            {
                pnlAlerta.Visible = true;
                pnlAlerta.CssClass = "login-alert login-alert-error";
                lblMensajeError.Text = "Seleccione un archivo .bak";
                return;
            }

            // El Value del dropdown ya es la ruta completa del .bak en el servidor
            bllBackupRestore.RealizarRestore(ddlBackups.SelectedValue);

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