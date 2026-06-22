using BE;
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

        BloquearAcceso();   // redirige según login/rol
        ControlarLogin();   // visibilidad del menú
        ControlarRoles();
    }

    private void BloquearAcceso()
    {
        string pagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

        // Públicas: cualquiera (incluso sin login) las ve
        string[] paginasPublicas = {
            "menuprincipal.aspx",
            "listadoanimales.aspx",
            "digitoverificadorusuario.aspx",
            "digitoverificadoradmin.aspx"
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

        // ----- USUARIO NORMAL (cualquier otro rol): solo públicas -----
        if (logueado && usuario.rol != "web master" && usuario.rol != "admin")
        {
            if (!esPublica)
            {
                Response.Redirect(paginaAnterior);
                return;
            }
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
}