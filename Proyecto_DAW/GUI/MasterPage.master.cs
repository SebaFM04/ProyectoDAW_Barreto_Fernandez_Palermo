using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ControlarLogin();
        ControlarRoles();
    }

    private void ControlarLogin()
    {
        bool haySesion = Session["UsuarioActual"] != null;

        liLogin.Visible = !haySesion;
        liLogout.Visible = haySesion;
    }

    private void ControlarRoles()
    {
        if (Session["Rol"] == null)
        {
            liAdministracion.Visible = false;
            return;
        }

        string rol = Session["Rol"].ToString().ToLower();

        liAdministracion.Visible = (rol == "admin" || rol == "webmaster");
    }

    protected void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        // 1. Borramos todos los datos guardados en la sesión (Usuario, Rol, etc.)
        claseSession.Gestor.UnsetUsuario();

        // 3. Redirigimos al usuario a la pantalla de Login (o al Inicio, donde prefieras)
        Response.Redirect("Login.aspx");
    }
}
