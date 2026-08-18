using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPageLogin : System.Web.UI.MasterPage, IObservadorIdioma
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BloquearAcceso();

        this.Unload += Page_Unload;

        if (Session["GestorIdiomaInicializado"] == null)
        {
            new bllIdioma().InicializarIdioma(1);
            Session["GestorIdiomaInicializado"] = true;
        }

        GestorIdioma.Instancia.Suscribir(this);
        GestorIdioma.Instancia.Notificar();

        if (!IsPostBack)
        {
            CargarSelectorIdioma();
        }
    }

    // ===== Selector de idioma =====

    private void CargarSelectorIdioma()
    {
        var bllIdioma = new bllIdioma();
        List<Idioma> idiomas = bllIdioma.ListarIdiomasDisponibles();

        rptIdiomas.DataSource = idiomas;
        rptIdiomas.DataBind();

        Idioma actual = idiomas.FirstOrDefault(i => i.codigo == GestorIdioma.Instancia.CodigoIdiomaActual);
        lblIdiomaActual.Text = actual != null ? actual.nombre : "Idioma";
    }

    protected void rptIdiomas_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;

        Idioma idioma = (Idioma)e.Item.DataItem;
        LinkButton lnk = (LinkButton)e.Item.FindControl("lnkIdioma");
        if (lnk != null)
        {
            lnk.CommandArgument = idioma.codigo.ToString();
        }
    }

    protected void rptIdiomas_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "CambiarIdioma") return;

        int codigoIdioma;
        bool esValido = int.TryParse(Convert.ToString(e.CommandArgument), out codigoIdioma);
        if (!esValido)
            return;

        new bllIdioma().CambiarIdioma(codigoIdioma);

        CargarSelectorIdioma();
    }

    // ===== IObservadorIdioma =====
    // Ver comentario equivalente en MasterPage.master.cs: la traducción de
    // controles se resuelve client-side vía ScriptMaster.js +
    // PageMethods.ObtenerTraducciones. Acá solo se marca el formulario actual.
    public void ActualizarIdioma()
    {
        string nombreFormulario = System.IO.Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
        if (bodyMaster != null)
        {
            bodyMaster.Attributes["data-formulario"] = nombreFormulario;
        }
    }

    // Ver comentario completo en MasterPage.master.cs. Esta master también
    // lo expone porque un PageMethod solo está disponible en la master que
    // efectivamente esté activa en el request (Login/DigitoVerificador usan
    // esta, el resto de las páginas usan MasterPage). La lógica de armado
    // vive una sola vez en bllIdioma.ObtenerTraduccionesApi.
    [WebMethod]
    public static string ObtenerTraducciones(string formulario)
    {
        return bllIdioma.ObtenerTraduccionesApi(formulario);
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        GestorIdioma.Instancia.Desuscribir(this);
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
