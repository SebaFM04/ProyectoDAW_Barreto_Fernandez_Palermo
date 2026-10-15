using BE;

namespace SERVICIOS
{
    // Patrón Observer: Observador.
    // Lo implementan las páginas (PaginaBase) y las MasterPages; cuando el sujeto
    // notifica un cambio de idioma, cada observador se re-traduce a sí mismo.
    public interface IObservadorIdioma
    {
        void ActualizarIdioma(Idioma idioma);
    }
}
