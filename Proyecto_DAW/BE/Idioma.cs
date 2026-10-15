using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Idioma
    {
        public Idioma(int pId, string pCodigo, string pNombre, bool pActivo = true, bool pEsPredeterminado = false)
        {
            ID = pId;
            Codigo = pCodigo;
            Nombre = pNombre;
            Activo = pActivo;
            EsPredeterminado = pEsPredeterminado;
            Traducciones = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public int ID { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public bool EsPredeterminado { get; set; }

        // Clave legible (ej. pageAnimal_btnBajaAnimal) -> texto traducido
        public Dictionary<string, string> Traducciones { get; set; }
    }
}
