using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GestionIdiomas : PaginaBase
{
    bllIdioma bllIdioma;
    bllTraduccion bllTraduccion;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllIdioma = new bllIdioma();
        bllTraduccion = new bllTraduccion();

        if (!IsPostBack)
        {
            CargarIdiomas();
            CargarPaginas();
            CargarTraducciones();
        }
    }

    // ===================== IDIOMAS =====================

    private void CargarIdiomas()
    {
        List<Idioma> idiomas = bllIdioma.RetornarIdiomas();
        gvIdiomas.DataSource = idiomas;
        gvIdiomas.DataBind();

        string seleccionado = ddlIdiomaTraduccion.SelectedValue;
        ddlIdiomaTraduccion.DataSource = idiomas;
        ddlIdiomaTraduccion.DataTextField = "Nombre";
        ddlIdiomaTraduccion.DataValueField = "ID";
        ddlIdiomaTraduccion.DataBind();
        if (ddlIdiomaTraduccion.Items.FindByValue(seleccionado) != null)
            ddlIdiomaTraduccion.SelectedValue = seleccionado;

        // El selector de la navbar también muestra solo los activos: se recarga
        SiteMaster master = Master as SiteMaster;
        if (master != null)
            master.CargarIdiomas();
    }

    protected void gvIdiomas_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlAlertaIdioma.Visible = false;
        ViewState["IdIdioma"] = Convert.ToInt32(gvIdiomas.DataKeys[gvIdiomas.SelectedIndex].Value);
        txtCodigo.Text = HttpUtility.HtmlDecode(gvIdiomas.SelectedRow.Cells[1].Text);
        txtNombre.Text = HttpUtility.HtmlDecode(gvIdiomas.SelectedRow.Cells[2].Text);
    }

    // Crea el idioma con lo que hay en los campos (sus textos quedan "[...]" hasta traducirlos)
    protected void btnAlta_Click(object sender, EventArgs e)
    {
        try
        {
            string codigo = txtCodigo.Text.Trim().ToLower();
            bllIdioma.Alta(codigo, txtNombre.Text);
            MostrarMensajeIdioma(Traducir("pageIdiomas_msgIdiomaCreadoPendiente", "Idioma creado. Sus textos quedaron entre [corchetes] hasta que se traduzcan."), false);

            RefrescarTodo();

            // Se deja elegido el idioma nuevo en Traducciones para empezar a traducir
            Idioma nuevo = bllIdioma.RetornarIdiomas().FirstOrDefault(i => i.Codigo == codigo);
            if (nuevo != null && ddlIdiomaTraduccion.Items.FindByValue(nuevo.ID.ToString()) != null)
            {
                ddlIdiomaTraduccion.SelectedValue = nuevo.ID.ToString();
                CargarTraducciones();
            }
        }
        catch (Exception ex)
        {
            MostrarMensajeIdioma(MensajeError(ex), true);
        }
    }

    // Guarda código y nombre del idioma seleccionado en la grilla
    protected void btnModificar_Click(object sender, EventArgs e)
    {
        if (!HayIdiomaSeleccionado()) return;
        try
        {
            bllIdioma.Modificar(IdIdiomaSeleccionado(), txtCodigo.Text, txtNombre.Text);
            MostrarMensajeIdioma(Traducir("pageIdiomas_msgIdiomaModificado", "Idioma modificado exitosamente."), false);
            RefrescarTodo();
        }
        catch (Exception ex)
        {
            MostrarMensajeIdioma(MensajeError(ex), true);
        }
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        pnlAlertaIdioma.Visible = false;
        gvIdiomas.SelectedIndex = -1;
        ViewState["IdIdioma"] = null;
        txtCodigo.Text = "";
        txtNombre.Text = "";
    }

    protected void btnBaja_Click(object sender, EventArgs e)
    {
        if (!HayIdiomaSeleccionado()) return;
        try
        {
            bllIdioma.Baja(IdIdiomaSeleccionado());
            MostrarMensajeIdioma(Traducir("pageIdiomas_msgIdiomaBaja", "Idioma dado de baja exitosamente."), false);
            RefrescarTodo();
        }
        catch (Exception ex)
        {
            MostrarMensajeIdioma(MensajeError(ex), true);
        }
    }

    protected void btnRehabilitar_Click(object sender, EventArgs e)
    {
        if (!HayIdiomaSeleccionado()) return;

        // Además del botón deshabilitado por data-permiso, se valida del lado del servidor
        if (!TienePermiso("IDIOMA_REHABILITAR"))
        {
            MostrarMensajeIdioma(Traducir("pageIdiomas_msgSinPermisoRehabilitar", "Solo un administrador puede rehabilitar idiomas."), true);
            return;
        }

        try
        {
            bllIdioma.Rehabilitar(IdIdiomaSeleccionado());
            MostrarMensajeIdioma(Traducir("pageIdiomas_msgIdiomaRehabilitado", "Idioma rehabilitado exitosamente."), false);
            RefrescarTodo();
        }
        catch (Exception ex)
        {
            MostrarMensajeIdioma(MensajeError(ex), true);
        }
    }

    protected void btnPredeterminado_Click(object sender, EventArgs e)
    {
        if (!HayIdiomaSeleccionado()) return;
        try
        {
            bllIdioma.MarcarPredeterminado(IdIdiomaSeleccionado());
            MostrarMensajeIdioma(Traducir("pageIdiomas_msgPredeterminado", "Idioma predeterminado actualizado."), false);
            RefrescarTodo();
            // Cambió el idioma de respaldo: se recarga el diccionario de la sesión
            IdiomaHelper.RecargarIdiomaActual();
        }
        catch (Exception ex)
        {
            MostrarMensajeIdioma(MensajeError(ex), true);
        }
    }

    private bool HayIdiomaSeleccionado()
    {
        if (ViewState["IdIdioma"] != null) return true;
        MostrarMensajeIdioma(Traducir("pageIdiomas_msgSeleccioneIdioma", "Por favor, seleccione un idioma de la lista."), true);
        return false;
    }

    private int IdIdiomaSeleccionado()
    {
        return (int)ViewState["IdIdioma"];
    }

    private void RefrescarTodo()
    {
        gvIdiomas.SelectedIndex = -1;
        ViewState["IdIdioma"] = null;
        txtCodigo.Text = "";
        txtNombre.Text = "";
        CargarIdiomas();
        CargarTraducciones();
    }

    private void MostrarMensajeIdioma(string mensaje, bool esError)
    {
        pnlAlertaIdioma.Visible = true;
        pnlAlertaIdioma.CssClass = esError ? "alert alert-error" : "alert alert-exito";
        lbMensajeIdioma.Text = HttpUtility.HtmlEncode(mensaje);
    }

    // ===================== TRADUCCIONES =====================

    private void CargarPaginas()
    {
        ddlPagina.Items.Clear();
        ddlPagina.Items.Add(new ListItem(Traducir("pageIdiomas_optTodas", "-- Todas --"), ""));
        foreach (string prefijo in bllTraduccion.RetornarPrefijos())
            ddlPagina.Items.Add(new ListItem(prefijo, prefijo));
    }

    private void CargarTraducciones()
    {
        gvTraducciones.SelectedIndex = -1;
        int idIdioma;
        if (!int.TryParse(ddlIdiomaTraduccion.SelectedValue, out idIdioma))
        {
            gvTraducciones.DataSource = null;
            gvTraducciones.DataBind();
            return;
        }

        gvTraducciones.DataSource = bllTraduccion.RetornarTraducciones(idIdioma, ddlPagina.SelectedValue);
        gvTraducciones.DataBind();
    }

    protected void ddlFiltroTraducciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlAlertaTraduccion.Visible = false;
        CargarTraducciones();
    }

    protected void btnGuardarTraducciones_Click(object sender, EventArgs e)
    {
        try
        {
            int idIdioma = Convert.ToInt32(ddlIdiomaTraduccion.SelectedValue);
            List<Traduccion> modificadas = new List<Traduccion>();

            foreach (GridViewRow fila in gvTraducciones.Rows)
            {
                TextBox txt = (TextBox)fila.FindControl("txtTraduccion");
                HiddenField original = (HiddenField)fila.FindControl("hfOriginal");
                if (txt.Text.Trim() == original.Value.Trim()) continue;

                DataKey claves = gvTraducciones.DataKeys[fila.RowIndex];
                modificadas.Add(new Traduccion(idIdioma, Convert.ToInt32(claves["IdEtiqueta"]), claves["Clave"].ToString(), txt.Text, null));
            }

            int cantidad = bllTraduccion.GuardarTraducciones(idIdioma, modificadas);
            CargarTraducciones();
            RecargarSiEsIdiomaEnUso(idIdioma);
            MostrarMensajeTraduccion(string.Format(Traducir("pageIdiomas_msgTraduccionesGuardadas", "Se guardaron {0} traducciones."), cantidad), false);
        }
        catch (Exception ex)
        {
            MostrarMensajeTraduccion(MensajeError(ex), true);
        }
    }

    protected void btnMarcarPendiente_Click(object sender, EventArgs e)
    {
        if (gvTraducciones.SelectedIndex < 0)
        {
            MostrarMensajeTraduccion(Traducir("pageIdiomas_msgSeleccioneTraduccion", "Por favor, seleccione una traducción de la lista."), true);
            return;
        }

        try
        {
            int idIdioma = Convert.ToInt32(ddlIdiomaTraduccion.SelectedValue);
            int idEtiqueta = Convert.ToInt32(gvTraducciones.SelectedDataKey["IdEtiqueta"]);

            bllTraduccion.MarcarPendiente(idIdioma, idEtiqueta);
            CargarTraducciones();
            RecargarSiEsIdiomaEnUso(idIdioma);
            MostrarMensajeTraduccion(Traducir("pageIdiomas_msgTraduccionPendiente", "La traducción quedó como pendiente: [texto predeterminado]."), false);
        }
        catch (Exception ex)
        {
            MostrarMensajeTraduccion(MensajeError(ex), true);
        }
    }

    // Si se editó el idioma que está usando la sesión (o el predeterminado, que es el respaldo),
    // se recarga y el sujeto notifica: la página y la navbar se re-traducen en este mismo postback.
    private void RecargarSiEsIdiomaEnUso(int idIdioma)
    {
        Idioma actual = SERVICIOS.GestorIdioma.Actual.IdiomaActual;
        Idioma predeterminado = bllIdioma.ObtenerPredeterminado();
        bool enUso = (actual != null && actual.ID == idIdioma) || (predeterminado != null && predeterminado.ID == idIdioma);
        if (enUso)
            IdiomaHelper.RecargarIdiomaActual();
    }

    private void MostrarMensajeTraduccion(string mensaje, bool esError)
    {
        pnlAlertaTraduccion.Visible = true;
        pnlAlertaTraduccion.CssClass = esError ? "alert alert-error" : "alert alert-exito";
        lbMensajeTraduccion.Text = HttpUtility.HtmlEncode(mensaje);
    }

    private bool TienePermiso(string clave)
    {
        SiteMaster master = Master as SiteMaster;
        return master != null && master.ObtenerClavesUsuario().Contains(clave);
    }
}
