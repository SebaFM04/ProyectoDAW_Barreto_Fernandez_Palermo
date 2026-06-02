using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class Certificados : System.Web.UI.Page
{
    bllAnimal bllAnimal;
    bllUsuario bllUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllAnimal  = new bllAnimal();
        bllUsuario = new bllUsuario();

        if (!IsPostBack)
        {
            CargarGridAnimales();
            CargarGridAdoptantes();
        }
    }

    private void CargarGridAnimales()
    {
        gvAnimales.DataSource = bllAnimal.RetornarAnimales();
        gvAnimales.DataBind();
    }

    private void CargarGridAdoptantes()
    {
        // Filtra usuarios con rol adoptante
        var adoptantes = bllUsuario.RetornarUsuarios()
            .FindAll(u => u.rol.Equals("adoptante", StringComparison.OrdinalIgnoreCase));

        gvAdoptantes.DataSource = adoptantes;
        gvAdoptantes.DataBind();
    }

    protected void gvCertificados_SelectedIndexChanged(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void gvAdoptantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void gvAnimales_SelectedIndexChanged(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void btnGenerarCertificado_Click(object sender, EventArgs e)
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

    protected void btnReporte_Click(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }
}
