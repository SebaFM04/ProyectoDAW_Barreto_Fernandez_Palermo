using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    bllBitacora bll;
    protected void Page_Load(object sender, EventArgs e)
    {
        bll = new bllBitacora();
        if (!IsPostBack)
        {
            CargarGrilla();
        }
    }

    private void CargarGrilla()
    {
        List<Evento> listaEventos = bll.RetornarEventos();

        gvProductos.DataSource = listaEventos;
        gvProductos.DataBind();
    }
}