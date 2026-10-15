using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GestionVacuna : PaginaBase
{
    bllVacuna bllvacuna;
    bllDigitoVerificador bllDigitoVerificador;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllvacuna = new bllVacuna();
        bllDigitoVerificador = new bllDigitoVerificador();

        if (!IsPostBack)
        {
            CargarGrillaVacunas();
            DeshabilitarFormulario();
        }
    }

    private void DeshabilitarFormulario()
    {
        txtCodigo.Enabled = false;
        txtNombre.Enabled = false;
        rbActivo.Enabled = false;
        rbInactivo.Enabled = false;
        btnAplicar.Visible = false;
    }

    private void HabilitarFormulario(bool esAlta)
    {
        txtCodigo.Enabled = esAlta;  // solo editable en alta
        txtNombre.Enabled = true;
        rbActivo.Enabled = !esAlta;  // solo editable en modificar
        rbInactivo.Enabled = !esAlta;
        btnAplicar.Visible = true;
    }

    protected void gvVacunas_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CodigoVacuna"] = gvVacunas.DataKeys[gvVacunas.SelectedIndex].Value.ToString();
        txtCodigo.Text = TextoCelda(1);
        txtNombre.Text = TextoCelda(2);
        string activo = gvVacunas.SelectedRow.Cells[3].Text;
        rbActivo.Checked = activo == "True" || activo == "1";
        rbInactivo.Checked = !rbActivo.Checked;
        DeshabilitarFormulario();
    }

    // La grilla codifica el texto en HTML ("Antirrábica" -> "Antirr&#225;bica", vacío -> "&nbsp;"): se decodifica
    private string TextoCelda(int indice)
    {
        return HttpUtility.HtmlDecode(gvVacunas.SelectedRow.Cells[indice].Text).Replace('\u00A0', ' ').Trim();
    }

    protected void btnAlta_Click(object sender, EventArgs e)
    {
        ViewState["Modo"] = "Alta";
        ViewState["CodigoVacuna"] = null;
        txtCodigo.Text = "";
        txtNombre.Text = "";
        rbActivo.Checked = true;
        HabilitarFormulario(esAlta: true);
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        if (ViewState["CodigoVacuna"] == null)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = Traducir("pageVacuna_msgSeleccioneVacuna", "Por favor, seleccione una vacuna de la lista.");
            return;
        }
        ViewState["Modo"] = "Modificar";
        HabilitarFormulario(esAlta: false);
    }

    protected void btnAplicar_Click(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        lbMensaje.Text = "";

        string modo = ViewState["Modo"].ToString();

        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            (modo == "Alta" && string.IsNullOrWhiteSpace(txtCodigo.Text)))
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = Traducir("pageVacuna_msgCompleteCampos", "Por favor, complete todos los campos.");
            return;
        }

        try
        {
            if (modo == "Alta")
            {
                bllvacuna.AltaVacuna(txtCodigo.Text.Trim(), txtNombre.Text.Trim());
                lbMensaje.Text = Traducir("pageVacuna_msgVacunaRegistrada", "Vacuna registrada exitosamente.");
            }
            else
            {
                string codigoVacuna = ViewState["CodigoVacuna"].ToString();
                bllvacuna.Modificar(codigoVacuna, txtNombre.Text.Trim(), rbActivo.Checked);
                lbMensaje.Text = Traducir("pageVacuna_msgVacunaModificada", "Vacuna modificada exitosamente.");
            }

            bllDigitoVerificador.CalcularDVVacuna();

            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-exito";
            DeshabilitarFormulario();
            CargarGrillaVacunas();
        }
        catch (Exception ex)
        {
            pnlAlerta.Visible = true;
            pnlAlerta.CssClass = "alert alert-error";
            lbMensaje.Text = Traducir("pageVacuna_msgError", "Error: ") + MensajeError(ex);
        }
    }


    private void CargarGrillaVacunas()
    {
        gvVacunas.DataSource = bllvacuna.RetornarVacunas();
        gvVacunas.DataBind();
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("GestionIntermediaVacunaAnimal.aspx");
    }
}