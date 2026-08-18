function limpiarFormulario() {
    document.getElementById(ids.dni).value = '';
    document.getElementById(ids.nombre).value = '';
    document.getElementById(ids.apellido).value = '';
    document.getElementById(ids.telefono).value = '';
    document.getElementById(ids.edad).value = '';
    document.getElementById(ids.domicilio).value = '';
    document.getElementById(ids.mascotas).checked = false;
}

function ocultarAlerta(inmediato = false) {
    var panel = document.getElementById(ids.alerta);
    if (panel) {
        if (inmediato) {
            panel.style.display = 'none';
        } else {
            setTimeout(function () {
                panel.style.display = 'none';
            }, 5000);
        }
    }
}

function salir() {
    window.location.href = 'MenuPrincipal.aspx';
}