using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;

public partial class ListadoAnimales : System.Web.UI.Page
{
    bllAnimal bllAnimal;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllAnimal = new bllAnimal();

        if (!IsPostBack)
        {
            CargarRazas();
            CargarEspecies();
            CargarAnimales();

        }
    }

    private void CargarRazas()
    {
        string especieSeleccionada = ddlEspecie.SelectedValue;

        var animales = bllAnimal.RetornarAnimales()
            .Where(a => a.vivo &&
                   a.estadoAdopcion.Equals("en adopcion", StringComparison.OrdinalIgnoreCase));

        // Si hay una especie seleccionada, filtrar las razas por esa especie
        if (!string.IsNullOrEmpty(especieSeleccionada))
            animales = animales.Where(a => a.especie.Equals(especieSeleccionada, StringComparison.OrdinalIgnoreCase));

        var razas = animales
            .Select(a => a.raza)
            .Distinct()
            .OrderBy(r => r)
            .ToList();

        ddlRaza.Items.Clear();
        ddlRaza.Items.Add(new ListItem("Todos", ""));
        foreach (var raza in razas)
            ddlRaza.Items.Add(new ListItem(raza, raza));
    }
    private void CargarEspecies()
    {
        string especieActual = ddlEspecie.SelectedValue;

        var especies = bllAnimal.RetornarAnimales()
            .Where(a => a.vivo &&
                   a.estadoAdopcion.Equals("en adopcion", StringComparison.OrdinalIgnoreCase))
            .Select(a => a.especie)
            .Distinct()
            .OrderBy(e => e)
            .ToList();

        ddlEspecie.Items.Clear();
        ddlEspecie.Items.Add(new ListItem("Todos", ""));
        foreach (var especie in especies)
            ddlEspecie.Items.Add(new ListItem(especie, especie));

        // Restaurar selección si todavía existe
        if (ddlEspecie.Items.FindByValue(especieActual) != null)
            ddlEspecie.SelectedValue = especieActual;
    }

    private void CargarAnimales()
    {
        string especie = ddlEspecie.SelectedValue;
        string raza = ddlRaza.SelectedValue;
        string sexo = ddlGenero.SelectedValue;

        List<Animal> animales = bllAnimal.RetornarAnimales()
            .Where(a => a.vivo && a.estadoAdopcion.Equals("en adopcion", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!string.IsNullOrEmpty(especie))
            animales = animales.Where(a => a.especie.Equals(especie, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrEmpty(raza))
            animales = animales.Where(a => a.raza.Equals(raza, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!string.IsNullOrEmpty(sexo))
            animales = animales.Where(a => a.sexo.Equals(sexo, StringComparison.OrdinalIgnoreCase)).ToList();

        pnlSinAnimales.Visible = (animales.Count == 0);
        rptAnimales.DataSource = animales;
        rptAnimales.DataBind();
    }

    protected void ddlEspecie_Changed(object sender, EventArgs e)
    {
        CargarRazas();
        ddlRaza.SelectedIndex = 0;
        CargarAnimales();
    }
    protected void ddlRaza_Changed(object sender, EventArgs e)
    {
        string razaSeleccionada = ddlRaza.SelectedValue;

        if (!string.IsNullOrEmpty(razaSeleccionada))
        {
            var animal = bllAnimal.RetornarAnimales()
                .FirstOrDefault(a => a.raza.Equals(razaSeleccionada, StringComparison.OrdinalIgnoreCase));

            if (animal != null)
            {
                CargarEspecies(); // ← recargar antes de setear
                ddlEspecie.SelectedValue = animal.especie;
                CargarRazas();
                ddlRaza.SelectedValue = razaSeleccionada;
            }
        }

        CargarAnimales();
    }


    protected void Filtros_Changed(object sender, EventArgs e)
    {
        CargarAnimales();
    }


}
