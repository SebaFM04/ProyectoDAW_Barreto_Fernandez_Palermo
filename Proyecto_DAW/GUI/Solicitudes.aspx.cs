using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class Solicitudes : System.Web.UI.Page
{
    bllAnimal bllAnimal;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllAnimal = new bllAnimal();

        if (!IsPostBack)
        {
            CargarGrid();
        }
    }

    private void CargarGrid()
    {
        gvAnimales.DataSource = bllAnimal.RetornarAnimales();
        gvAnimales.DataBind();
    }

    protected void gvAnimales_SelectedIndexChanged(object sender, EventArgs e)
    {
        // TODO: cargar estado actual del animal en los radio buttons
    }

    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        // TODO: implementar en proxima entrega
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuPrincipal.aspx");
    }
}
