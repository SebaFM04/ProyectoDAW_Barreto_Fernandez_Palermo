using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class GestionUsuarios : System.Web.UI.Page
{
    bllUsuario bllUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllUsuario = new bllUsuario();

        if (!IsPostBack)
        {
            CargarGrid();
        }
    }

    private void CargarGrid()
    {
        gvUsuarios.DataSource = bllUsuario.RetornarUsuarios();
        gvUsuarios.DataBind();
    }

    protected void gvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow fila = gvUsuarios.SelectedRow;

        txtDni.Text      = fila.Cells[1].Text;
        txtNombre.Text   = fila.Cells[3].Text;
        txtApellido.Text = fila.Cells[4].Text;
        txtEmail.Text    = fila.Cells[6].Text;
        ddlRol.SelectedValue    = fila.Cells[5].Text;
        ddlActivo.SelectedValue = fila.Cells[7].Text == "True" ? "true" : "false";
    }

    protected void btnAlta_Click(object sender, EventArgs e)
    {
        // TO DO: implementar en proxima entrega
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        // TO DO: implementar en proxima entrega
    }

    protected void btnDesbloquear_Click(object sender, EventArgs e)
    {
        // TO DO: implementar en proxima entrega
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtDni.Text      = "";
        txtNombre.Text   = "";
        txtApellido.Text = "";
        txtEmail.Text    = "";
        ddlRol.SelectedIndex    = 0;
        ddlActivo.SelectedIndex = 0;
        gvUsuarios.SelectedIndex = -1;
        pnlAlerta.Visible = false;
    }
}
