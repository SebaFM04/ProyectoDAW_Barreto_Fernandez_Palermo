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
        if (!IsPostBack)
        {
            ControlarLogin();
            ControlarRoles();
        }
    }

    private void ControlarLogin()
    {
        if (Session["Usuario"] != null)
        {
            liLogin.Visible = false;
            liLogout.Visible = true;
        }
        else
        {
            liLogin.Visible = true;
            liLogout.Visible = false;
        }
    }

    private void ControlarRoles()
    {
        if (Session["Rol"] != null)
        {
            string rol = Session["Rol"].ToString();

            if (rol == "Admin" || rol == "Webmaster")
            {
                liAdministracion.Visible = true;
            }
            else
            {
                liAdministracion.Visible = false;
            }
        }
        else
        {
            liAdministracion.Visible = false;
        }
    }
}
