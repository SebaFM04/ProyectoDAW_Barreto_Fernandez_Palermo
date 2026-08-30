using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegistrarAdopcion : System.Web.UI.Page
{
    bllAnimal bllAnimal;
    bllAdoptante bllAdoptante;
    bllCertificadoAdopcion bllCertificado;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllAnimal = new bllAnimal();
        bllAdoptante = new bllAdoptante();
        bllCertificado = new bllCertificadoAdopcion();

        if (!IsPostBack)
        {
            CargarGrillaAnimales();
            CargarGrillaAdoptantes();
        }
    }

    private void CargarGrillaAnimales()
    {
        var disponibles = bllAnimal.RetornarAnimales().FindAll(a => a.estadoAdopcion == "En Adopcion");
        gvAnimales.DataSource = disponibles;
        gvAnimales.DataBind();
    }

    private void CargarGrillaAdoptantes()
    {
        var activos = bllAdoptante.RetornarAdoptantes().FindAll(a => a.activo);
        gvAdoptantes.DataSource = activos;
        gvAdoptantes.DataBind();
    }

    protected void gvAnimales_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["codigoAnimal"] = gvAnimales.DataKeys[gvAnimales.SelectedIndex].Value.ToString();
    }

    protected void gvAdoptantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["dni"] = gvAdoptantes.DataKeys[gvAdoptantes.SelectedIndex].Value.ToString();
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
            if (ViewState["codigoAnimal"] == null)
            {
                MostrarMensaje("Seleccione un animal de la lista.", true);
                return;
            }
            if (ViewState["dni"] == null)
            {
                MostrarMensaje("Seleccione un adoptante de la lista.", true);
                return;
            }

            int codigoAnimal = int.Parse(ViewState["codigoAnimal"].ToString());
            string dni = ViewState["dni"].ToString();

            bllCertificado.RegistrarAdopcion(dni, codigoAnimal);

            MostrarMensaje("Adopción registrada exitosamente.", false);

            ViewState["codigoAnimal"] = null;
            ViewState["dni"] = null;

            CargarGrillaAnimales();
            CargarGrillaAdoptantes();
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