using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPageLogin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BloquearAcceso();
    }

    private void BloquearAcceso()
    {
        string pagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

        // Login siempre se puede ver, esté logueado o no
        if (pagina == "login.aspx")
            return;

        string[] paginasDigitoVerificador = {
            "digitoverificadorwebmaster.aspx",
            "digitoverificadoradmin.aspx",
            "digitoverificadorusuario.aspx"
        };

        if (paginasDigitoVerificador.Contains(pagina))
        {
            // Solo se puede entrar si vino del flujo de Login que detectó la inconsistencia
            bool tieneAcceso = Session["AccesoDigitoVerificador"] != null
                             && (bool)Session["AccesoDigitoVerificador"];

            if (!tieneAcceso)
            {
                Response.Redirect("MenuPrincipal.aspx");
                return;
            }

            // Se consume la bandera: solo sirve para esta única visita
            Session["AccesoDigitoVerificador"] = null;
        }
    }

    protected void btnInicio_Click(object sender, EventArgs e)
    {
        // Si hay un usuario logueado en este contexto (llegó por la
        // detección del Dígito Verificador), lo desloguea al salir.
        if (claseSession.Gestor.RetornarUsuarioSession() != null)
        {
            claseSession.Gestor.UnsetUsuario();
        }
        Response.Redirect("MenuPrincipal.aspx");
    }
}
