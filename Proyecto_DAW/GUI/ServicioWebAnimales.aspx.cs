using System;

// Página pública que consume el servicio web propio (AnimalesApiController) desde el navegador.
// No tiene lógica de servidor: los datos los trae ScriptServicioWeb.js con fetch.
public partial class ServicioWebAnimales : PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
}
