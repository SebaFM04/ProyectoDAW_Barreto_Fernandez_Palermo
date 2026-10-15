using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegistroAnimales : PaginaBase
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
        txtEspecie.Text = TextoCelda(2);
        txtRaza.Text = TextoCelda(3);
        txtNombre.Text = TextoCelda(4);
        Seleccionar(ddlTamano, TextoCelda(5));
        Seleccionar(ddlSexo, TextoCelda(6));
        Seleccionar(ddlEstado, TextoCelda(7));
    }

    // La grilla codifica el texto en HTML ("Pequeño" -> "Peque&#241;o", vacío -> "&nbsp;"): se decodifica
    private string TextoCelda(int indice)
    {
        return HttpUtility.HtmlDecode(gvAnimales.SelectedRow.Cells[indice].Text).Replace('\u00A0', ' ').Trim();
    }

    // Si el valor no está en la lista no se rompe la página: queda la primera opción
    private void Seleccionar(DropDownList ddl, string valor)
    {
        if (ddl.Items.FindByValue(valor) != null)
            ddl.SelectedValue = valor;
        else
            ddl.SelectedIndex = 0;
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

            int codigoAnimalNuevo = bllanimal.AltaAnimal(especie, raza, nombre, tamano, sexo, estadoAdopcion, true);
            Response.Redirect("FichaIngreso.aspx?codigoAnimal=" + codigoAnimalNuevo, false);
            Context.ApplicationInstance.CompleteRequest();

            //CargarGrid();
            //MostrarMensaje("Animal agregado exitosamente!", false);
            //ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(MensajeError(ex), true); }
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["codigoAnimal"] == null)
            {
                MostrarMensaje(Traducir("pageAnimal_msgSeleccioneAnimal", "Por favor, seleccione un animal de la lista."), true);
                return;
            }

            var codigo = TextoCelda(1);
            var especie = txtEspecie.Text.ToString();
            var raza = txtRaza.Text.ToString();
            var nombre = txtNombre.Text.ToString();
            var tamano = ddlTamano.SelectedValue.ToString();
            var sexo = ddlSexo.SelectedValue.ToString();
            var estadoAdopcion = ddlEstado.SelectedValue.ToString();

            bllanimal.Modificar(codigo, especie, raza, nombre, tamano, sexo, estadoAdopcion, true);

            CargarGrid();
            MostrarMensaje(Traducir("pageAnimal_msgAnimalModificado", "Animal modificado exitosamente!"), false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(MensajeError(ex), true); }
    }

    protected void btnBaja_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["codigoAnimal"] == null)
            {
                MostrarMensaje(Traducir("pageAnimal_msgSeleccioneAnimal", "Por favor, seleccione un animal de la lista."), true);
                return;
            }

            var codigo = TextoCelda(1);
            bllanimal.Baja(codigo);
            CargarGrid();
            MostrarMensaje(Traducir("pageAnimal_msgAnimalBorrado", "Animal borrado exitosamente!"), false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(MensajeError(ex), true); }
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
            lbMensaje.Text = Traducir("pageAnimal_msgSeleccioneXML", "Seleccioná un archivo XML primero.");
            return;
        }

        string extension = Path.GetExtension(fuImportar.FileName).ToLower();
        if (extension != ".xml")
        {
            lbMensajeXML.Text = Traducir("pageAnimal_msgFormatoXML", "El archivo debe tener formato .xml");
            return;
        }

        string resultado = bllanimal.ImportarAnimalesXML(fuImportar.FileContent); // ídem
        lbMensaje.Text = resultado;

        CargarGrid();
    }

}