using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GestionVacuna : System.Web.UI.Page
{
    bllVacuna bllvacuna;
    bllAnimal bllanimal;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllvacuna = new bllVacuna();
        bllanimal = new bllAnimal();
        if (!IsPostBack)
        {
            CargarGrillaVacunas();
            CargarGrillaAnimales();
        }
    }

    protected void gvAnimales_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CodigoAnimal"] = gvAnimales.SelectedDataKey.Value.ToString();
    }

    protected void gvVacunas_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CodigoVacuna"] = gvVacunas.SelectedDataKey.Value.ToString();
    }

    protected void btnAlta_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        lbMensaje.Text = "";

        if (!ValidarCampos()) return;
        if (!ConvertirCampos(out DateTime fechaAplicacion, out DateTime fechaProxima)) return;

        try
        {
            int codigoAnimal = int.Parse(ViewState["CodigoAnimal"].ToString());
            bllvacuna.AltaVacuna(codigoAnimal, txtNombreVacuna.Text, fechaAplicacion, fechaProxima);
            pnlAlerta.Visible = true;
            lbMensaje.Text = "Vacuna registrada exitosamente.";
            CargarGrillaVacunas();
        }
        catch (Exception ex)
        {
            lbMensaje.Text = "Error: " + ex.Message;
        }
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        lbMensaje.Text = "";

        if (!ValidarCampos()) return;
        if (!ConvertirCampos(out DateTime fechaAplicacion, out DateTime fechaProxima)) return;

        if (ViewState["CodigoVacuna"] == null)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "Por favor, seleccione una vacuna de la lista.";
            return;
        }

        try
        {
            int codigoVacuna = int.Parse(ViewState["CodigoVacuna"].ToString());
            bllvacuna.Modificar(codigoVacuna, fechaAplicacion, fechaProxima, txtNombreVacuna.Text);
            pnlAlerta.Visible = true;
            lbMensaje.Text = "Vacuna modificada exitosamente.";
            CargarGrillaVacunas();
        }
        catch (Exception ex)
        {
            lbMensaje.Text = "Error: " + ex.Message;
        }

    }

    private bool ValidarCampos()
    {
        if (string.IsNullOrWhiteSpace(txtNombreVacuna.Text) ||
            string.IsNullOrWhiteSpace(txtFechaAplicacion.Text) ||
            string.IsNullOrWhiteSpace(txtFechaAplicacionProxima.Text))
        {
            lbMensaje.Text = "Por favor, complete todos los campos.";
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            return false;
        }

        DateTime fechaAplicacion = DateTime.Parse(txtFechaAplicacion.Text);
        DateTime fechaProxima = DateTime.Parse(txtFechaAplicacionProxima.Text);

        if (fechaAplicacion >= fechaProxima)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "La fecha de aplicación debe ser menor a la próxima aplicación.";
            return false;
        }

        return true;
    }

    private bool ConvertirCampos(out DateTime fechaAplicacion, out DateTime fechaProxima)
    {
        fechaAplicacion = DateTime.MinValue; 
        fechaProxima = DateTime.MinValue;
        pnlAlerta.CssClass = "alert alert-exito";
        if (!DateTime.TryParse(txtFechaAplicacion.Text, out fechaAplicacion))
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "La fecha de aplicación no es válida.";
            return false;
        }
        if (!DateTime.TryParse(txtFechaAplicacionProxima.Text, out fechaProxima))
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "La fecha próxima de aplicación no es válida.";
            return false;
        }
        return true;
    }

    private void CargarGrillaVacunas()
    {
        List<Vacuna> listaVacunas = bllvacuna.RetornarVacunas();

        gvVacunas.DataSource = listaVacunas;
        gvVacunas.DataBind();
    }

    private void CargarGrillaAnimales()
    {
        List<Animal> listaAnimales = bllanimal.RetornarAnimales();

        gvAnimales.DataSource = listaAnimales;
        gvAnimales.DataBind();
    }
}