using BE;
using BLL;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GestionUsuarios : PaginaBase
{
    bllUsuario bllUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllUsuario = new bllUsuario();

        if (!IsPostBack)
        {
            CargarRoles();
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

    private void CargarRoles()
    {
        var roles = new bllRol().ObtenerRoles();

        // Alta
        ddlRolAlta.DataSource = roles;
        ddlRolAlta.DataTextField = "Nombre";
        ddlRolAlta.DataValueField = "Nombre";
        ddlRolAlta.DataBind();

        // Modificación
        ddlRol.DataSource = roles;
        ddlRol.DataTextField = "Nombre";
        ddlRol.DataValueField = "Nombre";
        ddlRol.DataBind();
    }

    protected void gvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        CerrarPopups();

        GridViewRow fila = gvUsuarios.SelectedRow;

        txtDni.Text = TextoCelda(fila, 1);
        txtNombre.Text = TextoCelda(fila, 3);
        txtApellido.Text = TextoCelda(fila, 4);
        txtEmail.Text = TextoCelda(fila, 6);
        txtNombreUsuario.Text = TextoCelda(fila, 2);
        string rol = TextoCelda(fila, 5);
        if (ddlRol.Items.FindByValue(rol) != null)
            ddlRol.SelectedValue = rol;
        ddlActivo.SelectedValue = fila.Cells[7].Text == "True" ? "true" : "false";
    }

    // La grilla codifica el texto en HTML ("Pérez" -> "P&#233;rez", vacío -> "&nbsp;"): se decodifica
    private string TextoCelda(GridViewRow fila, int indice)
    {
        return HttpUtility.HtmlDecode(fila.Cells[indice].Text).Replace('\u00A0', ' ').Trim();
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
            lbMensaje.Text = Traducir("pageUsuarios_msgUsuarioAlta", "Usuario dado de alta correctamente.");
            pnlAlerta.Visible = true;
            CargarGrid();
        }
        catch (Exception ex)
        {
            lbMensaje.Text = MensajeError(ex);
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
            lbMensaje.Text = Traducir("pageUsuarios_msgSeleccioneUsuarioPrimero", "Seleccioná un usuario de la grilla primero.");
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
            lbMensaje.Text = Traducir("pageUsuarios_msgContrasenaActualizada", "Contraseña actualizada correctamente.");
            pnlAlerta.Visible = true;
        }
        catch (Exception ex)
        {
            lbMensaje.Text = MensajeError(ex);
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
            lbMensaje.Text = Traducir("pageUsuarios_msgSeleccioneUsuario", "Seleccioná un usuario de la grilla.");
            pnlAlerta.Visible = true;
            return;
        }
        try
        {
            bool activo = ddlActivo.SelectedValue == "true";
            bllUsuario.Modificar(txtDni.Text.Trim(), ddlRol.SelectedValue, txtEmail.Text.Trim(), txtApellido.Text.Trim(), txtNombreUsuario.Text.Trim(), txtNombre.Text.Trim(), activo);
            lbMensaje.Text = Traducir("pageUsuarios_msgUsuarioModificado", "Usuario modificado correctamente.");
            pnlAlerta.Visible = true;
            CargarGrid();
        }
        catch (Exception ex)
        {
            lbMensaje.Text = MensajeError(ex);
            pnlAlerta.Visible = true;
        }
    }

    protected void btnDesbloquear_Click(object sender, EventArgs e)
    {
        CerrarPopups();

        if (txtDni.Text.Trim() == "")
        {
            lbMensaje.Text = Traducir("pageUsuarios_msgSeleccioneUsuario", "Seleccioná un usuario de la grilla.");
            pnlAlerta.Visible = true;
            return;
        }
        try
        {
            bool desbloqueado = bllUsuario.Desbloquear(txtDni.Text.Trim());
            lbMensaje.Text = desbloqueado ? Traducir("pageUsuarios_msgUsuarioDesbloqueado", "Usuario desbloqueado correctamente.") : Traducir("pageUsuarios_msgUsuarioYaDesbloqueado", "El usuario ya se encontraba desbloqueado.");
            pnlAlerta.Visible = true;
            CargarGrid();
        }
        catch (Exception ex)
        {
            lbMensaje.Text = MensajeError(ex);
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
        txtNombreUsuario.Text = "";
        ddlRol.SelectedIndex = 0;
        ddlActivo.SelectedIndex = 0;
        gvUsuarios.SelectedIndex = -1;
        pnlAlerta.Visible = false;
    }
}
