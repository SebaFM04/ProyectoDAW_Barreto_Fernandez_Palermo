using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage, IObservadorIdioma
{
    bllBitacora bllBitacora;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllBitacora = new bllBitacora();

        BloquearAcceso();   // redirige según login/rol
        ControlarLogin();   // visibilidad del menú
        ControlarRoles();

        // Patrón Observer: el MasterPage se suscribe al GestorIdioma (Sujeto)
        // en cada request y se desuscribe al terminar de renderizar (Unload),
        // ya que en Web Forms la página no persiste entre requests como un
        // Form de escritorio.
        this.Unload += Page_Unload;

        // Un visitante anónimo (páginas públicas, antes de loguearse) no pasa
        // por Login.aspx.cs, así que el GestorIdioma de su sesión nunca se
        // inicializa ahí. Si es la primera vez en esta sesión, se carga el
        // idioma por defecto (español).
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
    //
    // El texto de los controles de UI (Label/Button/etc.) NO se pisa acá.
    // Se detectó que reasignar .Text a un asp:Label desde el servidor no
    // sobrevive hasta el render final en este proyecto (asp:Button sí,
    // porque su texto va en el atributo value en vez de como contenido).
    // En vez de seguir peleando contra el ciclo de vida de Web Forms para
    // ese caso puntual, la traducción real de los controles se resuelve
    // client-side: este método solo marca en qué formulario está parado
    // el usuario (atributo data-formulario del <body>), y
    // Scripts/ScriptMaster.js pide las traducciones de ese formulario vía
    // PageMethods.ObtenerTraducciones (ver más abajo) y las aplica directo
    // sobre el DOM. El Sujeto (GestorIdioma) y la notificación del Observer
    // siguen existiendo igual: lo que cambió es solo el paso final de
    // "cómo se pinta la traducción en pantalla".
    public void ActualizarIdioma()
    {
        string nombreFormulario = System.IO.Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
        if (bodyMaster != null)
        {
            bodyMaster.Attributes["data-formulario"] = nombreFormulario;
        }
    }

    // Endpoint AJAX invocado desde ScriptMaster.js como
    // PageMethods.ObtenerTraducciones("GestionVacuna", callback). Requiere
    // el <asp:ScriptManager EnablePageMethods="true"> agregado en el
    // markup de esta master. Un PageMethod es siempre static y no tiene
    // acceso a la instancia de la página (this, controles): por eso
    // delega toda la lógica a bllIdioma.ObtenerTraduccionesApi, que a su
    // vez usa HttpContext.Current.Session vía GestorIdioma.Instancia.
    // El dato viene siempre de la tabla Traduccion, sin excepción.
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

        // Públicas: cualquiera (incluso sin login) las ve
        string[] paginasPublicas = {
            "menuprincipal.aspx",
            "listadoanimales.aspx"
        };

        bool esPublica = paginasPublicas.Contains(pagina);
        Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();
        bool logueado = usuario != null;

        string paginaAnterior = Session["UltimaPagina"] != null
            ? Session["UltimaPagina"].ToString()
            : "MenuPrincipal.aspx";

        // No logueado + página NO pública -> lo devuelvo
        if (!esPublica && !logueado)
        {
            Response.Redirect(paginaAnterior);
            return;
        }

        // ----- WEB MASTER: solo su grupo -----
        if (logueado && usuario.rol == "web master")
        {
            string[] paginasWebMaster = {
                "menuprincipal.aspx",
                "bitacora.aspx",
                "backup.aspx",
                "backuprestore.aspx",
                "restore.aspx",
                "gestionusuarios.aspx",
                "login.aspx",
                "digitoverificadorwebmaster.aspx"
            };
            if (!esPublica && !paginasWebMaster.Contains(pagina))
            {
                Response.Redirect(paginaAnterior);
                return;
            }
            Session["UltimaPagina"] = pagina;
            return;
        }

        // ----- ADMIN: públicas + su grupo -----
        if (logueado && usuario.rol == "admin")
        {
            string[] paginasAdmin = {
                "registroanimales.aspx",
                "gestionintermediavacunaanimal.aspx",
                "solicitudes.aspx",
                "certificados.aspx",
                "adoptantes.aspx",
                "gestionusuarios.aspx",
                "gestionvacuna.aspx",
                "login.aspx"
            };
            if (!esPublica && !paginasAdmin.Contains(pagina))
            {
                Response.Redirect(paginaAnterior);
                return;
            }
            Session["UltimaPagina"] = pagina;
            return;
        }

        // ----- USUARIO NORMAL (cualquier otro rol): públicas + su grupo -----
        if (logueado && usuario.rol != "web master" && usuario.rol != "admin")
        {
            string[] paginasUsuario = {
                "registroanimales.aspx",
                "gestionintermediavacunaanimal.aspx",
                "solicitudes.aspx",
                "certificados.aspx",
                "adoptantes.aspx",
                "login.aspx"
            };

            if (!esPublica && !paginasUsuario.Contains(pagina))
            {
                Response.Redirect(paginaAnterior);
                return;
            }
            Session["UltimaPagina"] = pagina;
            return;
        }

        // Acceso permitido -> guardo la página como última válida
        Session["UltimaPagina"] = pagina;
    }

    private void ControlarLogin()
    {
        bool haySesion = claseSession.Gestor.RetornarUsuarioSession() != null;
        liLogin.Visible = !haySesion;
        liLogout.Visible = haySesion;
    }

    private void ControlarRoles()
    {
        Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();

        if (usuario == null)
        {
            liAnimales.Visible = true;
            liAdopciones.Visible = false;
            liRegistrarAnimales.Visible = false;
            liGestionVacunas.Visible = false;
            liAdministracion.Visible = false;
            return;
        }

        string rol = usuario.rol;

        if (rol == "web master")
        {
            liAnimales.Visible = false;
            liAdopciones.Visible = false;
            liRegistrarAnimales.Visible = false;
            liGestionVacunas.Visible = false;
            liAdministracion.Visible = true;
            liBitacora.Visible = true;
            liBackupRestore.Visible = true;
        }
        else if (rol == "admin")
        {
            liAnimales.Visible = true;
            liAdopciones.Visible = true;
            liRegistrarAnimales.Visible = true;
            liGestionVacunas.Visible = true;
            liAdministracion.Visible = true;
            liBitacora.Visible = false;
            liBackupRestore.Visible = false;
        }
        else
        {
            // Usuario normal
            liAnimales.Visible = true;
            liAdopciones.Visible = true;
            liRegistrarAnimales.Visible = true;
            liGestionVacunas.Visible = true;
            liAdministracion.Visible = false;
        }
    }

    protected void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario,
                         "Usuario", "Cierre de sesión de usuario", 1);

        claseSession.Gestor.UnsetUsuario();
        Response.Redirect("Login.aspx");
    }

    protected void btnBitacora_Click(object sender, EventArgs e)
    {
        Response.Redirect("Bitacora.aspx");
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegistroAnimales.aspx");
    }

    protected void btnBackupRestore_Click(object sender, EventArgs e)
    {
        Response.Redirect("BackupRestore.aspx");
    }
}