using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FichaMedica : System.Web.UI.Page
{
    bllFichaMedica bllFichaMedica;
    bllAnimal bllAnimal;
    int codigoAnimal;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllFichaMedica = new bllFichaMedica();
        bllAnimal = new bllAnimal();

        if (!int.TryParse(Request.QueryString["codigoAnimal"], out codigoAnimal))
        {
            Response.Redirect("RegistroAnimales.aspx");
            return;
        }

        if (!IsPostBack)
        {
            CargarInfoAnimal();
            CargarHistorial();
        }
    }

    private void CargarInfoAnimal()
    {
        Animal animal = bllAnimal.BuscarAnimalPorCodigo(codigoAnimal.ToString());

        if (animal != null)
        {
            lbInfoAnimal.Text = $"Animal: {animal.nombre} ({animal.especie} - {animal.raza})";
        }

        // Si ya está castrado, el checkbox arranca marcado y bloqueado
        if (bllFichaMedica.YaEstaCastrado(codigoAnimal))
        {
            chkCastrado.Checked = true;
            chkCastrado.Enabled = false;
        }
    }

    private void CargarHistorial()
    {
        gvHistorial.DataSource = bllFichaMedica.ObtenerFichasPorAnimal(codigoAnimal);
        gvHistorial.DataBind();
    }

    private void MostrarMensaje(string mensaje, bool esError)
    {
        pnlAlerta.Visible = true;
        lbMensaje.Text = mensaje;
        pnlAlerta.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        try
        {
            bllFichaMedica.AltaFichaMedica(codigoAnimal, chkCastrado.Checked, txtDieta.Text, txtMedicamento.Text, txtObservaciones.Text);

            MostrarMensaje("Ficha médica registrada exitosamente.", false);

            txtDieta.Text = "";
            txtMedicamento.Text = "";
            txtObservaciones.Text = "";

            CargarInfoAnimal(); // por si quedó bloqueado el checkbox tras castrar
            CargarHistorial();
        }
        catch (Exception ex)
        {
            MostrarMensaje(ex.Message, true);
        }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegistroAnimales.aspx");
    }
}