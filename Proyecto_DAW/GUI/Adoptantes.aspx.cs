using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class Adoptantes : System.Web.UI.Page
{
    bllUsuario bllUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllUsuario = new bllUsuario();

        if (!IsPostBack)
        {
            CargarGridAdoptantes();
        }
    }

    private void CargarGridAdoptantes()
    {
        // Filtra usuarios con rol adoptante
        var adoptantes = bllUsuario.RetornarUsuarios()
            .FindAll(u => u.rol.Equals("adoptante", StringComparison.OrdinalIgnoreCase));

        gvAdoptantes.DataSource = adoptantes;
        gvAdoptantes.DataBind();
    }

    protected void gvEvaluaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void gvAdoptantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void btnGenerarEvaluacion_Click(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void btnAplicar_Click(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuPrincipal.aspx");
    }
}
