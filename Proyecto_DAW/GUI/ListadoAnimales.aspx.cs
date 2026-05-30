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
            CargarAnimales();
        }
    }

    private void CargarRazas()
    {
        var razas = bllAnimal.RetornarAnimales()
            .Where(a => a.vivo &&
                   a.estadoAdopcion.Equals("en adopcion", StringComparison.OrdinalIgnoreCase))
            .Select(a => a.raza)
            .Distinct()
            .OrderBy(r => r)
            .ToList();

        ddlRaza.Items.Clear();
        ddlRaza.Items.Add(new ListItem("Todos", ""));
        foreach (var raza in razas)
            ddlRaza.Items.Add(new ListItem(raza, raza));
    }

    private void CargarAnimales()
    {
        string especie = ddlEspecie.SelectedValue;
        string raza    = ddlRaza.SelectedValue;
        string sexo    = ddlGenero.SelectedValue;

        List<Animal> animales = bllAnimal.RetornarAnimales()
            .Where(a => a.vivo &&
                   a.estadoAdopcion.Equals("en adopcion", StringComparison.OrdinalIgnoreCase))
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

    protected void Filtros_Changed(object sender, EventArgs e)
    {
        CargarAnimales();
    }

    protected string ObtenerFoto(object especie, object codigo)
    {
        int cantFotos = 3;
        int num = (Convert.ToInt32(codigo) % cantFotos) + 1;
        string tipo = especie.ToString().ToLower() == "gato" ? "gato" : "perro";
        return "Images/" + tipo + num + ".jpg";
    }

   
}
