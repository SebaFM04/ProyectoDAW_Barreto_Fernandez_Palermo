using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GestionIntermediaVacunaAnimal : System.Web.UI.Page
{
    bllIntermediaVacunaAnimal bllIntermedia;
    bllVacuna bllvacuna;
    bllAnimal bllanimal;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllIntermedia = new bllIntermediaVacunaAnimal();
        bllvacuna = new bllVacuna();
        bllanimal = new bllAnimal();

        if (!IsPostBack)
        {
            CargarGrillaVacunas();
            CargarGrillaAnimales();
            CargarGrillaIntermedia();
        }
    }

    protected void gvVacunas_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CodigoVacuna"] = gvVacunas.DataKeys[gvVacunas.SelectedIndex].Value.ToString();
        ViewState["NombreVacuna"] = gvVacunas.SelectedRow.Cells[2].Text;
    }

    protected void gvAnimales_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CodigoAnimal"] = gvAnimales.DataKeys[gvAnimales.SelectedIndex].Value.ToString();
    }

    protected void btnAlta_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        lbMensaje.Text = "";

        // Validar selección de vacuna y animal
        if (ViewState["CodigoVacuna"] == null)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "Por favor, seleccione una vacuna de la lista.";
            return;
        }
        if (ViewState["CodigoAnimal"] == null)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "Por favor, seleccione un animal de la lista.";
            return;
        }

        if (!ValidarCampos()) return;
        if (!ConvertirCampos(out DateTime fechaAplicacion, out DateTime fechaProxima)) return;

        try
        {
            string codigoVacuna = ViewState["CodigoVacuna"].ToString();
            string nombreVacuna = ViewState["NombreVacuna"].ToString();
            int codigoAnimal = int.Parse(ViewState["CodigoAnimal"].ToString());

            bllIntermedia.Alta(codigoVacuna, codigoAnimal,nombreVacuna, fechaAplicacion, fechaProxima);

            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-exito";
            lbMensaje.Text = "Vacuna asignada exitosamente.";
            CargarGrillaIntermedia();
        }
        catch (Exception ex)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "Error: " + ex.Message;
        }
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        lbMensaje.Text = "";

        if (ViewState["CodigoIntermedia"] == null)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "Por favor, seleccione un registro de la lista para modificar.";
            return;
        }

        if (!ValidarCampos()) return;
        if (!ConvertirCampos(out DateTime fechaAplicacion, out DateTime fechaProxima)) return;

        try
        {
            int codigoIntermedia = int.Parse(ViewState["CodigoIntermedia"].ToString());
            bllIntermedia.Modificar(codigoIntermedia, fechaAplicacion, fechaProxima);

            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-exito";
            lbMensaje.Text = "Fechas modificadas exitosamente.";

            ViewState["CodigoIntermedia"] = null; 
            CargarGrillaIntermedia();
        }
        catch (Exception ex)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "Error: " + ex.Message;
        }
    }

    protected void gvIntermedia_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Guardar código de la intermedia seleccionada
        ViewState["CodigoIntermedia"] = gvIntermedia.DataKeys[gvIntermedia.SelectedIndex].Value.ToString();

        // Cargar las fechas actuales en los textbox
        // Cells[0]=botón, [1]=codigo, [2]=codigoVacuna, [3]=codigoAnimal, [4]=nombreVacuna, [5]=fechaAplicacion, [6]=fechaProximaAplicacion
        GridViewRow fila = gvIntermedia.SelectedRow;

        // Convertir el formato dd/MM/yyyy a yyyy-MM-dd para el TextMode="Date"
        DateTime fechaAplicacion = DateTime.ParseExact(fila.Cells[5].Text, "dd/MM/yyyy", null);
        DateTime fechaProxima = DateTime.ParseExact(fila.Cells[6].Text, "dd/MM/yyyy", null);

        txtFechaAplicacion.Text = fechaAplicacion.ToString("yyyy-MM-dd");
        txtFechaAplicacionProxima.Text = fechaProxima.ToString("yyyy-MM-dd");
    }

    private bool ValidarCampos()
    {
        if (string.IsNullOrWhiteSpace(txtFechaAplicacion.Text) ||
            string.IsNullOrWhiteSpace(txtFechaAplicacionProxima.Text))
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "Por favor, complete todos los campos.";
            return false;
        }

        DateTime fechaAplicacion = DateTime.Parse(txtFechaAplicacion.Text);
        DateTime fechaProxima = DateTime.Parse(txtFechaAplicacionProxima.Text);

        if (fechaAplicacion >= fechaProxima)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "La fecha de aplicación debe ser menor a la próxima aplicación.";
            return false;
        }
        return true;
    }

    private bool ConvertirCampos(out DateTime fechaAplicacion, out DateTime fechaProxima)
    {
        fechaAplicacion = DateTime.MinValue;
        fechaProxima = DateTime.MinValue;

        if (!DateTime.TryParse(txtFechaAplicacion.Text, out fechaAplicacion))
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "La fecha de aplicación no es válida.";
            return false;
        }
        if (!DateTime.TryParse(txtFechaAplicacionProxima.Text, out fechaProxima))
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = "La fecha próxima de aplicación no es válida.";
            return false;
        }
        return true;
    }

    private void CargarGrillaVacunas()
    {
        List<Vacuna> todasLasVacunas = bllvacuna.RetornarVacunas();
        List<Vacuna> soloActivas = todasLasVacunas.Where(v => v.activo).ToList();

        gvVacunas.DataSource = soloActivas;
        gvVacunas.DataBind();

        if (ViewState["CodigoVacuna"] != null)
        {
            foreach (GridViewRow fila in gvVacunas.Rows)
            {
                if (gvVacunas.DataKeys[fila.RowIndex].Value.ToString() == ViewState["CodigoVacuna"].ToString())
                {
                    gvVacunas.SelectedIndex = fila.RowIndex;
                    break;
                }
            }
        }
    }

    private void CargarGrillaAnimales()
    {
        gvAnimales.DataSource = bllanimal.RetornarAnimales();
        gvAnimales.DataBind();

        if (ViewState["CodigoAnimal"] != null)
        {
            foreach (GridViewRow fila in gvAnimales.Rows)
            {
                if (gvAnimales.DataKeys[fila.RowIndex].Value.ToString() == ViewState["CodigoAnimal"].ToString())
                {
                    gvAnimales.SelectedIndex = fila.RowIndex;
                    break;
                }
            }
        }
    }

    private void CargarGrillaIntermedia()
    {
        gvIntermedia.DataSource = bllIntermedia.RetornarVacunas();
        gvIntermedia.DataBind();

        if (ViewState["CodigoIntermedia"] != null)
        {
            foreach (GridViewRow fila in gvIntermedia.Rows)
            {
                if (gvIntermedia.DataKeys[fila.RowIndex].Value.ToString() == ViewState["CodigoIntermedia"].ToString())
                {
                    gvIntermedia.SelectedIndex = fila.RowIndex;
                    break;
                }
            }
        }
    }

    protected void btnGestionarVacunas_Click(object sender, EventArgs e)
    {
        Response.Redirect("GestionVacuna.aspx");
    }

    
}