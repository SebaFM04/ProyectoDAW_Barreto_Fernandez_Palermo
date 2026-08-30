using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    bllBitacora bllBitacora;

    private static readonly Dictionary<string, string[]> PaginaPermisos =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
    {
        { "RegistroAnimales.aspx",              new[] { "ANIMAL_ALTA", "ANIMAL_MODIFICAR", "ANIMAL_BAJA", "ANIMAL_EXPORTAR_XML", "ANIMAL_IMPORTAR_XML" } },
        { "GestionIntermediaVacunaAnimal.aspx", new[] { "GESTIONAR_VACUNAS", "ANIMAL_VACUNA_ALTA", "ANIMAL_VACUNA_MODIFICAR" } },
        { "Solicitudes.aspx",                   new[] { "SOLICITUD_ACEPTAR" } },
        { "Certificados.aspx",                  new[] { "CERTIFICADO_GENERAR", "CERTIFICADO_MODIFICAR", "CERTIFICADO_APLICAR", "CERTIFICADO_GENERAR_REPORTE" } },
        { "Adoptantes.aspx",                    new[] { "ADOPTANTE_ALTA", "ADOPTANTE_MODIFICAR", "ADOPTANTE_ACT_DESAC" } },
        { "GestionUsuarios.aspx",               new[] { "USUARIO_ALTA", "MODIFICAR_PASSWORD", "USUARIO_MODIFICAR", "USUARIO_DESBLOQUEAR" } },
        { "RolesFamilias.aspx",                 new[] { "ROLES_RB", "FAMILIAS_RB", "ROL_FAMILIA_ALTA", "ROL_FAMILIA_MODIFICAR", "ROL_FAMILIA_BAJA", "ROL_FAMILIA_ASIGNAR_PERMISO", "ROL_FAMILIA_DESASIGNAR_PERMISO" } },
        { "Bitacora.aspx",                      new[] { "BITACORA_APLICAR_FILTROS" } },
        { "BackupRestore.aspx",                 new[] { "REALIZAR_BACKUP", "REALIZAR_RESTORE" } },
        { "GestionVacuna.aspx",                 new[] { "VACUNA_ALTA", "VACUNA_MODIFICAR", "VACUNA_APLICAR" } },
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        bllBitacora = new bllBitacora();

        //BloquearAcceso();   // redirige según login/rol hardcodeados
        ValidarAccesoUrl();   // guardia por URL: si no corresponde, redirige y corta acá
        ControlarLogin();   // visibilidad del menú
        AplicarPermisos();   // menú + controles de la página, según los permisos del rol
        //ControlarRoles(); //para hardcodeados 
    }

    //private void BloquearAcceso()
    //{
    //    string pagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

    //    // Públicas: cualquiera (incluso sin login) las ve
    //    string[] paginasPublicas = {
    //        "menuprincipal.aspx",
    //        "listadoanimales.aspx"
    //    };

    //    bool esPublica = paginasPublicas.Contains(pagina);
    //    Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();
    //    bool logueado = usuario != null;

    //    string paginaAnterior = Session["UltimaPagina"] != null
    //        ? Session["UltimaPagina"].ToString()
    //        : "MenuPrincipal.aspx";

    //    // No logueado + página NO pública -> lo devuelvo
    //    if (!esPublica && !logueado)
    //    {
    //        Response.Redirect(paginaAnterior);
    //        return;
    //    }

    //    // ----- WEB MASTER: solo su grupo -----
    //    if (logueado && usuario.rol == "Web Master")
    //    {
    //        string[] paginasWebMaster = {
    //            "menuprincipal.aspx",
    //            "bitacora.aspx",
    //            "backup.aspx",
    //            "backuprestore.aspx",
    //            "restore.aspx",
    //            "gestionusuarios.aspx",
    //            "rolesfamilias.aspx",
    //            "login.aspx",
    //            "digitoverificadorwebmaster.aspx"
    //        };
    //        if (!esPublica && !paginasWebMaster.Contains(pagina))
    //        {
    //            Response.Redirect(paginaAnterior);
    //            return;
    //        }
    //        Session["UltimaPagina"] = pagina;
    //        return;
    //    }

    //    // ----- ADMIN: públicas + su grupo ----- AdminSuper
    //    if (logueado && usuario.rol == "AdminSuper")
    //    {
    //        string[] paginasAdmin = {
    //            "registroanimales.aspx",
    //            "gestionintermediavacunaanimal.aspx",
    //            "solicitudes.aspx",
    //            "certificados.aspx",
    //            "adoptantes.aspx",
    //            "gestionusuarios.aspx",
    //            "gestionvacuna.aspx",
    //            "rolesfamilias.aspx",
    //            "login.aspx"
    //        };
    //        if (!esPublica && !paginasAdmin.Contains(pagina))
    //        {
    //            Response.Redirect(paginaAnterior);
    //            return;
    //        }
    //        Session["UltimaPagina"] = pagina;
    //        return;
    //    }

    //    // ----- USUARIO NORMAL (cualquier otro rol): públicas + su grupo -----
    //    if (logueado && usuario.rol != "Web Master" && usuario.rol != "AdminSuper")
    //    {
    //        string[] paginasUsuario = {
    //            "registroanimales.aspx",
    //            "gestionintermediavacunaanimal.aspx",
    //            "solicitudes.aspx",
    //            "certificados.aspx",
    //            "adoptantes.aspx",
    //            "login.aspx"
    //        };

    //        if (!esPublica && !paginasUsuario.Contains(pagina))
    //        {
    //            Response.Redirect(paginaAnterior);
    //            return;
    //        }
    //        Session["UltimaPagina"] = pagina;
    //        return;
    //    }

    //    // Acceso permitido -> guardo la página como última válida
    //    Session["UltimaPagina"] = pagina;
    //}

    private void AplicarPermisos()
    {
        HashSet<string> claves = ObtenerClavesUsuario();

        PermisoHelper.AplicarPermisosMenu(navMenu, claves, PaginaPermisos); // Menú: muestro/oculto items (data-pagina) y sus dropdown padres

        PermisoHelper.AplicarPermisosControles(this.Page, claves); // Página actual: habilito/deshabilito controles (data-permiso)
    }

    public HashSet<string> ObtenerClavesUsuario()
    {
        Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();
        if (usuario == null)
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        List<string> cacheadas = Session["ClavesUsuario"] as List<string>;
        if (cacheadas != null)
            return new HashSet<string>(cacheadas, StringComparer.OrdinalIgnoreCase);

        bllRol bllRol = new bllRol();
        Rol rol = bllRol.ObtenerRoles().FirstOrDefault(r => r.Nombre == usuario.rol);

        HashSet<string> claves = (rol == null)
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : PermisoHelper.ObtenerClaves(bllRol.ObtenerAccesosPorRol(rol.ID));

        Session["ClavesUsuario"] = claves.ToList();
        return claves;
    }

    private void ValidarAccesoUrl()
    {
        string paginaActual = System.IO.Path.GetFileName(Request.CurrentExecutionFilePath);

        // Si no está en el mapa, es pública (MenuPrincipal, ListadoAnimales, ...) -> paso
        if (!PaginaPermisos.ContainsKey(paginaActual))
            return;

        Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();

        // 1) No logueado -> a Login
        if (usuario == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        // 2) Logueado pero sin permiso para esta página -> al menú
        HashSet<string> claves = ObtenerClavesUsuario();
        if (!PermisoHelper.TieneAccesoAPagina(paginaActual, claves, PaginaPermisos))
        {
            bllBitacora.Alta(usuario.nombreUsuario, "Seguridad","Intento de acceso sin permiso a " + paginaActual, 5);
            Response.Redirect("MenuPrincipal.aspx");
        }
    }

    private void ControlarLogin()
    {
        bool haySesion = claseSession.Gestor.RetornarUsuarioSession() != null;
        liLogin.Visible = !haySesion;
        liLogout.Visible = haySesion;
    }

    //private void ControlarRoles()
    //{
    //    Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();

    //    if (usuario == null)
    //    {
    //        liAnimales.Visible = true;
    //        liAdopciones.Visible = false;
    //        liRegistrarAnimales.Visible = false;
    //        liGestionVacunas.Visible = false;
    //        liAdministracion.Visible = false;
    //        return;
    //    }

    //    string rol = usuario.rol;

    //    if (rol == "Web Master")
    //    {
    //        liAnimales.Visible = false;
    //        liAdopciones.Visible = false;
    //        liRegistrarAnimales.Visible = false;
    //        liGestionVacunas.Visible = false;
    //        liAdministracion.Visible = true;
    //        liBitacora.Visible = true;
    //        liBackupRestore.Visible = true;
    //    }
    //    else if (rol == "AdminSuper")
    //    {
    //        liAnimales.Visible = true;
    //        liAdopciones.Visible = true;
    //        liRegistrarAnimales.Visible = true;
    //        liGestionVacunas.Visible = true;
    //        liAdministracion.Visible = true;
    //        liBitacora.Visible = false;
    //        liBackupRestore.Visible = false;
    //    }
    //    else
    //    {
    //        // Usuario normal
    //        liAnimales.Visible = true;
    //        liAdopciones.Visible = true;
    //        liRegistrarAnimales.Visible = true;
    //        liGestionVacunas.Visible = true;
    //        liAdministracion.Visible = false;
    //    }
    //}

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