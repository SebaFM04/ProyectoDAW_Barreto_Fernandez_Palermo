using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    bllBitacora bll;
    protected void Page_Load(object sender, EventArgs e)
    {
        bll = new bllBitacora();
        if (!IsPostBack)
        {
            txtFechaInicio.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFechaFinal.Text = DateTime.Today.ToString("yyyy-MM-dd");

            // Cargar los dropdowns
            CargarDropdowns();

            // Cargar la grilla con todos los datos
            CargarGrilla();
        }
    }

    private void CargarDropdowns()
    {
        CargarDropdownsFiltrados("", "", "");
    }

    private void CargarDropdownsFiltrados(string moduloSeleccionado, string eventoSeleccionado, string criticidadSeleccionada)
    {
        var todos = bll.RetornarEventos();

        // Base para cascada: cada dropdown filtra con lo que está seleccionado ANTES que él
        var paraEvento = string.IsNullOrEmpty(moduloSeleccionado)
            ? todos
            : todos.Where(e => e.modulo == moduloSeleccionado).ToList();

        var paraCriticidad = string.IsNullOrEmpty(eventoSeleccionado)
            ? paraEvento
            : paraEvento.Where(e => e.evento == eventoSeleccionado).ToList();

        // Módulo: siempre muestra todos (es el primero)
        var moduloActual = dlModulo.SelectedValue;
        dlModulo.Items.Clear();
        dlModulo.Items.Add(new ListItem("-- Todos --", ""));
        foreach (var item in todos.Select(e => e.modulo).Distinct().OrderBy(x => x))
            dlModulo.Items.Add(new ListItem(item, item));
        if (!string.IsNullOrEmpty(moduloActual))
            dlModulo.SelectedValue = moduloActual;

        // Evento: filtrado por módulo seleccionado
        var eventoActual = dlEvento.SelectedValue;
        dlEvento.Items.Clear();
        dlEvento.Items.Add(new ListItem("-- Todos --", ""));
        foreach (var item in paraEvento.Select(e => e.evento).Distinct().OrderBy(x => x))
            dlEvento.Items.Add(new ListItem(item, item));
        if (dlEvento.Items.FindByValue(eventoActual) != null)
            dlEvento.SelectedValue = eventoActual;

        // Criticidad: filtrado por módulo + evento seleccionados
        var criticidadActual = dlCriticidad.SelectedValue;
        dlCriticidad.Items.Clear();
        dlCriticidad.Items.Add(new ListItem("-- Todos --", ""));
        foreach (var item in paraCriticidad.Select(e => e.criticidad).Distinct().OrderBy(x => x))
            dlCriticidad.Items.Add(new ListItem(item.ToString(), item.ToString()));
        if (dlCriticidad.Items.FindByValue(criticidadActual) != null)
            dlCriticidad.SelectedValue = criticidadActual;
    }

    // Handler compartido para los tres dropdowns
    protected void dlFiltro_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargarDropdownsFiltrados(
            dlModulo.SelectedValue,
            dlEvento.SelectedValue,
            dlCriticidad.SelectedValue
        );
    }

    private void CargarGrilla(List<Evento> lista = null)
    {
        gvBitacora.DataSource = (lista ?? bll.RetornarEventos())
            .OrderByDescending(e => e.fecha)
            .ThenByDescending(e => e.hora.Ticks)
            .ToList();
        gvBitacora.DataBind();
    }

    protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
    {
        txtFechaInicio.Text = DateTime.Today.ToString("yyyy-MM-dd");
        txtFechaFinal.Text = DateTime.Today.ToString("yyyy-MM-dd");
        dlModulo.SelectedIndex = 0;
        dlEvento.SelectedIndex = 0;
        dlCriticidad.SelectedIndex = 0;
        pnlAlerta.Visible = false;
        CargarGrilla();
    }

    protected void btnAplicarFiltros_Click(object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, string> filtros = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(dlModulo.SelectedValue))
                filtros.Add("modulo", dlModulo.SelectedValue);

            if (!string.IsNullOrEmpty(dlEvento.SelectedValue))
                filtros.Add("evento", dlEvento.SelectedValue);

            if (!string.IsNullOrEmpty(dlCriticidad.SelectedValue))
                filtros.Add("criticidad", dlCriticidad.SelectedValue);

            if (!string.IsNullOrEmpty(txtFechaInicio.Text) && !string.IsNullOrEmpty(txtFechaFinal.Text))
            {
                DateTime fechaInicio = DateTime.Parse(txtFechaInicio.Text);
                DateTime fechaFin = DateTime.Parse(txtFechaFinal.Text);

                if (fechaInicio <= fechaFin)
                {
                    filtros.Add("fechaInicio", fechaInicio.ToString());
                    filtros.Add("fechaFin", fechaFin.ToString());
                }
                else
                {
                    txtFechaInicio.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    txtFechaFinal.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    throw new Exception("La fecha de inicio no puede ser mayor a la de fin");
                }
            }

            CargarGrilla(bll.Filtros(filtros));
        }
        catch (Exception ex)
        {
            lblMensajeError.Text = ex.Message;
            pnlAlerta.CssClass = "alert alert-error";
            pnlAlerta.Visible = true;
        }
    }
}