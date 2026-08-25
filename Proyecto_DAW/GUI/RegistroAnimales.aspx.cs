using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegistroAnimales : System.Web.UI.Page
{
    bllAnimal bllanimal;
    bllDigitoVerificador bllDigitoVerificador;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllanimal = new bllAnimal();
        bllDigitoVerificador = new bllDigitoVerificador();

        if (!IsPostBack)
        {
            CargarGrid();
        }
    }

    private void CargarGrid()
    {
        gvAnimales.DataSource = bllanimal.RetornarAnimales();
        gvAnimales.DataBind();
    }

    protected void gvAnimales_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        ViewState["codigoAnimal"] = gvAnimales.DataKeys[gvAnimales.SelectedIndex].Value.ToString();
        txtEspecie.Text = gvAnimales.SelectedRow.Cells[2].Text;
        txtRaza.Text = gvAnimales.SelectedRow.Cells[3].Text;
        txtNombre.Text = gvAnimales.SelectedRow.Cells[4].Text;
        ddlTamano.Text = gvAnimales.SelectedRow.Cells[5].Text;
        ddlSexo.Text = gvAnimales.SelectedRow.Cells[6].Text;
        ddlEstado.Text = gvAnimales.SelectedRow.Cells[7].Text;
    }

    private void MostrarMensaje(string mensaje, bool esError)
    {
        pnlAlerta.Visible = true;
        lbMensaje.Text = mensaje;
        
        pnlAlerta.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    protected void btnAlta_Click(object sender, EventArgs e)
    {
        try
        {
            var especie = txtEspecie.Text.ToString();
            var raza = txtRaza.Text.ToString();
            var nombre = txtNombre.Text.ToString();
            var tamano = ddlTamano.SelectedValue.ToString();
            var sexo = ddlSexo.SelectedValue.ToString();
            var estadoAdopcion = ddlEstado.SelectedValue.ToString();

            bllanimal.AltaAnimal(especie, raza, nombre, tamano, sexo, estadoAdopcion, true);

            CargarGrid();
            MostrarMensaje("Animal agregado exitosamente!", false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(ex.Message, true); }
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["codigoAnimal"] == null)
            {
                MostrarMensaje("Por favor, seleccione un animal de la lista.", true);
                return;
            }

            var codigo = gvAnimales.SelectedRow.Cells[1].Text.ToString();
            var especie = txtEspecie.Text.ToString();
            var raza = txtRaza.Text.ToString();
            var nombre = txtNombre.Text.ToString();
            var tamano = ddlTamano.SelectedValue.ToString();
            var sexo = ddlSexo.SelectedValue.ToString();
            var estadoAdopcion = ddlEstado.SelectedValue.ToString();

            bllanimal.Modificar(codigo, especie, raza, nombre, tamano, sexo, estadoAdopcion, true);

            CargarGrid();
            MostrarMensaje("Animal modificado exitosamente!", false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(ex.Message, true); }
    }

    protected void btnBaja_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["codigoAnimal"] == null)
            {
                MostrarMensaje("Por favor, seleccione un animal de la lista.", true);
                return;
            }

            var codigo = gvAnimales.SelectedRow.Cells[1].Text.ToString();
            bllanimal.Baja(codigo);
            CargarGrid();
            MostrarMensaje("Animal borrado exitosamente!", false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(ex.Message, true); }
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        string xml = bllanimal.ExportarAnimalesXML(); // adentro arma el wrapper, vos no lo tocás

        Response.Clear();
        Response.ContentType = "text/xml";
        Response.AddHeader("Content-Disposition", "attachment; filename=Animales.xml");
        Response.Write(xml);
        Response.End();
    }

    protected void btnImportar_Click(object sender, EventArgs e)
    {
        if (!fuImportar.HasFile)
        {
            lbMensaje.Text = "Seleccioná un archivo XML primero.";
            return;
        }

        string extension = Path.GetExtension(fuImportar.FileName).ToLower();
        if (extension != ".xml")
        {
            lbMensajeXML.Text = "El archivo debe tener formato .xml";
            return;
        }

        string resultado = bllanimal.ImportarAnimalesXML(fuImportar.FileContent); // ídem
        lbMensaje.Text = resultado;

        CargarGrid();
    }

}