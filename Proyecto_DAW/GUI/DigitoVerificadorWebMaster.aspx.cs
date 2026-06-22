using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    bllDigitoVerificador bllDigitoVerificador;
    bllBackUpRestore bllBackUpRestore;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllDigitoVerificador = new bllDigitoVerificador();
        bllBackUpRestore = new bllBackUpRestore();

        if (!IsPostBack)
        {
            claseSession.Gestor.UnsetUsuario();
            CargarInconsistencias();
            CargarBackups();   // vuelve a llenar el dropdown
        }
    }

    private void CargarBackups()
    {
        ddlBackups.Items.Clear();
        foreach (string archivo in bllBackUpRestore.ObtenerBackups())
        {
            ddlBackups.Items.Add(new ListItem(System.IO.Path.GetFileName(archivo), archivo));
        }
    }

    // Llena el ListBox con las tablas inconsistentes
    private void CargarInconsistencias()
    {
        lstTablas.Items.Clear();
        foreach (var s in bllDigitoVerificador.MostrarInconsistencias())
        {
            lstTablas.Items.Add(s);
        }
    }

    protected void btnRecalcular_Click(object sender, EventArgs e)
    {
        try
        {
            bllDigitoVerificador.CalcularDVAnimal();
            bllDigitoVerificador.CalcularDVIntermediaVacunaAnimal();
            bllDigitoVerificador.CalcularDVVacuna();
            bllDigitoVerificador.CalcularDVUsuario();

            // Cerramos la sesión en el servidor
            claseSession.Gestor.UnsetUsuario();

            // Mostramos el mensaje y, al aceptar, el navegador va al login
            string script = "alert('Dígitos verificadores recalculados correctamente.');" +
                            "window.location='Login.aspx';";
            ClientScript.RegisterStartupScript(GetType(), "redir", script, true);
        }
        catch (Exception ex) { MostrarMensaje(ex.Message); }
    }

    protected void btnRestore_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlBackups.SelectedItem == null || ddlBackups.SelectedValue == "")
            {
                MostrarMensaje("Seleccione un backup de la lista.");
                return;
            }

            // El Value del dropdown ya es la ruta completa del .bak en el servidor
            bllBackUpRestore.RealizarRestore(ddlBackups.SelectedValue);

            // Restore OK: cerramos sesión y volvemos al login
            claseSession.Gestor.UnsetUsuario();
            string script = "alert('Restore realizado correctamente.');" +
                            "window.location='Login.aspx';";
            ClientScript.RegisterStartupScript(GetType(), "redirRestore", script, true);
        }
        catch (Exception ex) { MostrarMensaje(ex.Message); }
    }

    // Reemplaza a MessageBox.Show
    private void MostrarMensaje(string mensaje)
    {
        string msg = mensaje.Replace("'", "\\'").Replace("\n", " ");
        ClientScript.RegisterStartupScript(GetType(), "msg", $"alert('{msg}');", true);
    }
}