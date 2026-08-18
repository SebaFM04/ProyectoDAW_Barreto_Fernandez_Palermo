document.addEventListener("DOMContentLoaded", function () {
    const dropdowns = document.querySelectorAll(".dropdown");
    const toggleMenu = document.getElementById("btnToggleMenu");
    const navbarMenu = document.getElementById("navbarMenu");

    // Dropdowns
    dropdowns.forEach(dropdown => {
        const toggle = dropdown.querySelector(".dropdown-toggle");
        toggle.addEventListener("click", function (e) {
            e.preventDefault();
            dropdowns.forEach(d => {
                if (d !== dropdown) d.classList.remove("open");
            });
            dropdown.classList.toggle("open");
        });
    });

    // Click afuera
    document.addEventListener("click", function (e) {
        dropdowns.forEach(dropdown => {
            if (!dropdown.contains(e.target)) {
                dropdown.classList.remove("open");
            }
        });
    });

    // Menú hamburguesa
    if (toggleMenu && navbarMenu) {
        toggleMenu.addEventListener("click", function () {
            navbarMenu.classList.toggle("active");
        });
    }

    // ===== Multi-idioma: traducción client-side =====
    // Reasignar .Text a un asp:Label desde el servidor no está sobreviviendo
    // hasta el render final en este proyecto (asp:Button sí, porque su texto
    // va en el atributo value). En vez de pelear más contra eso, se pide acá
    // el diccionario de traducciones del formulario actual (vía PageMethods,
    // que expone ScriptManager) y se pisa directo el texto visible de cada
    // elemento por su id.
    aplicarTraducciones();
});

function aplicarTraducciones() {
    var nombreFormulario = document.body.getAttribute("data-formulario");
    if (!nombreFormulario) return;

    // PageMethods lo genera automáticamente el <asp:ScriptManager
    // EnablePageMethods="true"> agregado en MasterPage.master /
    // MasterPageLogin.master, según cuál esté activa en esta página. Si por
    // algún motivo el ScriptManager no llegó a cargar todavía, no rompe
    // nada: se deja el texto que ya trae el HTML (fallback español).
    if (typeof PageMethods === "undefined" || !PageMethods.ObtenerTraducciones) return;

    PageMethods.ObtenerTraducciones(nombreFormulario, function (resultadoJson) {
        var traducciones;
        try {
            traducciones = JSON.parse(resultadoJson);
        } catch (e) {
            return;
        }

        Object.keys(traducciones).forEach(function (idControl) {
            var el = document.getElementById(idControl);
            if (!el) return;

            var texto = traducciones[idControl];

            // asp:Button / asp:TextBox renderizan <input>, con el texto en
            // el atributo value. asp:Label/LinkButton/HyperLink renderizan
            // <span>/<a>, con el texto como contenido.
            var tag = el.tagName.toUpperCase();
            if (tag === "INPUT") {
                el.value = texto;
            } else {
                el.textContent = texto;
            }
        });
    }, function (error) {
        // Error de red o de servidor: se deja el texto que ya trae el HTML.
    });
}