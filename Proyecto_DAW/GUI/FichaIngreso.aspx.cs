using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FichaIngreso : PaginaBase
{
    bllFichaDeIngreso bllFicha;
    bllAnimal bllAnimal;
    int codigoAnimal;

    protected void Page_Load(object sender, EventArgs e)
    {
        bllFicha = new bllFichaDeIngreso();
        bllAnimal = new bllAnimal();

        if (!int.TryParse(Request.QueryString["codigoAnimal"], out codigoAnimal))
        {
            Response.Redirect("RegistroAnimales.aspx");
            return;
        }

        if (!IsPostBack)
        {
            CargarInfoAnimal();
            CargarHistorial();
        }
    }

    private void CargarInfoAnimal()
    {
        Animal animal = bllAnimal.BuscarAnimalPorCodigo(codigoAnimal.ToString());

        if (animal != null)
        {
            lbInfoAnimal.Text = $"Animal: {animal.nombre} ({animal.especie} - {animal.raza})";
        }

        bool esReingreso = bllFicha.TieneFicha(codigoAnimal);
        btnRegistrar.Text = esReingreso ? Traducir("pageFichaIngreso_btnRegistrarReingreso", "Registrar reingreso") : Traducir("pageFichaIngreso_btnRegistrarIngreso", "Registrar ingreso");
    }

    private void CargarHistorial()
    {
        FichaDeIngreso ficha = bllFicha.ObtenerFichaPorAnimal(codigoAnimal);

        if (ficha != null)
        {
            gvHistorial.DataSource = bllFicha.ObtenerHistorial(ficha.codigoFicha);
            gvHistorial.DataBind();
        }
    }

    private void MostrarMensaje(string mensaje, bool esError)
    {
        pnlAlerta.Visible = true;
        lbMensaje.Text = mensaje;
        pnlAlerta.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        try
        {
            if (bllFicha.TieneFicha(codigoAnimal))
            {
                bllFicha.RegistrarReingreso(codigoAnimal, txtMotivo.Text);
                MostrarMensaje(Traducir("pageFichaIngreso_msgReingresoRegistrado", "Reingreso registrado exitosamente."), false);
            }
            else
            {
                bllFicha.CrearFichaConPrimerIngreso(codigoAnimal, txtMotivo.Text);
                MostrarMensaje(Traducir("pageFichaIngreso_msgFichaCreada", "Ficha de ingreso creada exitosamente."), false);
            }

            txtMotivo.Text = "";
            CargarHistorial();
        }
        catch (Exception ex)
        {
            MostrarMensaje(MensajeError(ex), true);
        }
    }

    protected void btnVolver_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegistroAnimales.aspx");
    }
}