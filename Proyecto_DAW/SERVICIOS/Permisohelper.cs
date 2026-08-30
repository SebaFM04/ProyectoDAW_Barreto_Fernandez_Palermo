using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SERVICIOS
{
    public static class PermisoHelper
    {
        public static HashSet<string> ObtenerClaves(List<AccesoPermiso> permisos)
        {
            HashSet<string> claves = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (permisos == null) return claves;

            foreach (AccesoPermiso acceso in permisos)
            {
                foreach (Permiso simple in acceso.ObtenerPermisosSimples())
                {
                    if (!string.IsNullOrEmpty(simple.Clave))
                        claves.Add(simple.Clave);
                }
            }
            return claves;
        }

        //  Controles con data-permiso 
        public static void AplicarPermisosControles(Control raiz, HashSet<string> claves)
        {
            if (raiz == null) return;

            foreach (Control ctrl in raiz.Controls)
            {
                // 1) Control web con data-permiso (Button, LinkButton, TextBox, ...)
                string clave = LeerAtributo(ctrl, "data-permiso");
                if (!string.IsNullOrEmpty(clave))
                {
                    WebControl wc = ctrl as WebControl;
                    if (wc != null) wc.Enabled = claves.Contains(clave);
                }

                // 2) Items de listas con data-permiso (RadioButtonList / CheckBoxList / DropDownList)
                ListControl lista = ctrl as ListControl;
                if (lista != null)
                {
                    foreach (ListItem item in lista.Items)
                    {
                        string claveItem = item.Attributes["data-permiso"];
                        if (!string.IsNullOrEmpty(claveItem))
                            item.Enabled = claves.Contains(claveItem);
                    }
                }

                // 3) Recursión en contenedores
                if (ctrl.HasControls())
                    AplicarPermisosControles(ctrl, claves);
            }
        }

        //  Menú (navbar) con data-pagina
        public static void AplicarPermisosMenu(Control menuRaiz, HashSet<string> claves, Dictionary<string, string[]> paginaPermisos)
        {
            if (menuRaiz == null) return;

            // Paso 1: muestro/oculto cada item que apunta a una página (data-pagina)
            ItemsPagina(menuRaiz, claves, paginaPermisos);

            // Paso 2: un dropdown padre queda visible sólo si le quedó algún hijo visible
            DropdownsVisibles(menuRaiz);
        }

        public static bool TieneAccesoAPagina(string pagina, HashSet<string> claves, Dictionary<string, string[]> paginaPermisos)
        {
            if (string.IsNullOrEmpty(pagina) || paginaPermisos == null) return false;

            string[] requeridas;
            if (!paginaPermisos.TryGetValue(pagina, out requeridas)) return false;

            return requeridas.Any(claves.Contains);
        }

        // ---------- privados ----------
        private static void ItemsPagina(Control raiz, HashSet<string> claves, Dictionary<string, string[]> mapa)
        {
            foreach (Control ctrl in raiz.Controls)
            {
                HtmlControl hc = ctrl as HtmlControl;
                if (hc != null)
                {
                    string pagina = hc.Attributes["data-pagina"];
                    if (!string.IsNullOrEmpty(pagina))
                        hc.Visible = TieneAccesoAPagina(pagina, claves, mapa);
                }

                if (ctrl.HasControls())
                    ItemsPagina(ctrl, claves, mapa);
            }
        }

        // Devuelve true si dentro de 'raiz' quedó algún <li> de menú visible.
        // Un <li> dropdown se hace visible sólo si contiene algún hijo visible.
        private static bool DropdownsVisibles(Control raiz)
        {
            bool hayVisible = false;

            foreach (Control ctrl in raiz.Controls)
            {
                HtmlGenericControl li = ctrl as HtmlGenericControl;
                bool esLi = li != null && string.Equals(li.TagName, "li", StringComparison.OrdinalIgnoreCase);

                if (esLi && EsDropdown(li))
                {
                    // Padre: visible si alguno de sus hijos quedó visible
                    li.Visible = DropdownsVisibles(li);
                    if (li.Visible) hayVisible = true;
                }
                else if (esLi)
                {
                    // <li> hoja: los gated ya se setearon en el paso 1;
                    // los públicos (sin data-pagina) quedan visibles y cuentan.
                    if (li.Visible) hayVisible = true;
                }
                else if (ctrl.HasControls())
                {
                    // Contenedores intermedios (form, ul no-server, etc.): sigo bajando
                    if (DropdownsVisibles(ctrl)) hayVisible = true;
                }
            }

            return hayVisible;
        }

        private static bool EsDropdown(HtmlGenericControl li)
        {
            string clase = li.Attributes["class"];
            if (string.IsNullOrEmpty(clase)) return false;
            return clase.Split(' ').Contains("dropdown");
        }

        private static string LeerAtributo(Control ctrl, string nombre)
        {
            WebControl wc = ctrl as WebControl;
            if (wc != null) return wc.Attributes[nombre];

            HtmlControl hc = ctrl as HtmlControl;
            if (hc != null) return hc.Attributes[nombre];

            return null;
        }
    }
}
