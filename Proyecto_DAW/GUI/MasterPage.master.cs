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

        // Login / Logout
        liLogin.Visible = !haySesion;
        liLogout.Visible = haySesion;

        // Elementos que requieren sesion
        liRegistrarAnimales.Visible = haySesion;
        liGestionVacunas.Visible = haySesion;
        liAdopciones.Visible = haySesion;
        liRegistrarAnimales.Visible = haySesion;
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

        claseSession.Gestor.UnsetUsuario();

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
