using BE;
using SERVICIOS;
using System;
using System.Web.UI;

// Patrón Observer: Observador concreto.
// Todas las páginas heredan de acá: se suscriben al sujeto (GestorIdioma) al iniciar,
// se traducen cuando éste notifica y se desuscriben al terminar el request.
public class PaginaBase : Page, IObservadorIdioma
{
    protected override void OnInit(EventArgs e)
    {
        GestorIdioma.Actual.Suscribir(this);
        base.OnInit(e);
        IdiomaHelper.AsegurarIdioma();
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        // Cada request se renderiza traducido (incluye controles que el code-behind haya hecho visibles)
        GestorIdioma.Actual.Notificar();
    }

    protected override void OnUnload(EventArgs e)
    {
        GestorIdioma.Actual.Desuscribir(this);
        base.OnUnload(e);
    }

    // La página traduce solo su contenido; la navbar y el footer los traduce la MasterPage (otro observador)
    public virtual void ActualizarIdioma(Idioma idioma)
    {
        Control contenido = Master == null ? null : Master.FindControl("ContentPlaceHolder1");
        TraductorHelper.Traducir(contenido ?? this, idioma.Traducciones);
    }

    // Para mensajes armados desde el code-behind
    protected string Traducir(string clave, string porDefecto)
    {
        return GestorIdioma.Actual.Traducir(clave, porDefecto);
    }

    // Para mostrar el mensaje de una excepción (las de la BLL/DAL vienen con clave de traducción)
    protected string MensajeError(Exception ex)
    {
        return GestorIdioma.Actual.TraducirExcepcion(ex);
    }
}
