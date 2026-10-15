using BE;
using BLL;
using SERVICIOS;
using System;
using System.Data.SqlClient;

// Carga idiomas desde la BLL y se los entrega al sujeto (GestorIdioma).
// Está en GUI porque SERVICIOS no puede referenciar a la BLL.
public static class IdiomaHelper
{
    // Si la sesión todavía no tiene idioma, carga el del usuario logueado o el predeterminado.
    // Devuelve true si cargó uno (y por lo tanto ya se notificó a los observadores).
    public static bool AsegurarIdioma()
    {
        if (GestorIdioma.Actual.TieneIdioma)
            return false;

        Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();
        return Cargar(usuario == null ? null : usuario.lenguaje);
    }

    // Al iniciar sesión: aplica el idioma guardado en el usuario
    public static void CargarIdiomaUsuario(Usuario usuario)
    {
        if (usuario != null)
            Cargar(usuario.lenguaje);
    }

    // El usuario eligió un idioma en la navbar: se guarda en Session y, si está logueado, en la BD.
    // persistir = false en las pantallas del Dígito Verificador: guardar en Usuario recalcularía el DV
    // y taparía la inconsistencia detectada.
    public static void CambiarIdioma(string codigo, bool persistir = true)
    {
        Idioma idioma = new bllIdioma().ObtenerIdiomaConTraducciones(codigo);
        if (idioma == null) return;

        Usuario usuario = claseSession.Gestor.RetornarUsuarioSession();
        if (persistir && usuario != null)
            new bllUsuario().CambiarIdioma(usuario, idioma.Codigo);

        GestorIdioma.Actual.CambiarIdioma(idioma);
    }

    // Vuelve a leer de la BD el idioma actual (por ejemplo después de editar sus traducciones)
    public static void RecargarIdiomaActual()
    {
        Idioma actual = GestorIdioma.Actual.IdiomaActual;
        Cargar(actual == null ? null : actual.Codigo);
    }

    public static string CodigoActual()
    {
        Idioma actual = GestorIdioma.Actual.IdiomaActual;
        return actual == null ? null : actual.Codigo;
    }

    private static bool Cargar(string codigo)
    {
        try
        {
            Idioma idioma = new bllIdioma().ObtenerIdiomaConTraducciones(codigo);
            if (idioma == null) return false;

            GestorIdioma.Actual.CambiarIdioma(idioma);
            return true;
        }
        catch (SqlException)
        {
            // Si todavía no se ejecutó script_idiomas.sql, el sitio sigue funcionando en español
            return false;
        }
    }
}
