using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegistroAnimales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarGrid();
        }
    }

    private void CargarGrid()
    {
        var lista = new List<Animal>()
        {
            new Animal { Codigo = 1, Especie = "Perro", Raza = "Labrador", Nombre = "Max" },
            new Animal { Codigo = 2, Especie = "Gato", Raza = "Siames", Nombre = "Luna" },
            new Animal { Codigo = 3, Especie = "Perro", Raza = "Caniche", Nombre = "Rocky" }
        };

        gvAnimales.DataSource = lista;
        gvAnimales.DataBind();
    }
}

public class Animal
{
    public int Codigo { get; set; }
    public string Especie { get; set; }
    public string Raza { get; set; }
    public string Nombre { get; set; }
}