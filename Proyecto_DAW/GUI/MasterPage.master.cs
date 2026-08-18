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
        // Form de escritorio. Se registra por código para no tener que tocar
        // el markup del .master.
        this.Unload += Page_Unload;

        // Un visitante anónimo (páginas públicas, antes de loguearse) no pasa
        // por Login.aspx.cs, así que el GestorIdioma de su sesión nunca se
        // inicializa ahí. Si es la primera vez en esta sesión, se carga el
        // idioma por defecto (español) para que Traducir() ya tenga datos.
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

    // Se asigna CommandArgument por código en vez de por binding declarativo
    // (CommandArgument='<%# Eval("codigo") %>'): con el binding declarativo
    // el atributo llegaba vacío al cliente (__doPostBack con el segundo
    // parámetro en blanco), lo que hacía fallar Convert.ToInt32 en
    // rptIdiomas_ItemCommand silenciosamente. Asignarlo acá, con acceso
    // tipado directo al objeto Idioma del ítem, es confiable.
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
            return; // CommandArgument vacío o inválido: no hace nada, no rompe la página

        new bllIdioma().CambiarIdioma(codigoIdioma);

        // Recargar el selector para que el label del idioma actual y el
        // resaltado del Repeater reflejen el cambio en este mismo postback.
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