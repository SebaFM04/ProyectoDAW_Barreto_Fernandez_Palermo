using BLL;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adoptantes : PaginaBase
{
    bllAdoptante bllAdoptante;
    bllDigitoVerificador bllDigitoVerificador;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllAdoptante = new bllAdoptante();
        bllDigitoVerificador = new bllDigitoVerificador();

        if (!IsPostBack)
        {
            CargarGrid();
        }
    }

    private void CargarGrid()
    {
        gvAdoptantes.DataSource = bllAdoptante.RetornarAdoptantes();
        gvAdoptantes.DataBind();
    }

    protected void gvAdoptantes_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlAlerta.Visible = false;
        ViewState["dni"] = gvAdoptantes.DataKeys[gvAdoptantes.SelectedIndex].Value.ToString();
        txtDni.Text = TextoCelda(1);
        txtNombre.Text = TextoCelda(2);
        txtApellido.Text = TextoCelda(3);
        txtTelefono.Text = TextoCelda(4);
        txtEdad.Text = TextoCelda(5);
        txtDomicilio.Text = TextoCelda(6);
        chkMascotas.Checked = gvAdoptantes.SelectedRow.Cells[7].Text == "True";
    }

    // La grilla codifica el texto en HTML ("Pérez" -> "P&#233;rez", vacío -> "&nbsp;"): se decodifica
    private string TextoCelda(int indice)
    {
        return HttpUtility.HtmlDecode(gvAdoptantes.SelectedRow.Cells[indice].Text).Replace('\u00A0', ' ').Trim();
    }

    private void MostrarMensaje(string mensaje, bool esError)
    {
        pnlAlerta.Visible = true;
        lbMensaje.Text = mensaje;

        pnlAlerta.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    private void ValidarCampos(string dni, string nombre, string apellido, string telefono, string edad)
    {
        var regexDni = new Regex(@"^\d{8}$");
        var regexTexto = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]+$");
        var regexTelefonoAR = new Regex(@"^(\+54|54)?\s?(9)?\s?(11|[2368]\d{2})\s?\d{3,4}[- ]?\d{4}$");
        var regexEdad = new Regex(@"^(?:[1-9]|[1-9][0-9])$");

        if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(apellido) || string.IsNullOrWhiteSpace(telefono) ||
            string.IsNullOrWhiteSpace(edad))
        {
            throw new Exception(Traducir("pageAdoptantes_msgCompleteCampos", "Debe completar todos los campos obligatorios."));
        }

        if (!regexDni.IsMatch(dni))
            throw new Exception(Traducir("pageAdoptantes_msgDniInvalido", "El DNI ingresado es inválido. Debe contener exactamente 8 dígitos."));

        if (!regexTexto.IsMatch(nombre))
            throw new Exception(Traducir("pageAdoptantes_msgNombreInvalido", "El nombre ingresado es inválido. Solo se permiten letras y espacios."));

        if (!regexTexto.IsMatch(apellido))
            throw new Exception(Traducir("pageAdoptantes_msgApellidoInvalido", "El apellido ingresado es inválido. Solo se permiten letras y espacios."));

        if (!regexTelefonoAR.IsMatch(telefono))
            throw new Exception(Traducir("pageAdoptantes_msgTelefonoInvalido", "El teléfono ingresado es inválido."));

        if (!regexEdad.IsMatch(edad))
            throw new Exception(Traducir("pageAdoptantes_msgEdadInvalida", "La edad ingresada es inválida. Debe ser un número entre 1 y 99."));
    }

    protected void btnAlta_Click(object sender, EventArgs e)
    {
        try
        {
            var dni = txtDni.Text.ToString();
            var nombre = txtNombre.Text.ToString();
            var apellido = txtApellido.Text.ToString();
            var telefono = txtTelefono.Text.ToString();
            var edad = txtEdad.Text.ToString();
            var domicilio = txtDomicilio.Text.ToString();
            var mascotas = chkMascotas.Checked;

            ValidarCampos(dni, nombre, apellido, telefono, edad);

            if (bllAdoptante.ValidarDNI(dni))
                throw new Exception(Traducir("pageAdoptantes_msgDniExistente", "Ya existe un adoptante con ese DNI."));

            bllAdoptante.Alta(dni, nombre, apellido, telefono, int.Parse(edad), domicilio, mascotas);

            // bllDigitoVerificador.CalcularDVAdoptante(); // TODO: descomentar cuando esté terminado

            CargarGrid();
            MostrarMensaje(Traducir("pageAdoptantes_msgAdoptanteAgregado", "Adoptante agregado exitosamente!"), false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(MensajeError(ex), true); }
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["dni"] == null)
            {
                MostrarMensaje(Traducir("pageAdoptantes_msgSeleccioneAdoptante", "Por favor, seleccione un adoptante de la lista."), true);
                return;
            }

            var dni = TextoCelda(1);
            var nombre = txtNombre.Text.ToString();
            var apellido = txtApellido.Text.ToString();
            var telefono = txtTelefono.Text.ToString();
            var edad = txtEdad.Text.ToString();
            var domicilio = txtDomicilio.Text.ToString();
            var mascotas = chkMascotas.Checked;

            ValidarCampos(dni, nombre, apellido, telefono, edad);

            bllAdoptante.Modificar(dni, nombre, apellido, telefono, int.Parse(edad), domicilio, mascotas);

            // bllDigitoVerificador.CalcularDVAdoptante(); // TODO: descomentar cuando esté terminado

            CargarGrid();
            MostrarMensaje(Traducir("pageAdoptantes_msgAdoptanteModificado", "Adoptante modificado exitosamente!"), false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(MensajeError(ex), true); }
    }

    protected void btnActivarDesactivar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["dni"] == null)
            {
                MostrarMensaje(Traducir("pageAdoptantes_msgSeleccioneAdoptante", "Por favor, seleccione un adoptante de la lista."), true);
                return;
            }

            var dni = TextoCelda(1);
            bllAdoptante.ActivarDesactivar(dni);

            // bllDigitoVerificador.CalcularDVAdoptante(); // TODO: descomentar cuando esté terminado

            CargarGrid();
            MostrarMensaje(Traducir("pageAdoptantes_msgEstadoActualizado", "Estado del adoptante actualizado exitosamente!"), false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(MensajeError(ex), true); }
    }
}