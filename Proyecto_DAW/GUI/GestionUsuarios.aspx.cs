using BE;
using BLL;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GestionUsuarios : System.Web.UI.Page
{
    bllUsuario bllUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllUsuario = new bllUsuario();

        if (!IsPostBack)
        {
            CargarGrid();
            CerrarPopups();
        }
    }
    private void CerrarPopups()
    {
        pnlPopupAlta.Visible = false;
        pnlPopupContraseña.Visible = false;
    }
    private void CargarGrid()
    {
        gvUsuarios.DataSource = bllUsuario.RetornarUsuarios();
        gvUsuarios.DataBind();
    }

    protected void gvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        CerrarPopups();

        GridViewRow fila = gvUsuarios.SelectedRow;

        txtDni.Text = fila.Cells[1].Text;
        txtNombre.Text = fila.Cells[3].Text;
        txtApellido.Text = fila.Cells[4].Text;
        txtEmail.Text = fila.Cells[6].Text;
        txtNombreUsuario.Text = fila.Cells[2].Text;
        ddlRol.SelectedValue = fila.Cells[5].Text;
        ddlActivo.SelectedValue = fila.Cells[7].Text == "True" ? "true" : "false";
    }

    protected void btnNuevoUsuario_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        txtDniAlta.Text = txtNombreAlta.Text = txtApellidoAlta.Text = txtEmailAlta.Text = "";
        txtContraseñaAlta.Text = txtConfirmarContraseñaAlta.Text = "";
        ddlRolAlta.SelectedIndex = 0;
        pnlPopupAlta.Visible = true;
    }

    protected void btnGuardarAlta_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        if (!Page.IsValid) return;
        try
        {
            bllUsuario.Alta(txtDniAlta.Text.Trim(), txtNombreAlta.Text.Trim(), txtApellidoAlta.Text.Trim(),
                             ddlRolAlta.SelectedValue, txtEmailAlta.Text.Trim(), txtContraseñaAlta.Text);
            pnlPopupAlta.Visible = false;
            lbMensaje.Text = "Usuario dado de alta correctamente.";
            pnlAlerta.Visible = true;
            CargarGrid();
        }
        catch (Exception ex)
        {
            lbMensaje.Text = ex.Message;
            pnlAlerta.Visible = true;
        }
    }

    protected void btnCancelarAlta_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        pnlPopupAlta.Visible = false;
    }

    protected void btnCambiarContraseña_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        if (txtDni.Text.Trim() == "")
        {
            lbMensaje.Text = "Seleccioná un usuario de la grilla primero.";
            pnlAlerta.Visible = true;
            return;
        }
        hdnDniSeleccionado.Value = txtDni.Text.Trim();
        txtNuevaContraseñaPopup.Text = txtConfirmarContraseñaPopup.Text = "";
        pnlPopupContraseña.Visible = true;
    }

    protected void btnGuardarContraseña_Click(object sender, EventArgs e)
    {

        if (!Page.IsValid) return;
        try
        {
            bllUsuario.CambiarContraseñaAdmin(hdnDniSeleccionado.Value, txtNuevaContraseñaPopup.Text);
            pnlPopupContraseña.Visible = false;
            lbMensaje.Text = "Contraseña actualizada correctamente.";
            pnlAlerta.Visible = true;
        }
        catch (Exception ex)
        {
            lbMensaje.Text = ex.Message;
            pnlAlerta.Visible = true;
        }
    }

    protected void btnCancelarContraseña_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        pnlPopupContraseña.Visible = false;
    }


 

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        if (txtDni.Text.Trim() == "")
        {
            lbMensaje.Text = "Seleccioná un usuario de la grilla.";
            pnlAlerta.Visible = true;
            return;
        }
        try
        {
            bool activo = ddlActivo.SelectedValue == "true";
            bllUsuario.Modificar(txtDni.Text.Trim(), ddlRol.SelectedValue, txtEmail.Text.Trim(), activo);
            lbMensaje.Text = "Usuario modificado correctamente.";
            pnlAlerta.Visible = true;
            CargarGrid();
        }
        catch (Exception ex)
        {
            lbMensaje.Text = ex.Message;
            pnlAlerta.Visible = true;
        }
    }

    protected void btnDesbloquear_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        if (txtDni.Text.Trim() == "")
        {
            lbMensaje.Text = "Seleccioná un usuario de la grilla.";
            pnlAlerta.Visible = true;
            return;
        }
        try
        {
            bool desbloqueado = bllUsuario.Desbloquear(txtDni.Text.Trim());
            lbMensaje.Text = desbloqueado ? "Usuario desbloqueado correctamente." : "El usuario ya se encontraba desbloqueado.";
            pnlAlerta.Visible = true;
            CargarGrid();
        }
        catch (Exception ex)
        {
            lbMensaje.Text = ex.Message;
            pnlAlerta.Visible = true;
        }
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        txtDni.Text = "";
        txtNombre.Text = "";
        txtApellido.Text = "";
        txtEmail.Text = "";
        ddlRol.SelectedIndex = 0;
        ddlActivo.SelectedIndex = 0;
        gvUsuarios.SelectedIndex = -1;
        pnlAlerta.Visible = false;
    }


}
