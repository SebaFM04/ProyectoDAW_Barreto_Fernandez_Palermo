(function () {
    // Guarda/restaura el scroll interno de cada árbol (.pf-tree) entre postbacks.
    function arboles() { return document.querySelectorAll('.pf-tree'); }

    function restaurar() {
        arboles().forEach(function (el) {
            if (!el.id) return;
            var v = sessionStorage.getItem('scroll-' + el.id);
            if (v !== null) el.scrollTop = parseInt(v, 10);
        });
    }

    function guardarAlScrollear() {
        arboles().forEach(function (el) {
            if (!el.id) return;
            el.addEventListener('scroll', function () {
                sessionStorage.setItem('scroll-' + el.id, el.scrollTop);
            });
        });
    }

    window.addEventListener('load', function () {
        restaurar();
        guardarAlScrollear();
    });
})();