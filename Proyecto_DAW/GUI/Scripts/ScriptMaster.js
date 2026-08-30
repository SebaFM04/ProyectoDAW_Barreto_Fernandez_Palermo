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
    //
    // PageMethods lo genera un <script> que el ScriptManager inyecta en la
    // página, y ese script puede terminar de cargar DESPUÉS de
    // DOMContentLoaded (confirmado con diagnóstico: en este proyecto
    // "typeof PageMethods" da "undefined" en ese momento). En vez de asumir
    // que ya está listo, se reintenta con un pequeño delay hasta que
    // aparezca, con un tope de intentos para no reintentar para siempre si
    // de verdad no está disponible (por ejemplo, EnablePageMethods mal
    // configurado).
    esperarPageMethodsYAplicarTraducciones();
});

function esperarPageMethodsYAplicarTraducciones() {
    var intentosMaximos = 20;   // 20 x 100ms = hasta 2 segundos de espera
    var intentoActual = 0;

    function intentar() {
        intentoActual++;

        if (typeof PageMethods !== "undefined" && PageMethods.ObtenerTraducciones) {
            aplicarTraducciones();
            return;
        }

        if (intentoActual >= intentosMaximos) {
            // PageMethods nunca apareció: se deja el texto que ya trae el
            // HTML (fallback español), sin romper la página.
            return;
        }

        setTimeout(intentar, 100);
    }

    intentar();
}

function aplicarTraducciones() {
    var nombreFormulario = document.body.getAttribute("data-formulario");
    if (!nombreFormulario) return;

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