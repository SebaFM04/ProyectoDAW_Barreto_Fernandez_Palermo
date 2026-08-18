using BLL;
using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Adoptantes : System.Web.UI.Page
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
        txtDni.Text = gvAdoptantes.SelectedRow.Cells[1].Text;
        txtNombre.Text = gvAdoptantes.SelectedRow.Cells[2].Text;
        txtApellido.Text = gvAdoptantes.SelectedRow.Cells[3].Text;
        txtTelefono.Text = gvAdoptantes.SelectedRow.Cells[4].Text;
        txtEdad.Text = gvAdoptantes.SelectedRow.Cells[5].Text;
        txtDomicilio.Text = gvAdoptantes.SelectedRow.Cells[6].Text;
        chkMascotas.Checked = gvAdoptantes.SelectedRow.Cells[7].Text == "True";
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
            throw new Exception("Debe completar todos los campos obligatorios.");
        }

        if (!regexDni.IsMatch(dni))
            throw new Exception("El DNI ingresado es inválido. Debe contener exactamente 8 dígitos.");

        if (!regexTexto.IsMatch(nombre))
            throw new Exception("El nombre ingresado es inválido. Solo se permiten letras y espacios.");

        if (!regexTexto.IsMatch(apellido))
            throw new Exception("El apellido ingresado es inválido. Solo se permiten letras y espacios.");

        if (!regexTelefonoAR.IsMatch(telefono))
            throw new Exception("El teléfono ingresado es inválido.");

        if (!regexEdad.IsMatch(edad))
            throw new Exception("La edad ingresada es inválida. Debe ser un número entre 1 y 99.");
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
                throw new Exception("Ya existe un adoptante con ese DNI.");

            bllAdoptante.Alta(dni, nombre, apellido, telefono, int.Parse(edad), domicilio, mascotas);

            // bllDigitoVerificador.CalcularDVAdoptante(); // TODO: descomentar cuando esté terminado

            CargarGrid();
            MostrarMensaje("Adoptante agregado exitosamente!", false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(ex.Message, true); }
    }

    protected void btnModificar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["dni"] == null)
            {
                MostrarMensaje("Por favor, seleccione un adoptante de la lista.", true);
                return;
            }

            var dni = gvAdoptantes.SelectedRow.Cells[1].Text.ToString();
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
            MostrarMensaje("Adoptante modificado exitosamente!", false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(ex.Message, true); }
    }

    protected void btnActivarDesactivar_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["dni"] == null)
            {
                MostrarMensaje("Por favor, seleccione un adoptante de la lista.", true);
                return;
            }

            var dni = gvAdoptantes.SelectedRow.Cells[1].Text.ToString();
            bllAdoptante.ActivarDesactivar(dni);

            // bllDigitoVerificador.CalcularDVAdoptante(); // TODO: descomentar cuando esté terminado

            CargarGrid();
            MostrarMensaje("Estado del adoptante actualizado exitosamente!", false);
            ScriptManager.RegisterStartupScript(this, GetType(), "acciones", "limpiarFormulario(); ocultarAlerta();", true);
        }
        catch (Exception ex) { MostrarMensaje(ex.Message, true); }
    }
}