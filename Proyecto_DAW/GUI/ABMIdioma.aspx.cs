using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ABMIdioma : System.Web.UI.Page
{
    bllIdioma bllIdioma;

    // Nombres de formulario == nombre de archivo .aspx sin extensión, que es
    // la misma clave que usa GestorIdioma (ver MasterPage.master.cs,
    // AplicarTraduccionRecursiva). Si se agrega una página nueva al proyecto,
    // hay que sumarla acá para poder cargarle traducciones.
    private static readonly string[] FORMULARIOS = new[]
    {
        "Adoptantes", "BackupRestore", "Bitacora", "Certificados",
        "DigitoVerificadorAdmin", "DigitoVerificadorUsuario", "DigitoVerificadorWebMaster",
        "GestionIntermediaVacunaAnimal", "GestionUsuarios", "GestionVacuna",
        "ListadoAnimales", "Login", "MenuPrincipal", "RegistroAnimales", "Solicitudes"
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        bllIdioma = new bllIdioma();

        if (!IsPostBack)
        {
            CargarGrillaIdiomas();
            CargarComboFormularios();
            DeshabilitarFormulario();
        }
    }

    // ===== ABM de Idioma =====

    private void CargarGrillaIdiomas()
    {
        gvIdiomas.DataSource = bllIdioma.ListarIdiomas();
        gvIdiomas.DataBind();
    }

    private void CargarComboFormularios()
    {
        ddlFormulario.Items.Clear();
        foreach (var formulario in FORMULARIOS)
            ddlFormulario.Items.Add(new ListItem(formulario, formulario));
    }

    private void DeshabilitarFormulario()
    {
        txtNombre.Enabled = false;
        btnAplicar.Visible = false;
        btnToggleDisponibilidad.Enabled = (ViewState["CodigoIdioma"] != null);
    }

    private void HabilitarFormulario()
    {
        txtNombre.Enabled = true;
        btnAplicar.Visible = true;
    }

    protected void gvIdiomas_SelectedIndexChanged(object sender, EventArgs e)
    {
        int codigoIdioma = Convert.ToInt32(gvIdiomas.DataKeys[gvIdiomas.SelectedIndex].Value);
        string nombre = gvIdiomas.SelectedRow.Cells[1].Text;

        ViewState["CodigoIdioma"] = codigoIdioma;
        txtNombre.Text = nombre;
        DeshabilitarFormulario();

        MostrarTraduccionesDeIdioma(codigoIdioma, nombre);
    }

    // Alta de un idioma nuevo, disponible para cualquier usuario que tenga
    // acceso a esta pantalla. El idioma arranca sin ninguna traducción
    // cargada: bllIdioma.AgregarIdioma no toca la tabla Traduccion, así que
    // GestorIdioma.Traducir() va a devolver "[NombreControl]" para todo
    // hasta que alguien las complete desde el panel de abajo.
    protected void btnAlta_Click(object sender, EventArgs e)
    {
        ViewState["Modo"] = "Alta";
        ViewState["CodigoIdioma"] = null;
        txtNombre.Text = "";
        pnlTraducciones.Visible = false;
        HabilitarFormulario();
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        if (ViewState["CodigoIdioma"] == null)
        {
            MostrarError("Por favor, seleccione un idioma de la lista.");
            return;
        }
        ViewState["Modo"] = "Modificar";
        HabilitarFormulario();
    }

    protected void btnAplicar_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;

        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            MostrarError("Por favor, ingrese un nombre de idioma.");
            return;
        }

        try
        {
            string modo = ViewState["Modo"].ToString();

            if (modo == "Alta")
            {
                int codigoNuevo = bllIdioma.AgregarIdioma(txtNombre.Text.Trim());
                MostrarExito("Idioma registrado exitosamente. Ya podés cargarle traducciones.");
                ViewState["CodigoIdioma"] = codigoNuevo;
            }
            else
            {
                int codigoIdioma = Convert.ToInt32(ViewState["CodigoIdioma"]);
                bllIdioma.ModificarNombreIdioma(codigoIdioma, txtNombre.Text.Trim());
                MostrarExito("Idioma modificado exitosamente.");
            }

            DeshabilitarFormulario();
            CargarGrillaIdiomas();
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
    }

    // Nunca borra un idioma: solo alterna isDisponible (soft-delete), para no
    // perder las traducciones ya cargadas ni afectar a usuarios que ya lo
    // tengan elegido como preferido.
    protected void btnToggleDisponibilidad_Click(object sender, EventArgs e)
    {
        if (ViewState["CodigoIdioma"] == null)
        {
            MostrarError("Por favor, seleccione un idioma de la lista.");
            return;
        }

        try
        {
            int codigoIdioma = Convert.ToInt32(ViewState["CodigoIdioma"]);
            bllIdioma.ToggleDisponibilidad(codigoIdioma);
            MostrarExito("Disponibilidad del idioma actualizada.");
            CargarGrillaIdiomas();
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
    }

    // ===== Traducciones del idioma seleccionado =====

    private void MostrarTraduccionesDeIdioma(int codigoIdioma, string nombreIdioma)
    {
        pnlTraducciones.Visible = true;
        lblIdiomaSeleccionado.Text = nombreIdioma;

        if (ddlFormulario.Items.Count > 0)
            CargarGrillaTraducciones(codigoIdioma, ddlFormulario.SelectedValue);
    }

    protected void ddlFormulario_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["CodigoIdioma"] == null) return;

        int codigoIdioma = Convert.ToInt32(ViewState["CodigoIdioma"]);
        CargarGrillaTraducciones(codigoIdioma, ddlFormulario.SelectedValue);
    }

    // Trae TODOS los controles registrados para el formulario elegido, con su
    // traducción si existe o null si todavía está pendiente (se ve como
    // "[NombreControl]" en la grilla, igual que en tiempo de ejecución).
    private void CargarGrillaTraducciones(int codigoIdioma, string nombreFormulario)
    {
        List<Traduccion> estado = bllIdioma.ObtenerEstadoTraduccionesPorFormulario(codigoIdioma, nombreFormulario);
        gvTraducciones.DataSource = estado;
        gvTraducciones.DataBind();
    }

    protected void gvTraducciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "GuardarTraduccion") return;
        if (ViewState["CodigoIdioma"] == null) return;

        int codigoControl = Convert.ToInt32(e.CommandArgument);
        int codigoIdioma = Convert.ToInt32(ViewState["CodigoIdioma"]);

        int filaIndex = ((Control)e.CommandSource).NamingContainer is GridViewRow row
            ? row.RowIndex
            : -1;

        if (filaIndex < 0) return;

        TextBox txtTexto = (TextBox)gvTraducciones.Rows[filaIndex].FindControl("txtTexto");
        string texto = txtTexto.Text.Trim();

        if (string.IsNullOrEmpty(texto))
        {
            MostrarError("El texto traducido no puede estar vacío.");
            return;
        }

        try
        {
            bllIdioma.ModificarTraduccion(codigoControl, codigoIdioma, texto);
            MostrarExito("Traducción guardada.");
            CargarGrillaTraducciones(codigoIdioma, ddlFormulario.SelectedValue);
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
    }

    // ===== Utilidades de UI =====

    private void MostrarError(string mensaje)
    {
        pnlAlerta.Visible = true;
        pnlAlerta.CssClass = "alert alert-error";
        lbMensaje.Text = mensaje;
    }

    private void MostrarExito(string mensaje)
    {
        pnlAlerta.Visible = true;
        pnlAlerta.CssClass = "alert alert-exito";
        lbMensaje.Text = mensaje;
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuPrincipal.aspx");
    }
}
