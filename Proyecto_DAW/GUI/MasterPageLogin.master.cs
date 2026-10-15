using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// Patrón Observer: observador que traduce la navbar mínima del Login y del Dígito Verificador
public partial class MasterPageLogin : System.Web.UI.MasterPage, IObservadorIdioma
{
    protected void Page_Init(object sender, EventArgs e)
    {
        GestorIdioma.Actual.Suscribir(this);
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        GestorIdioma.Actual.Desuscribir(this);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BloquearAcceso();
            CargarIdiomas();
        }
    }

    private void CargarIdiomas()
    {
        try
        {
            ddlIdioma.DataSource = new bllIdioma().RetornarIdiomas(soloActivos: true);
            ddlIdioma.DataTextField = "Nombre";
            ddlIdioma.DataValueField = "Codigo";
            ddlIdioma.DataBind();
            SeleccionarIdioma(IdiomaHelper.CodigoActual());
        }
        catch (System.Data.SqlClient.SqlException)
        {
            ddlIdioma.Visible = false;   // todavía no existen las tablas de idiomas
        }
    }

    private void SeleccionarIdioma(string codigo)
    {
        if (codigo != null && ddlIdioma.Items.FindByValue(codigo) != null)
            ddlIdioma.SelectedValue = codigo;
    }

    protected void ddlIdioma_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Solo en Session: en las pantallas del Dígito Verificador no se debe tocar la tabla Usuario
        IdiomaHelper.CambiarIdioma(ddlIdioma.SelectedValue, persistir: false);
    }

    public void ActualizarIdioma(Idioma idioma)
    {
        TraductorHelper.Traducir(this, idioma.Traducciones, excluir: ContentPlaceHolder1);
        SeleccionarIdioma(idioma.Codigo);
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
