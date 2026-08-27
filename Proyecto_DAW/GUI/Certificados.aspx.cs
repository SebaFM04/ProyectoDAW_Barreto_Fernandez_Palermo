using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class Certificados : System.Web.UI.Page
{
    bllCertificadoAdopcion bllCertificado;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllCertificado = new bllCertificadoAdopcion();

        if (!IsPostBack)
        {
            CargarGrilla();
        }
    }

    private void CargarGrilla()
    {
        string codigoAnimalParam = Request.QueryString["codigoAnimal"];
        string dniParam = Request.QueryString["dni"];

        if (!string.IsNullOrEmpty(codigoAnimalParam) && int.TryParse(codigoAnimalParam, out int codigoAnimal))
        {
            gvCertificados.DataSource = bllCertificado.ObtenerCertificadosPorAnimal(codigoAnimal);
            lbFiltroActivo.Text = "Mostrando certificados del animal seleccionado.";
            btnVerTodos.Visible = true;
        }
        else if (!string.IsNullOrEmpty(dniParam))
        {
            gvCertificados.DataSource = bllCertificado.ObtenerCertificadosPorAdoptante(dniParam);
            lbFiltroActivo.Text = "Mostrando certificados del adoptante seleccionado.";
            btnVerTodos.Visible = true;
        }
        else
        {
            gvCertificados.DataSource = bllCertificado.RetornarCertificados();
            lbFiltroActivo.Text = "";
            btnVerTodos.Visible = false;
        }

        gvCertificados.DataBind();
    }

    protected void gvCertificados_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancelar")
        {
            try
            {
                string codigo = e.CommandArgument.ToString();
                bllCertificado.CancelarAdopcion(codigo);
                MostrarMensaje("Adopción cancelada exitosamente. El animal fue reingresado al refugio.", false);
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message, true);
            }

            CargarGrilla();
        }
    }

    private void MostrarMensaje(string mensaje, bool esError)
    {
        pnlAlerta.Visible = true;
        lbMensaje.Text = mensaje;
        pnlAlerta.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    protected void btnVerTodos_Click(object sender, EventArgs e)
    {
        Response.Redirect("Certificados.aspx");
    }
}
