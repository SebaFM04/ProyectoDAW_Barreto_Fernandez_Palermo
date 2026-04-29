using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    bllBitacora bllBitacora;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllBitacora = new bllBitacora(); 
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
        bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Usuario", "Cierre de sesión de usuario", 1);
        
        // 1. Borramos todos los datos guardados en la sesión (Usuario, Rol, etc.)
        claseSession.Gestor.UnsetUsuario();

        // 3. Redirigimos al usuario a la pantalla de Login (o al Inicio, donde prefieras)
        Response.Redirect("Login.aspx");
    }

    protected void btnBitacora_Click(object sender, EventArgs e)
    {
        Response.Redirect("Bitacora.aspx");
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        Response.Redirect("GestionAnimal.aspx");
    }
}
