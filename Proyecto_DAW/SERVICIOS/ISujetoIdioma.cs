namespace SERVICIOS
{
    // Patrón Observer: Sujeto.
    public interface ISujetoIdioma
    {
        void Suscribir(IObservadorIdioma observador);
        void Desuscribir(IObservadorIdioma observador);
        void Notificar();
    }
}
