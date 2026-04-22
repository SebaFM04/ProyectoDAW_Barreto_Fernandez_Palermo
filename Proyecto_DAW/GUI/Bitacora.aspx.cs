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
        var eventos = bll.RetornarEventos();

        // Modulo
        dlModulo.Items.Clear();
        dlModulo.Items.Add(new ListItem("-- Todos --", ""));
        foreach (var item in eventos.Select(e => e.modulo).Distinct())
            dlModulo.Items.Add(new ListItem(item, item));

        // Evento
        dlEvento.Items.Clear();
        dlEvento.Items.Add(new ListItem("-- Todos --", ""));
        foreach (var item in eventos.Select(e => e.evento).Distinct())
            dlEvento.Items.Add(new ListItem(item, item));

        // Criticidad
        dlCriticidad.Items.Clear();
        dlCriticidad.Items.Add(new ListItem("-- Todos --", ""));
        foreach (var item in eventos.Select(e => e.criticidad).Distinct())
            dlCriticidad.Items.Add(new ListItem(item.ToString(), item.ToString()));
    }

    private void CargarGrilla()
    {
        List<Evento> listaEventos = bll.RetornarEventos();

        gvProductos.DataSource = listaEventos;
        gvProductos.DataBind();
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

            // Llamás a tu BLL igual que antes
            gvProductos.DataSource = bll.Filtros(filtros);
            gvProductos.DataBind();
        }
        catch (Exception ex)
        {
            lblMensajeError.Text = ex.Message;
            pnlAlerta.CssClass = "alert alert-error";
            pnlAlerta.Visible = true;
        }
    }
}