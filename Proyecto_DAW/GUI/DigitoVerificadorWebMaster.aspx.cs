using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class _Default : System.Web.UI.Page
{
    bllDigitoVerificador bllDigitoVerificador;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllDigitoVerificador = new bllDigitoVerificador();

        if (!IsPostBack)
        {
            CargarInconsistencias();
            CargarBackups();
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

    // Lista los .bak que hay en C:\BackUp del servidor
    private void CargarBackups()
    {
        ddlBackups.Items.Clear();
        string carpeta = @"C:\BackUp";

        if (Directory.Exists(carpeta))
        {
            foreach (string archivo in Directory.GetFiles(carpeta, "*.bak"))
            {
                // Texto = nombre del archivo, Value = ruta completa
                ddlBackups.Items.Add(new System.Web.UI.WebControls.ListItem(
                    Path.GetFileName(archivo), archivo));
            }
        }
    }

    protected void btnRecalcular_Click(object sender, EventArgs e)
    {
        try
        {
            //bllDigitoVerificador.CalcularDVFichaMedica();
            //bllDigitoVerificador.CalcularDVFichaIngreso();
            //bllDigitoVerificador.CalcularDVEvaluaciones();
            //bllDigitoVerificador.CalcularDVCertificadoAdopcion();
            //bllDigitoVerificador.CalcularDVCedente();
            //bllDigitoVerificador.CalcularDVAnimales();
            //bllDigitoVerificador.CalcularDVAdoptante();
            //bllDigitoVerificador.CalcularDVMedicamentos();

            // Tras recalcular, refrescamos la lista (debería quedar vacía)
            CargarInconsistencias();
            MostrarMensaje("Dígitos verificadores recalculados correctamente.");
        }
        catch (Exception ex) { MostrarMensaje(ex.Message); }
    }

    protected void btnRestore_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlBackups.SelectedItem == null || ddlBackups.SelectedValue == "")
            {
                MostrarMensaje("Seleccione un archivo .bak primero.");
                return;
            }

            // Value del DropDownList = ruta completa del .bak en el servidor
           // bllBackUpRestore.RealizarRestore(ddlBackups.SelectedValue);

            CargarInconsistencias();
            MostrarMensaje("Restore realizado correctamente.");
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