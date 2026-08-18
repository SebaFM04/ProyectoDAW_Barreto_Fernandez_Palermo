using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPageLogin : System.Web.UI.MasterPage, IObservadorIdioma
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BloquearAcceso();

        // Patrón Observer: mismo mecanismo que en MasterPage.master.cs. Se
        // suscribe/notifica en cada request (incluidos postbacks, por si hay
        // un selector de idioma con AutoPostBack en esta master) y se
        // desuscribe al terminar de renderizar.
        this.Unload += Page_Unload;

        if (Session["GestorIdiomaInicializado"] == null)
        {
            new BLL.bllIdioma().InicializarIdioma(1);
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

    // Ver comentario equivalente en MasterPage.master.cs: se asigna
    // CommandArgument por código porque el binding declarativo
    // (CommandArgument='<%# Eval("codigo") %>') llegaba vacío al cliente.
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

        if (!int.TryParse(Convert.ToString(e.CommandArgument), out int codigoIdioma))
            return;

        new bllIdioma().CambiarIdioma(codigoIdioma);

        CargarSelectorIdioma();
    }

    // ===== IObservadorIdioma =====

    public void ActualizarIdioma()
    {
        string nombreFormulario = System.IO.Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
        AplicarTraduccionRecursiva(this.Page, nombreFormulario);
    }

    private void AplicarTraduccionRecursiva(System.Web.UI.Control raiz, string nombreFormulario)
    {
        var gestor = GestorIdioma.Instancia;
        foreach (System.Web.UI.Control ctrl in raiz.Controls)
        {
            // lblIdiomaActual no se traduce por este mecanismo: su texto es
            // el NOMBRE del idioma elegido (ej. "Español", "English"), lo
            // arma CargarSelectorIdioma() a partir de la tabla Idioma, no es
            // una clave de Control/Traduccion.
            if (!string.IsNullOrEmpty(ctrl.ID) && ctrl.ID != "lblIdiomaActual")
            {
                string traduccion = gestor.Traducir(nombreFormulario, ctrl.ID);
                if (ctrl is Label lbl) lbl.Text = traduccion;
                else if (ctrl is Button btn) btn.Text = traduccion;
                else if (ctrl is LinkButton lnk) lnk.Text = traduccion;
                else if (ctrl is HyperLink hl) hl.Text = traduccion;
            }
            if (ctrl.Controls.Count > 0)
                AplicarTraduccionRecursiva(ctrl, nombreFormulario);
        }
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
